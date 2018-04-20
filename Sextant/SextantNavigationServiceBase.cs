using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Splat;
using Xamarin.Forms;

namespace Sextant
{
    public class SextantNavigationServiceBase : ISextantNavigationServiceBase
    {
        IBaseLogger logger;
        public IBaseLogger Logger
        {
            get
            {
                if (logger == null)
                    logger = new BaseLogger();

                return logger;
            }

            set
            {
                logger = value;
            }
        }

        readonly Dictionary<Type, Type> _navigationPageModelTypes = new Dictionary<Type, Type>();
        readonly Dictionary<Type, Type> _viewModelTypes = new Dictionary<Type, Type>();
        readonly Dictionary<Type, Func<object>> _pageCreation = new Dictionary<Type, Func<object>>();
        readonly Dictionary<Type, Func<object>> _pageModelCreation = new Dictionary<Type, Func<object>>();
        readonly ConditionalWeakTable<IBaseNavigationPageModel, object> _weakPageCache = new ConditionalWeakTable<IBaseNavigationPageModel, object>();

        public SextantNavigationServiceBase(Application appInstance, bool automaticAssembliesDiscovery = true, params Assembly[] additionalPagesAssemblies)
        {
            if (automaticAssembliesDiscovery)
            {
                var pagesAssemblies = additionalPagesAssemblies.ToList();
                pagesAssemblies.Add(appInstance.GetType().GetTypeInfo().Assembly);
                AutomaticPageRegister(pagesAssemblies);
            }
        }

        private void AutomaticPageRegister(List<Assembly> pagesAssemblies)
        {
            foreach (var assembly in pagesAssemblies.Distinct())
            {
                foreach (var pageTypeInfo in assembly.DefinedTypes.Where(t => t.IsClass && !t.IsAbstract
                     && t.ImplementedInterfaces != null && !t.IsGenericTypeDefinition))
                {
                    var found = pageTypeInfo.ImplementedInterfaces.FirstOrDefault(t => t.IsConstructedGenericType &&
                        t.GetGenericTypeDefinition() == typeof(IBaseNavigationPage<>));

                    if (found != default(Type))
                    {
                        var pageType = pageTypeInfo.AsType();
                        var pageModelType = found.GenericTypeArguments.First();

                        if (!_navigationPageModelTypes.ContainsKey(pageModelType))
                        {
                            _navigationPageModelTypes.Add(pageModelType, pageType);
                        }
                        else
                        {
                            var oldPageType = _navigationPageModelTypes[pageModelType];

                            if (pageTypeInfo.IsSubclassOf(oldPageType))
                            {
                                _navigationPageModelTypes.Remove(pageModelType);
                                _navigationPageModelTypes.Add(pageModelType, pageType);
                            }
                        }
                    }

                    var foundView = pageTypeInfo.ImplementedInterfaces.FirstOrDefault(t => t.IsConstructedGenericType &&
                        t.GetGenericTypeDefinition() == typeof(IBaseView<>));

                    if (foundView != default(Type))
                    {
                        var viewType = pageTypeInfo.AsType();
                        var viewModelType = foundView.GenericTypeArguments.First();

                        SetViewModelType(pageTypeInfo, viewType, viewModelType);
                    }
                }
            }
        }

        public virtual void RegisterPage<TPageModel>(   Func<TPageModel> createPageModel = null, 
                                                        Func<IBaseNavigationPage<TPageModel>> createPage = null) 
                                                        where TPageModel : class, IBaseNavigationPageModel
        {
            if (createPageModel != null)
            {
                Func<object> found = null;
                if (_pageModelCreation.TryGetValue(typeof(TPageModel), out found))
                    _pageModelCreation[typeof(TPageModel)] = createPageModel;
                else
                    _pageModelCreation.Add(typeof(TPageModel), createPageModel);
            }

            if (createPage != null)
            {
                Func<object> found = null;
                if (_pageCreation.TryGetValue(typeof(TPageModel), out found))
                    _pageCreation[typeof(TPageModel)] = createPage;
                else
                    _pageCreation.Add(typeof(TPageModel), createPage);
            }

            SetViewModelType(null, typeof(TPageModel), typeof(TPageModel));
        }

        public virtual void RegisterNavigationPage<TNavPageModel>(  Func<IBaseNavigationPage<IBaseNavigationPageModel>> initialPage = null,
                                                                    Func<TNavPageModel> createNavModel = null,
                                                                    Func<IBaseNavigationPage<IBaseNavigationPageModel>, IBaseNavigationPage<TNavPageModel>> createNav = null)
                                                                    where TNavPageModel : class, IBaseNavigationPageModel
        {
            // This defaults to null, when is it used?
            if (createNavModel != null)
            {
                Func<object> found = null;
                if (_pageModelCreation.TryGetValue(typeof(TNavPageModel), out found))
                    _pageModelCreation[typeof(TNavPageModel)] = createNavModel;
                else
                    _pageModelCreation.Add(typeof(TNavPageModel), createNavModel);
            }

            // This defaults to null, when is it used?
            if (createNav == null)
            {
                var pageModelType = typeof(TNavPageModel);
                var pageType = GetPageType(pageModelType) ?? typeof(BaseNavigationPage<TNavPageModel>);
                _navigationPageModelTypes[pageModelType] = pageType;
            }

            // this creates a new lambda function that will be later invoked
            var createNavWithPage = new Func<IBaseNavigationPage<TNavPageModel>>(() =>
            {
                // Take the initial page and invoke it. The page is passed in as a lambda expression that returns something of type IBaseNavigationPage<T>
                var page = initialPage?.Invoke();
                return createNav(page);
            });

            Func<object> foundPageCreation = null;
            if (_pageCreation.TryGetValue(typeof(TNavPageModel), out foundPageCreation))
                _pageCreation[typeof(TNavPageModel)] = createNavWithPage;
            else
                _pageCreation.Add(typeof(TNavPageModel), createNavWithPage);
        }

        public virtual void RegisterNavigationPage<TNavPageModel, TInitalPageModel>()
                                                                            where TNavPageModel : class, IBaseNavigationPageModel
                                                                            where TInitalPageModel : class, IBaseNavigationPageModel
        {
            RegisterNavigationPage<TNavPageModel>(() => GetPage<TInitalPageModel>(), null, null);
        }


        public virtual IBaseNavigationPage<TPageModel> GetPage<TPageModel>(TPageModel setPageModel = null) where TPageModel : class, IBaseNavigationPageModel
        {
            var pageModelType = typeof(TPageModel);
            var pageType = GetPageType(pageModelType);

            IBaseNavigationPage<TPageModel> page;
            Func<object> pageCreationFunc;
            _pageCreation.TryGetValue(pageModelType, out pageCreationFunc);

            // first check if we have a registered PageCreation method for this pageModelType
            if (pageCreationFunc != null)
            {
                page = pageCreationFunc() as IBaseNavigationPage<TPageModel>;
            }
            else
            {
                // if not check if it failed because there was no registered PageType
                // we cannot check this earlier because for NavgationPages we allow PageModels of the NavigationPage not having a custom NavigationPage type
                // in this case we register a default creation method, but we won't register a pageType
                // !!!!!!!!! Not sure if this is the correct way to do it
                if (pageType == null)
                {
                    throw new NoPageForPageModelRegisteredException("No PageType Registered for PageModel type: " + pageModelType);
                }
                page = Locator.Current.GetService(pageType) as IBaseNavigationPage<TPageModel>;

                if (page == null)
                {
                    throw new NoPageForPageModelRegisteredException("PageType not registered in IOC: " + pageType);
                }
            }

            if (setPageModel != null)
            {
                SetPageModel(page, setPageModel);
            }
            else
            {
                Func<object> pageModelCreationFunc;
                if (_pageModelCreation.TryGetValue(pageModelType, out pageModelCreationFunc))
                    SetPageModel(page, pageModelCreationFunc() as TPageModel);
                else
                    SetPageModel(page, Locator.Current.GetService<TPageModel>());
            }

            return page;

            throw new NotImplementedException();
        }

        public IBaseNavigationPage<IBaseNavigationPageModel> GetPage(Type pageModelType)
        {
            var pageType = GetPageType(pageModelType);
            IBaseNavigationPage<IBaseNavigationPageModel> page;
            Func<object> pageCreationFunc;
            if (_pageCreation.TryGetValue(pageModelType, out pageCreationFunc))
            {
                page = pageCreationFunc() as IBaseNavigationPage<IBaseNavigationPageModel>;
            }
            else
                page = Locator.Current.GetService(pageType) as IBaseNavigationPage<IBaseNavigationPageModel>;

            Func<object> pageModelCreationFunc;
            if (_pageModelCreation.TryGetValue(pageModelType, out pageModelCreationFunc))
                SetPageModel(page, pageModelCreationFunc() as IBaseNavigationPageModel);
            else
                SetPageModel(page, Locator.Current.GetService(pageModelType) as IBaseNavigationPageModel);

            return page;
        }

        internal void AddToWeakCacheIfNotExists<TPageModel>(IBaseNavigationPage<TPageModel> page, TPageModel pageModel) where TPageModel : class, IBaseNavigationPageModel
        {
            if (pageModel == null)
                return;

            object weakExists;
            if (!_weakPageCache.TryGetValue(pageModel, out weakExists))
                _weakPageCache.Add(pageModel, page);
        }

        internal Type GetPageType(Type pageModelType)
        {
            Type pageType = null;
            if (_navigationPageModelTypes.TryGetValue(pageModelType, out pageType))
                return pageType;

            return null;
        }

        internal Type GetViewType(Type viewModelType)
        {
            Type viewType = null;
            if (_viewModelTypes.TryGetValue(viewModelType, out viewType))
                return viewType;

            return null;
        }

        private void SetViewModelType(TypeInfo pageTypeInfo, Type viewType, Type viewModelType)
        {
            if (!_viewModelTypes.ContainsKey(viewModelType))
            {
                _viewModelTypes.Add(viewModelType, viewType);
            }
            else if (pageTypeInfo != null)
            {
                var oldPageType = _viewModelTypes[viewModelType];

                if (pageTypeInfo.IsSubclassOf(oldPageType))
                {
                    _viewModelTypes.Remove(viewModelType);
                    _viewModelTypes.Add(viewModelType, viewType);
                }
            }
        }

        internal Type GetPageModelType(IBaseNavigationPage<IBaseNavigationPageModel> page)
        {
            var found = page.GetType().GetTypeInfo().ImplementedInterfaces.FirstOrDefault(t => t.IsConstructedGenericType && t.GetGenericTypeDefinition() == typeof(IBaseNavigationPage<>));
            var viewModelType = found.GenericTypeArguments.First();
            return viewModelType;
        }

        public virtual IBaseNavigationPage<TPageModel> GetPageByModel<TPageModel>(TPageModel pageModel) where TPageModel : class, IBaseNavigationPageModel
        {
            object page = null;
            _weakPageCache.TryGetValue(pageModel, out page);
            return page as IBaseNavigationPage<TPageModel>;
        }

        public virtual TPageModel GetPageModel<TPageModel>(IBaseNavigationPage<TPageModel> page) where TPageModel : class, IBaseNavigationPageModel
        {
            throw new NotImplementedException();
        }

        public virtual void SetPageModel<TPageModel>(IBaseNavigationPage<TPageModel> page, TPageModel newPageModel) where TPageModel : class, IBaseNavigationPageModel
        {
            throw new NotImplementedException();
        }
    }
}