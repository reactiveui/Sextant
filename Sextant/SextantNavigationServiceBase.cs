using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Splat;
using Xamarin.Forms;

namespace Sextant
{
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

		public SextantNavigationServiceBase(Application appInstance,
											bool automaticAssembliesDiscovery = true,
											params Assembly[] additionalPagesAssemblies)
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

		public virtual IBaseNavigationPage<TPageModel> GetPage<TPageModel>(TPageModel setPageModel = null)
			where TPageModel : class, IBaseNavigationPageModel
		{
			var navigationElement = _navigationDeck.FirstOrDefault(p => p.ViewModelType == typeof(TPageModel));
			IBaseNavigationPage<TPageModel> page;
			IBaseNavigationPageModel pageModel;
                     
			page = GetView<TPageModel>(navigationElement.ViewType);         

			if (setPageModel != null)
			{
				SetPageModel(page, setPageModel);
			}
			else
			{
				pageModel = GetViewModel<TPageModel>(navigationElement.ViewModelType);
				SetPageModel(page, pageModel);
			}

			return page;
		}

		private static IBaseNavigationPageModel GetViewModel<TPageModel>(Type viewModelType) where TPageModel : class, IBaseNavigationPageModel
		{
			var pageModel = Locator.Current.GetService(viewModelType) as TPageModel;
			if (pageModel == null)
			{
				throw new NoPageForPageModelRegisteredException("ViewModel not registered in IOC: " + viewModelType.Name);
			}

			return pageModel;
		}

		private static IBaseNavigationPage<TPageModel> GetView<TPageModel>(Type viewType) where TPageModel : class, IBaseNavigationPageModel
		{
			IBaseNavigationPage<TPageModel> page = Locator.Current.GetService(viewType) as IBaseNavigationPage<TPageModel>;
			if (page == null)
			{
				throw new NoPageForPageModelRegisteredException("View not registered in IOC: " + viewType.Name);
			}

			return page;
		}

		public virtual IBaseNavigationPage<TNavigationViewModel> GetNavigationPage<TNavigationViewModel>(
			TNavigationViewModel setNavigationPageModel = null)
			where TNavigationViewModel : class, IBaseNavigationPageModel
		{
			var navigationElement = _navigationDeck.FirstOrDefault(p => p.NavigationViewModelType == typeof(TNavigationViewModel));
			IBaseNavigationPage<TNavigationViewModel> navigationPage;
            
			var viewModel = Locator.Current.GetService(navigationElement.ViewModelType) as IBaseNavigationPageModel;
            var view = Locator.Current.GetService(navigationElement.ViewType) as IBaseNavigationPage<IBaseNavigationPageModel>;

            if (viewModel == null || view == null)
                throw new Exception("No Navigation Creation func: " + navigationElement.NavigationViewModelType.Name);

            SetPageModel(view, viewModel);                  

			navigationPage = GetView<TNavigationViewModel>(navigationElement.NavigationViewType);         

			if (setNavigationPageModel != null)
			{
				SetPageModel(navigationPage, setNavigationPageModel);
			}
			else
			{
				var navigationPageModel = GetViewModel<TNavigationViewModel>(navigationElement.NavigationViewModelType);            
				SetPageModel(navigationPage, navigationPageModel);
			}         

			return navigationPage;
		}

		public virtual IBaseNavigationPage<TPageModel> GetPageByModel<TPageModel>(TPageModel pageModel)
			where TPageModel : class, IBaseNavigationPageModel
		{
			var page = Locator.Current.GetService<TPageModel>();
			return page as IBaseNavigationPage<TPageModel>;
		}

		public virtual TPageModel GetPageModel<TPageModel>(IBaseNavigationPage<TPageModel> page)
			where TPageModel : class, IBaseNavigationPageModel
		{
			throw new NotImplementedException();
		}

		public virtual void SetPageModel<TPageModel>(IBaseNavigationPage<TPageModel> page, TPageModel newPageModel)
			where TPageModel : class, IBaseNavigationPageModel
		{
			throw new NotImplementedException();
		}
	}
}