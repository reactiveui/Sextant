using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Splat;
using Xamarin.Forms;

namespace Sextant
{
    public class Factory : IBaseFactory
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
        readonly ConditionalWeakTable<IBasePageModel, object> _weakPageCache = new ConditionalWeakTable<IBasePageModel, object>();

        public Factory(Application appInstance, bool automaticAssembliesDiscovery = true, params Assembly[] additionalPagesAssemblies)
        {
            var pagesAssemblies = additionalPagesAssemblies.ToList();

            if (automaticAssembliesDiscovery)
                pagesAssemblies.Add(appInstance.GetType().GetTypeInfo().Assembly);

            foreach (var assembly in pagesAssemblies.Distinct())
            {
                foreach (var pageTypeInfo in assembly.DefinedTypes.Where(t => t.IsClass && !t.IsAbstract
                     && t.ImplementedInterfaces != null && !t.IsGenericTypeDefinition))
                {
                    var found = pageTypeInfo.ImplementedInterfaces.FirstOrDefault(t => t.IsConstructedGenericType &&
                        t.GetGenericTypeDefinition() == typeof(IBasePage<>));

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

        public virtual void RegisterPage<TPageModel>(Func<TPageModel> createPageModel = null, Func<IBasePage<TPageModel>> createPage = null) where TPageModel : class, IBasePageModel
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

        public virtual void RegisterNavigationPage<TNavPageModel, TInitalPageModel>()
                                                                                    where TNavPageModel : class, IBasePageModel
                                                                                    where TInitalPageModel : class, IBasePageModel
        {
            RegisterNavigationPage<TNavPageModel>(() => GetPage<TInitalPageModel>(), null, null);
        }

        public virtual void RegisterNavigationPage<TNavPageModel>(  Func<IBasePage<IBasePageModel>> initialPage = null,
                                                                    Func<TNavPageModel> createNavModel = null,
                                                                    Func<IBasePage<IBasePageModel>, IBasePage<TNavPageModel>> createNav = null)
                                                                    where TNavPageModel : class, IBasePageModel
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
            var createNavWithPage = new Func<IBasePage<TNavPageModel>>(() =>
            {
                // Take the initial page and invoke it. The page is passed in as a lambda expression that returns something of type IBasePage<T>
                var page = initialPage?.Invoke();
                return createNav(page);
            });

            Func<object> foundPageCreation = null;
            if (_pageCreation.TryGetValue(typeof(TNavPageModel), out foundPageCreation))
                _pageCreation[typeof(TNavPageModel)] = createNavWithPage;
            else
                _pageCreation.Add(typeof(TNavPageModel), createNavWithPage);
        }

        public virtual IBasePage<TPageModel> GetPage<TPageModel>(TPageModel setPageModel = null) where TPageModel : class, IBasePageModel
        {
            var pageModelType = typeof(TPageModel);
            var pageType = GetPageType(pageModelType);

            IBasePage<TPageModel> page;
            Func<object> pageCreationFunc;
            _pageCreation.TryGetValue(pageModelType, out pageCreationFunc);

            // first check if we have a registered PageCreation method for this pageModelType
            if (pageCreationFunc != null)
            {
                page = pageCreationFunc() as IBasePage<TPageModel>;
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
                page = Locator.Current.GetService(pageType) as IBasePage<TPageModel>;

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

        public IBasePage<IBasePageModel> GetPage(Type pageModelType)
        {
            var pageType = GetPageType(pageModelType);
            IBasePage<IBasePageModel> page;
            Func<object> pageCreationFunc;
            if (_pageCreation.TryGetValue(pageModelType, out pageCreationFunc))
            {
                page = pageCreationFunc() as IBasePage<IBasePageModel>;
            }
            else
                page = Locator.Current.GetService(pageType) as IBasePage<IBasePageModel>;

            Func<object> pageModelCreationFunc;
            if (_pageModelCreation.TryGetValue(pageModelType, out pageModelCreationFunc))
                SetPageModel(page, pageModelCreationFunc() as IBasePageModel);
            else
                SetPageModel(page, Locator.Current.GetService(pageModelType) as IBasePageModel);

            return page;
        }

        internal void AddToWeakCacheIfNotExists<TPageModel>(IBasePage<TPageModel> page, TPageModel pageModel) where TPageModel : class, IBasePageModel
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

        internal Type GetPageModelType(IBasePage<IBasePageModel> page)
        {
            var found = page.GetType().GetTypeInfo().ImplementedInterfaces.FirstOrDefault(t => t.IsConstructedGenericType && t.GetGenericTypeDefinition() == typeof(IBasePage<>));
            var viewModelType = found.GenericTypeArguments.First();
            return viewModelType;
        }

        public virtual IBasePage<TPageModel> GetPageByModel<TPageModel>(TPageModel pageModel) where TPageModel : class, IBasePageModel
        {
            object page = null;
            _weakPageCache.TryGetValue(pageModel, out page);
            return page as IBasePage<TPageModel>;
        }

        public virtual TPageModel GetPageModel<TPageModel>(IBasePage<TPageModel> page) where TPageModel : class, IBasePageModel
        {
            var xfPage = page as Page;
            return xfPage?.BindingContext as TPageModel;
        }

        public virtual void SetPageModel<TPageModel>(IBasePage<TPageModel> page, TPageModel newPageModel) where TPageModel : class, IBasePageModel
        {
            var formsPage = (Page)page;

            var oldVisChange = formsPage.BindingContext as IPageVisibilityChange;
            if (oldVisChange != null)
            {
                formsPage.Appearing -= FormsPage_Appearing;
                formsPage.Disappearing -= FormsPage_Disappearing;
            }

            formsPage.BindingContext = newPageModel;

            var newVisChange = newPageModel as IPageVisibilityChange;
            if (newVisChange != null)
            {
                formsPage.Appearing += FormsPage_Appearing;
                formsPage.Disappearing += FormsPage_Disappearing;
            }

            AddToWeakCacheIfNotExists(page, newPageModel);
        }

        void FormsPage_Appearing(object sender, EventArgs e)
        {
            var model = ((sender as Page).BindingContext as IPageVisibilityChange);
            if (model != null)
            {
                model.OnAppearing();
            }
        }

        void FormsPage_Disappearing(object sender, EventArgs e)
        {
            var model = ((sender as Page).BindingContext as IPageVisibilityChange);
            if (model != null)
            {
                model.OnDisappearing();
            }
        }




        // navigation 

        public async virtual Task<bool> PushPageAsync<TCurrentPageModel, TPageModel>(IBasePage<TCurrentPageModel> currentPage, IBasePage<TPageModel> pageToPush, bool animated = true) where TCurrentPageModel : class, IBasePageModel where TPageModel : class, IBasePageModel
        {
            var navigation = ((Page)currentPage)?.Navigation;
            var navEventsPage = pageToPush as INavigationCanPush;
            if (navigation == null || (navEventsPage != null && !navEventsPage.NavigationCanPush()))
                return false;

            var navEventsPageModel = pageToPush.GetPageModel() as INavigationCanPush;
            if (navEventsPageModel != null && !navEventsPageModel.NavigationCanPush())
                return false;

            await navigation.PushAsync((Page)pageToPush, animated);

            var navEventsPage2 = pageToPush as INavigationPushed;
            if (navEventsPage2 != null)
                navEventsPage2.NavigationPushed();

            var navEventsPageModel2 = pageToPush.GetPageModel() as INavigationPushed;
            if (navEventsPageModel2 != null)
                navEventsPageModel2.NavigationPushed();

            return true;
        }

        public async virtual Task<bool> PushModalPageAsync<TCurrentPageModel, TPageModel>(IBasePage<TCurrentPageModel> currentPage, IBasePage<TPageModel> pageToPush, bool animated = true) where TCurrentPageModel : class, IBasePageModel where TPageModel : class, IBasePageModel
        {
            var navigation = ((Page)currentPage)?.Navigation;
            var navEventsPage = pageToPush as INavigationCanPush;
            if (navigation == null || (navEventsPage != null && !navEventsPage.NavigationCanPush()))
                return false;

            var navEventsPageModel = pageToPush.GetPageModel() as INavigationCanPush;
            if (navEventsPageModel != null && !navEventsPageModel.NavigationCanPush())
                return false;

            await navigation.PushModalAsync((Page)pageToPush, animated);


            var navEventsPage2 = pageToPush as INavigationPushed;
            if (navEventsPage2 != null)
                navEventsPage2.NavigationPushed();

            var navEventsPageModel2 = pageToPush.GetPageModel() as INavigationPushed;
            if (navEventsPageModel2 != null)
                navEventsPageModel2.NavigationPushed();

            return true;
        }

        public virtual Task<bool> InsertPageBeforeAsync<TPageModel, TBeforePageModel>(IBasePage<TPageModel> pageToInsert, IBasePage<TBeforePageModel> beforePage) where TPageModel : class, IBasePageModel where TBeforePageModel : class, IBasePageModel
        {
            var navigation = ((Page)beforePage)?.Navigation;
            var navEventsPage = pageToInsert as INavigationCanInsert;
            if (navigation == null || (navEventsPage != null && !navEventsPage.NavigationCanInsert()))
                return Task.FromResult(false);

            var navEventsPageModel = pageToInsert.GetPageModel() as INavigationCanInsert;
            if (navEventsPageModel != null && !navEventsPageModel.NavigationCanInsert())
                return Task.FromResult(false);

            navigation.InsertPageBefore((Page)pageToInsert, (Page)beforePage);

            var navEventsPage2 = pageToInsert as INavigationInserted;
            if (navEventsPage2 != null)
                navEventsPage2.NavigationInserted();

            var navEventsPageModel2 = pageToInsert.GetPageModel() as INavigationInserted;
            if (navEventsPageModel2 != null)
                navEventsPageModel2.NavigationInserted();

            return Task.FromResult(true);
        }

        public async virtual Task<bool> PopPageAsync<TCurrentPageModel>(IBasePage<TCurrentPageModel> currentPage, bool animated = true) where TCurrentPageModel : class, IBasePageModel
        {
            var navigation = ((Page)currentPage)?.Navigation;
            var navEventsPage = currentPage as INavigationCanPop;

            if (navigation == null || (navEventsPage != null && !navEventsPage.NavigationCanPop()))
                return false;

            var navEventsPageModel = currentPage.GetPageModel() as INavigationCanPop;
            if (navEventsPageModel != null && !navEventsPageModel.NavigationCanPop())
                return false;

            (navEventsPageModel as IDisposable)?.Dispose();

            await navigation.PopAsync(animated);

            var navEventsPage2 = currentPage as INavigationPopped;
            if (navEventsPage2 != null)
                navEventsPage2.NavigationPopped();

            var navEventsPageModel2 = currentPage.GetPageModel() as INavigationPopped;
            if (navEventsPageModel2 != null)
                navEventsPageModel2.NavigationPopped();

            return true;
        }

        public async virtual Task<bool> PopModalPageAsync<TCurrentPageModel>(IBasePage<TCurrentPageModel> currentPage, bool animated = true) where TCurrentPageModel : class, IBasePageModel
        {
            var navigation = ((Page)currentPage)?.Navigation;
            var navEventsPage = currentPage as INavigationCanPop;

            if (navigation == null || (navEventsPage != null && !navEventsPage.NavigationCanPop()))
                return false;

            var navEventsPageModel = currentPage.GetPageModel() as INavigationCanPop;
            if (navEventsPageModel != null && !navEventsPageModel.NavigationCanPop())
                return false;

            (navEventsPageModel as IDisposable)?.Dispose();

            await navigation.PopModalAsync(animated);

            var navEventsPage2 = currentPage as INavigationPopped;
            if (navEventsPage2 != null)
                navEventsPage2.NavigationPopped();

            var navEventsPageModel2 = currentPage.GetPageModel() as INavigationPopped;
            if (navEventsPageModel2 != null)
                navEventsPageModel2.NavigationPopped();

            return true;
        }

        public virtual Task<bool> RemovePageAsync<TCurrentPageModel, TPageModel>(IBasePage<TCurrentPageModel> currentPage, IBasePage<TPageModel> pageToRemove) where TCurrentPageModel : class, IBasePageModel where TPageModel : class, IBasePageModel
        {
            var navigation = ((Page)currentPage)?.Navigation;
            var navEventsPage = pageToRemove as INavigationCanRemove;
            if (navigation == null || (navEventsPage != null && !navEventsPage.NavigationCanRemove()))
                return Task.FromResult(false);

            var navEventsPageModel = pageToRemove.GetPageModel() as INavigationCanRemove;
            if (navEventsPageModel != null && !navEventsPageModel.NavigationCanRemove())
                return Task.FromResult(false);

            (navEventsPageModel as IDisposable)?.Dispose();

            navigation.RemovePage((Page)pageToRemove);

            var navEventsPage2 = pageToRemove as INavigationRemoved;
            if (navEventsPage2 != null)
                navEventsPage2.NavigationRemoved();

            var navEventsPageModel2 = pageToRemove.GetPageModel() as INavigationRemoved;
            if (navEventsPageModel2 != null)
                navEventsPageModel2.NavigationRemoved();

            return Task.FromResult(true);
        }

        public async virtual Task<bool> PopPagesToRootAsync<TCurrentPageModel>(IBasePage<TCurrentPageModel> currentPage, bool animated = true) where TCurrentPageModel : class, IBasePageModel
        {
            var navigation = ((Page)currentPage)?.Navigation;
            if (navigation == null)
                return false;

            await navigation.PopToRootAsync(animated);

            return true;
        }

        public virtual Task<bool> SetNewRootAndResetAsync<TPageModel>(IBasePage<TPageModel> newRootPage) where TPageModel : class, IBasePageModel
        {
            Application.Current.MainPage = (Page)newRootPage;

            return Task.FromResult(true);
        }

        public virtual Task<bool> SetNewRootAndResetAsync<TPageModelOfNewRoot>() where TPageModelOfNewRoot : class, IBasePageModel
        {
            Application.Current.MainPage = (Page)GetPage<TPageModelOfNewRoot>();

            return Task.FromResult(true);
        }
    }
}