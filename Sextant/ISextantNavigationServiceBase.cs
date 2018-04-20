using System;

namespace Sextant
{
    public interface ISextantNavigationServiceBase
    {
        IBaseLogger Logger { get; set; }

        IBaseNavigationPage<TPageModel> GetPage<TPageModel>(TPageModel setPageModel = null) where TPageModel : class, IBaseNavigationPageModel;
        IBaseNavigationPage<IBaseNavigationPageModel> GetPage(Type pageModelType);
        IBaseNavigationPage<TPageModel> GetPageByModel<TPageModel>(TPageModel pageModel) where TPageModel : class, IBaseNavigationPageModel;
        TPageModel GetPageModel<TPageModel>(IBaseNavigationPage<TPageModel> page) where TPageModel : class, IBaseNavigationPageModel;
        void RegisterNavigationPage<TNavPageModel, TInitalPageModel>()
            where TNavPageModel : class, IBaseNavigationPageModel
            where TInitalPageModel : class, IBaseNavigationPageModel;
        void RegisterNavigationPage<TNavPageModel>(Func<IBaseNavigationPage<IBaseNavigationPageModel>> initialPage = null, Func<TNavPageModel> createNavModel = null, Func<IBaseNavigationPage<IBaseNavigationPageModel>, IBaseNavigationPage<TNavPageModel>> createNav = null) where TNavPageModel : class, IBaseNavigationPageModel;
        void RegisterPage<TPageModel>(Func<TPageModel> createPageModel = null, Func<IBaseNavigationPage<TPageModel>> createPage = null) where TPageModel : class, IBaseNavigationPageModel;
        void SetPageModel<TPageModel>(IBaseNavigationPage<TPageModel> page, TPageModel newPageModel) where TPageModel : class, IBaseNavigationPageModel;
    }
}