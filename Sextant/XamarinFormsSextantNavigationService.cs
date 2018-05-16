using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sextant
{
    public class XamarinFormsSextantNavigationService : SextantNavigationServiceBase, ISextantNavigationService
    {
        public XamarinFormsSextantNavigationService(Application appInstance, bool automaticAssembliesDiscovery = true, params Assembly[] additionalPagesAssemblies)
            : base()
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


        public void OnPageAppearing(object sender, EventArgs e)
        {
            var model = ((sender as Page).BindingContext as IPageVisibilityChange);
            if (model != null)
            {
                model.OnAppearing();
            }
        }

        public void OnPageDisappearing(object sender, EventArgs e)
        {
            var model = ((sender as Page).BindingContext as IPageVisibilityChange);
            if (model != null)
            {
                model.OnDisappearing();
            }
        }

        public async Task<bool> PushPageAsync<TCurrentPageModel, TPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, IBaseNavigationPage<TPageModel> pageToPush, bool animated = true) where TCurrentPageModel : class, IBaseNavigationPageModel where TPageModel : class, IBaseNavigationPageModel
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

        public async Task<bool> PushModalPageAsync<TCurrentPageModel, TPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, IBaseNavigationPage<TPageModel> pageToPush, bool animated = true) where TCurrentPageModel : class, IBaseNavigationPageModel where TPageModel : class, IBaseNavigationPageModel
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

        public Task<bool> InsertPageBeforeAsync<TPageModel, TBeforePageModel>(IBaseNavigationPage<TPageModel> pageToInsert, IBaseNavigationPage<TBeforePageModel> beforePage) where TPageModel : class, IBaseNavigationPageModel where TBeforePageModel : class, IBaseNavigationPageModel
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

        public async Task<bool> PopPageAsync<TCurrentPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, bool animated = true) where TCurrentPageModel : class, IBaseNavigationPageModel
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

        public async Task<bool> PopModalPageAsync<TCurrentPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, bool animated = true) where TCurrentPageModel : class, IBaseNavigationPageModel
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

        public Task<bool> RemovePageAsync<TCurrentPageModel, TPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, IBaseNavigationPage<TPageModel> pageToRemove) where TCurrentPageModel : class, IBaseNavigationPageModel where TPageModel : class, IBaseNavigationPageModel
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

        public async Task<bool> PopPagesToRootAsync<TCurrentPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, bool animated = true) where TCurrentPageModel : class, IBaseNavigationPageModel
        {
            var navigation = ((Page)currentPage)?.Navigation;
            if (navigation == null)
                return false;

            await navigation.PopToRootAsync(animated);

            return true;
        }

        public Task<bool> SetNewRootAndResetAsync<TPageModel>(IBaseNavigationPage<TPageModel> newRootPage) where TPageModel : class, IBaseNavigationPageModel
        {
            Application.Current.MainPage = (Page)newRootPage;

            return Task.FromResult(true);
        }

        public Task<bool> SetNewRootAndResetAsync<TPageModelOfNewRoot>() where TPageModelOfNewRoot : class, IBaseNavigationPageModel
        {
            //TODO: Find a better way to distinguish between navigation page VMs and page VMs instead of the below try/catch block

            Page page = null;
            try
            {
                page = (Page)GetPage<TPageModelOfNewRoot>();
            }
            catch (NullReferenceException)
            {
                page = (Page)GetNavigationPage<TPageModelOfNewRoot>();
            }
            catch (Exception)
            {
                throw;
            }

            Application.Current.MainPage = page;

            return Task.FromResult(true);
        }


        public override TPageModel GetPageModel<TPageModel>(IBaseNavigationPage<TPageModel> page)
        {
            var xfPage = page as Page;
            return xfPage?.BindingContext as TPageModel;
        }

        public override void SetPageModel<TPageModel>(IBaseNavigationPage<TPageModel> page, TPageModel newPageModel)
        {
            var formsPage = (Page)page;

            var oldVisChange = formsPage.BindingContext as IPageVisibilityChange;
            if (oldVisChange != null)
            {
                formsPage.Appearing -= OnPageAppearing;
                formsPage.Disappearing -= OnPageDisappearing;
            }

            formsPage.BindingContext = newPageModel;

            var newVisChange = newPageModel as IPageVisibilityChange;
            if (newVisChange != null)
            {
                formsPage.Appearing += OnPageAppearing;
                formsPage.Disappearing += OnPageDisappearing;
            }
        }
    }
}
