using System;

namespace Sextant
{
    public interface ISextantNavigationServiceBase
    {
		IBaseLogger Logger { get; set; }

        IBaseNavigationPage<TPageModel> GetPage<TPageModel>(TPageModel setPageModel = null) where TPageModel : class, IBaseNavigationPageModel;
		IBaseNavigationPage<TNavigationViewModel> GetNavigationPage<TNavigationViewModel>(Action<IBaseViewModel> executeOnPageModel = null, TNavigationViewModel setPageModel = null) where TNavigationViewModel : class, IBaseNavigationPageModel;
        IBaseNavigationPage<TPageModel> GetPageByModel<TPageModel>(TPageModel pageModel) where TPageModel : class, IBaseNavigationPageModel;
        TPageModel GetPageModel<TPageModel>(IBaseNavigationPage<TPageModel> page) where TPageModel : class, IBaseNavigationPageModel;      
		void RegisterPage<TPage, TPageModel, TNavigationPage, TNavigationPageModel>()
			where TPage : class, IBaseNavigationPage<TPageModel>, new()
			where TPageModel : class, IBaseNavigationPageModel, new()
			where TNavigationPage : class, IBaseNavigationPage<TNavigationPageModel>, new()
			where TNavigationPageModel : class, IBaseNavigationPageModel, new();
        void RegisterPage<TPage, TPageModel, TNavigationPage, TNavigationPageModel>(Func<IBaseNavigationPage<TPageModel>> viewCreationFunc)
            where TPage : class, IBaseNavigationPage<TPageModel>
            where TPageModel : class, IBaseNavigationPageModel, new()
            where TNavigationPage : class, IBaseNavigationPage<TNavigationPageModel>, new()
            where TNavigationPageModel : class, IBaseNavigationPageModel, new();
        void RegisterPage<TPage, TPageModel>(Func<TPageModel> createPageModel = null)
			where TPage : class, IBaseNavigationPage<TPageModel>, new()
			where TPageModel : class, IBaseNavigationPageModel, new();
        void SetPageModel<TPageModel>(IBaseNavigationPage<TPageModel> page, TPageModel newPageModel) where TPageModel : class, IBaseNavigationPageModel;
    }
}