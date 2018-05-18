using System;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sextant.UnitTests
{
    public class MockedSextantNavigationService : SextantNavigationServiceBase, ISextantNavigationService
    {
        public void OnPageAppearing(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnPageDisappearing(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        Task<bool> ISextantNavigationService.InsertPageBeforeAsync<TPageModel, TBeforePageModel>(IBaseNavigationPage<TPageModel> pageToInsert, IBaseNavigationPage<TBeforePageModel> beforePage)
        {
            throw new NotImplementedException();
        }

        Task<bool> ISextantNavigationService.PopModalPageAsync<TCurrentPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, bool animated)
        {
            throw new NotImplementedException();
        }

        Task<bool> ISextantNavigationService.PopPageAsync<TCurrentPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, bool animated)
        {
            throw new NotImplementedException();
        }

        Task<bool> ISextantNavigationService.PopPagesToRootAsync<TCurrentPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, bool animated)
        {
            throw new NotImplementedException();
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

        Task<bool> ISextantNavigationService.PushPageAsync<TCurrentPageModel, TPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, IBaseNavigationPage<TPageModel> pageToPush, bool animated)
        {
            throw new NotImplementedException();
        }

        Task<bool> ISextantNavigationService.RemovePageAsync<TCurrentPageModel, TPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, IBaseNavigationPage<TPageModel> pageToRemove)
        {
            throw new NotImplementedException();
        }

        Task<bool> ISextantNavigationService.SetNewRootAndResetAsync<TPageModel>(IBaseNavigationPage<TPageModel> newRootPage)
        {
            throw new NotImplementedException();
        }

        Task<bool> ISextantNavigationService.SetNewRootAndResetAsync<TPageModelOfNewRoot>()
        {
            throw new NotImplementedException();
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
