using System;
using System.Threading.Tasks;

namespace Sextant
{
    public interface IBaseFactory
    {
        IBaseLogger Logger { get; set; }

        IBasePage<TPageModel> GetPage<TPageModel>(TPageModel setPageModel = null) where TPageModel : class, IBasePageModel;
        IBasePage<IBasePageModel> GetPage(Type pageModelType);
        IBasePage<TPageModel> GetPageByModel<TPageModel>(TPageModel pageModel) where TPageModel : class, IBasePageModel;
        TPageModel GetPageModel<TPageModel>(IBasePage<TPageModel> page) where TPageModel : class, IBasePageModel;
        Task<bool> InsertPageBeforeAsync<TPageModel, TBeforePageModel>(IBasePage<TPageModel> pageToInsert, IBasePage<TBeforePageModel> beforePage)
            where TPageModel : class, IBasePageModel
            where TBeforePageModel : class, IBasePageModel;
        Task<bool> PopModalPageAsync<TCurrentPageModel>(IBasePage<TCurrentPageModel> currentPage, bool animated = true) where TCurrentPageModel : class, IBasePageModel;
        Task<bool> PopPageAsync<TCurrentPageModel>(IBasePage<TCurrentPageModel> currentPage, bool animated = true) where TCurrentPageModel : class, IBasePageModel;
        Task<bool> PopPagesToRootAsync<TCurrentPageModel>(IBasePage<TCurrentPageModel> currentPage, bool animated = true) where TCurrentPageModel : class, IBasePageModel;
        Task<bool> PushModalPageAsync<TCurrentPageModel, TPageModel>(IBasePage<TCurrentPageModel> currentPage, IBasePage<TPageModel> pageToPush, bool animated = true)
            where TCurrentPageModel : class, IBasePageModel
            where TPageModel : class, IBasePageModel;
        Task<bool> PushPageAsync<TCurrentPageModel, TPageModel>(IBasePage<TCurrentPageModel> currentPage, IBasePage<TPageModel> pageToPush, bool animated = true)
            where TCurrentPageModel : class, IBasePageModel
            where TPageModel : class, IBasePageModel;
        void RegisterNavigationPage<TNavPageModel, TInitalPageModel>()
            where TNavPageModel : class, IBasePageModel
            where TInitalPageModel : class, IBasePageModel;
        void RegisterNavigationPage<TNavPageModel>(Func<IBasePage<IBasePageModel>> initialPage = null, Func<TNavPageModel> createNavModel = null, Func<IBasePage<IBasePageModel>, IBasePage<TNavPageModel>> createNav = null) where TNavPageModel : class, IBasePageModel;
        void RegisterPage<TPageModel>(Func<TPageModel> createPageModel = null, Func<IBasePage<TPageModel>> createPage = null) where TPageModel : class, IBasePageModel;
        Task<bool> RemovePageAsync<TCurrentPageModel, TPageModel>(IBasePage<TCurrentPageModel> currentPage, IBasePage<TPageModel> pageToRemove)
            where TCurrentPageModel : class, IBasePageModel
            where TPageModel : class, IBasePageModel;
        Task<bool> SetNewRootAndResetAsync<TPageModel>(IBasePage<TPageModel> newRootPage) where TPageModel : class, IBasePageModel;
        Task<bool> SetNewRootAndResetAsync<TPageModelOfNewRoot>() where TPageModelOfNewRoot : class, IBasePageModel;
        void SetPageModel<TPageModel>(IBasePage<TPageModel> page, TPageModel newPageModel) where TPageModel : class, IBasePageModel;
    }
}