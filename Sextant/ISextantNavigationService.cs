using System;
using System.Threading.Tasks;

namespace Sextant
{
    public interface ISextantNavigationService : ISextantNavigationServiceBase
    {
        Task<bool> InsertPageBeforeAsync<TPageModel, TBeforePageModel>(IBaseNavigationPage<TPageModel> pageToInsert, IBaseNavigationPage<TBeforePageModel> beforePage)
            where TPageModel : class, IBaseNavigationPageModel
            where TBeforePageModel : class, IBaseNavigationPageModel;
        void OnPageAppearing(object sender, EventArgs e);
        void OnPageDisappearing(object sender, EventArgs e);
        Task<bool> PopModalPageAsync<TCurrentPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, bool animated = true) where TCurrentPageModel : class, IBaseNavigationPageModel;
        Task<bool> PopPageAsync<TCurrentPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, bool animated = true) where TCurrentPageModel : class, IBaseNavigationPageModel;
        Task<bool> PopPagesToRootAsync<TCurrentPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, bool animated = true) where TCurrentPageModel : class, IBaseNavigationPageModel;
        Task<bool> PushModalPageAsync<TCurrentPageModel, TPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, IBaseNavigationPage<TPageModel> pageToPush, bool animated = true)
            where TCurrentPageModel : class, IBaseNavigationPageModel
            where TPageModel : class, IBaseNavigationPageModel;
        Task<bool> PushPageAsync<TCurrentPageModel, TPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, IBaseNavigationPage<TPageModel> pageToPush, bool animated = true)
            where TCurrentPageModel : class, IBaseNavigationPageModel
            where TPageModel : class, IBaseNavigationPageModel;
        Task<bool> RemovePageAsync<TCurrentPageModel, TPageModel>(IBaseNavigationPage<TCurrentPageModel> currentPage, IBaseNavigationPage<TPageModel> pageToRemove)
            where TCurrentPageModel : class, IBaseNavigationPageModel
            where TPageModel : class, IBaseNavigationPageModel;
        Task<bool> SetNewRootAndResetAsync<TPageModel>(IBaseNavigationPage<TPageModel> newRootPage) where TPageModel : class, IBaseNavigationPageModel;
        Task<bool> SetNewRootAndResetAsync<TPageModelOfNewRoot>() where TPageModelOfNewRoot : class, IBaseNavigationPageModel;
    }
}