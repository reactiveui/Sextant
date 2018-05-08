using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Splat;
using Xamarin.Forms;

namespace Sextant
{
	[DebuggerDisplay("{ViewType.Name, ViewModelType.Name}")]
	public sealed class NavigationElement
	{
		public NavigationElement(Type viewType, Type viewModelType, Type navigationViewType, Type navigationViewModelType, Func<object> viewModelCreationFunc = null)
		{
			NavigationViewType = navigationViewType;
			NavigationViewModelType = navigationViewModelType;
			ViewType = viewType;
			ViewModelType = viewModelType;
			ViewModelCreationFunc = viewModelCreationFunc;
		}

		public NavigationElement(Type viewType, Type viewModelType, Func<object> viewModelCreationFunc = null)
			: this(null, null, viewType, viewModelType, viewModelCreationFunc)
		{ }


		public Type NavigationViewType
		{
			get;
			set;
		}

		public Type NavigationViewModelType
		{
			get;
			set;
		}

		public Type ViewType
		{
			get;
			set;
		}

		public Type ViewModelType
		{
			get;
			set;
		}

		public Func<object> ViewCreationFunc
		{
			get;
			set;
		}

		public Func<object> ViewModelCreationFunc
        {
            get;
            set;
        }
	}



	public abstract class SextantNavigationServiceBase : ISextantNavigationServiceBase
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

		readonly IList<NavigationElement> _navigationDeck = new List<NavigationElement>();
		//readonly Dictionary<Type, Type> _navigationPageModelTypes = new Dictionary<Type, Type>();
		readonly Dictionary<Type, Type> _viewModelTypes = new Dictionary<Type, Type>();
		//readonly Dictionary<Type, Func<object>> _pageCreation = new Dictionary<Type, Func<object>>();
		//readonly Dictionary<Type, Func<object>> _pageModelCreation = new Dictionary<Type, Func<object>>();
		readonly ConditionalWeakTable<IBaseNavigationPageModel, object> _weakPageCache = new ConditionalWeakTable<IBaseNavigationPageModel, object>();

		public SextantNavigationServiceBase(Application appInstance, bool automaticAssembliesDiscovery = true, params Assembly[] additionalPagesAssemblies)
		{
			if (automaticAssembliesDiscovery)
			{
				var pagesAssemblies = additionalPagesAssemblies.ToList();
				pagesAssemblies.Add(appInstance.GetType().GetTypeInfo().Assembly);
				//AutomaticPageRegister(pagesAssemblies);
			}
		}

		//private void AutomaticPageRegister(List<Assembly> pagesAssemblies)
		//{
		//    foreach (var assembly in pagesAssemblies.Distinct())
		//    {
		//        foreach (var pageTypeInfo in assembly.DefinedTypes.Where(t => t.IsClass && !t.IsAbstract
		//             && t.ImplementedInterfaces != null && !t.IsGenericTypeDefinition))
		//        {
		//            var found = pageTypeInfo.ImplementedInterfaces.FirstOrDefault(t => t.IsConstructedGenericType &&
		//                t.GetGenericTypeDefinition() == typeof(IBaseNavigationPage<>));

		//            if (found != default(Type))
		//            {
		//                var pageType = pageTypeInfo.AsType();
		//                var pageModelType = found.GenericTypeArguments.First();

		//                if (!_navigationPageModelTypes.ContainsKey(pageModelType))
		//                {
		//                    _navigationPageModelTypes.Add(pageModelType, pageType);
		//                }
		//                else
		//                {
		//                    var oldPageType = _navigationPageModelTypes[pageModelType];

		//                    if (pageTypeInfo.IsSubclassOf(oldPageType))
		//                    {
		//                        _navigationPageModelTypes.Remove(pageModelType);
		//                        _navigationPageModelTypes.Add(pageModelType, pageType);
		//                    }
		//                }
		//            }

		//            var foundView = pageTypeInfo.ImplementedInterfaces.FirstOrDefault(t => t.IsConstructedGenericType &&
		//                t.GetGenericTypeDefinition() == typeof(IBaseView<>));

		//            if (foundView != default(Type))
		//            {
		//                var viewType = pageTypeInfo.AsType();
		//                var viewModelType = foundView.GenericTypeArguments.First();

		//                SetViewModelType(pageTypeInfo, viewType, viewModelType);
		//            }
		//        }
		//    }
		//}

		public virtual void RegisterPage<TPage, TPageModel>(Func<TPageModel> createPageModel = null)
			where TPageModel : class, IBaseNavigationPageModel, new()
			where TPage : class, IBaseNavigationPage<TPageModel>, new()
		{
			_navigationDeck.Add(new NavigationElement(typeof(TPage), typeof(TPageModel), createPageModel));

			Locator.CurrentMutable.Register(() => new TPage(), typeof(TPage));
			Locator.CurrentMutable.Register(() => new TPageModel(), typeof(TPageModel));
		}

		public virtual void RegisterPage<TPage, TPageModel, TNavigationPage, TNavigationPageModel>()
			where TPage : class, IBaseNavigationPage<TPageModel>, new()
			where TPageModel : class, IBaseNavigationPageModel, new()
			where TNavigationPage : class, IBaseNavigationPage<TNavigationPageModel>, new()
			where TNavigationPageModel : class, IBaseNavigationPageModel, new()
		{
			_navigationDeck.Add(new NavigationElement(typeof(TPage), typeof(TPageModel), typeof(TNavigationPage), typeof(TNavigationPageModel)));


			var navCreation = new Func<IBaseNavigationPage<IBaseNavigationPageModel>, IBaseNavigationPage<TNavigationPageModel>>(
				(page) => Activator.CreateInstance(typeof(TNavigationPage), page) as IBaseNavigationPage<TNavigationPageModel>);


			Locator.CurrentMutable.Register(() => new TPage(), typeof(TPage));
			Locator.CurrentMutable.Register(() => new TPageModel(), typeof(TPageModel));
			Locator.CurrentMutable.Register(() => navCreation(Locator.Current.GetService<TPage>()), typeof(TNavigationPage));
			Locator.CurrentMutable.Register(() => new TNavigationPageModel(), typeof(TNavigationPageModel));
		}


		public virtual IBaseNavigationPage<TPageModel> GetPage<TPageModel>(TPageModel setPageModel = null) where TPageModel : class, IBaseNavigationPageModel
		{
			var navigationElement = _navigationDeck.FirstOrDefault(p => p.ViewModelType == typeof(TPageModel));
			IBaseNavigationPage<TPageModel> page;

			page = Locator.Current.GetService(navigationElement.ViewType) as IBaseNavigationPage<TPageModel>;         
			if (page == null)
            {
				throw new NoPageForPageModelRegisteredException("View not registered in IOC: " + navigationElement.ViewType.Name);
            }

			var pageModel = Locator.Current.GetService(navigationElement.ViewModelType) as TPageModel;
			if (setPageModel != null)
            {
				throw new NoPageForPageModelRegisteredException("ViewModel not registered in IOC: " + navigationElement.ViewModelType.Name);
            }

			SetPageModel(page, setPageModel);

			return page;
		}

		public virtual IBaseNavigationPage<TNavigationViewModel> GetNavigationPage<TNavigationViewModel>(TNavigationViewModel setPageModel = null) where TNavigationViewModel : class, IBaseNavigationPageModel
        {
			var navigationElement = _navigationDeck.FirstOrDefault(p => p.NavigationViewModelType == typeof(TNavigationViewModel));
			IBaseNavigationPage<TNavigationViewModel> page;

			page = Locator.Current.GetService(navigationElement.ViewType) as IBaseNavigationPage<TNavigationViewModel>;
            if (page == null)
            {
                throw new NoPageForPageModelRegisteredException("View not registered in IOC: " + navigationElement.ViewType.Name);
            }

			var pageModel = Locator.Current.GetService(navigationElement.ViewModelType) as TNavigationViewModel;
            if (setPageModel != null)
            {
                throw new NoPageForPageModelRegisteredException("ViewModel not registered in IOC: " + navigationElement.ViewModelType.Name);
            }

            SetPageModel(page, setPageModel);

            return page;
        }

		//public IBaseNavigationPage<IBaseNavigationPageModel> GetPage(Type pageModelType)
		//{
        //    return 
        //}
        
		internal Type GetPageType(Type pageModelType)
		{         
			return _navigationDeck.FirstOrDefault(p => p.ViewModelType == pageModelType)?.ViewType;
		}

		internal Type GetViewType(Type viewModelType)
		{
			Type viewType = null;
			if (_viewModelTypes.TryGetValue(viewModelType, out viewType))
				return viewType;

			return null;
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