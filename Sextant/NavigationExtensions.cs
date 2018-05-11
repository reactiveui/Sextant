using System;
using System.Threading.Tasks;

namespace Sextant
{
	public static class NavigationExtensions
	{
		public static Task<bool> PushPageAsync<TCurrentPageModel, TPageModel>(this TCurrentPageModel currentPageModel, IBaseNavigationPage<TPageModel> pageToPush, Action<TPageModel> executeOnPageModel = null, bool animated = true) where TCurrentPageModel : class, IBaseNavigationPageModel where TPageModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currentPageModel);

			if (currentPage != null)
			{
				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.Instance.PushPageAsync(currentPage, pageToPush, animated);
			}

			return Task.FromResult(false);
		}

		public static Task<bool> PushPageAsync<TPageModel>(this IBaseNavigationPageModel currentPageModel, Action<TPageModel> executeOnPageModel = null, bool animated = true) where TPageModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currentPageModel);

			if (currentPage != null)
			{
				var pageToPush = SextantCore.Instance.GetPage<TPageModel>();

				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.Instance.PushPageAsync(currentPage, pageToPush, animated);
			}

			return Task.FromResult(false);
		}

		public static Task<bool> PushModalPageAsync<TPageModel>(this IBaseNavigationPageModel currentPageModel, Action<TPageModel> executeOnPageModel = null, bool animated = true) where TPageModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currentPageModel);

			if (currentPage != null)
			{
				var pageToPush = SextantCore.Instance.GetPage<TPageModel>();

				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.Instance.PushModalPageAsync(currentPage, pageToPush, animated);
			}

			return Task.FromResult(false);
		}

		public static Task<bool> PushModalPageAsync<TCurrentPageModel, TPageModel>(this TCurrentPageModel currentPageModel, IBaseNavigationPage<TPageModel> pageToPush, Action<TPageModel> executeOnPageModel = null, bool animated = true) where TCurrentPageModel : class, IBaseNavigationPageModel where TPageModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currentPageModel);

			if (currentPage != null)
			{
				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.Instance.PushModalPageAsync(currentPage, pageToPush, animated);
			}

			return Task.FromResult(false);
		}

		public static Task<bool> PopPageAsync<TPageModel>(this TPageModel pageModel, bool animated = true) where TPageModel : class, IBaseNavigationPageModel
		{
			var page = SextantCore.Instance.GetPageByModel(pageModel);

			if (page != null)
				return SextantCore.Instance.PopPageAsync(page, animated);

			return Task.FromResult(false);
		}


		public static Task<bool> PopModalPageAsync<TPageModel>(this TPageModel pageModel, bool animated = true) where TPageModel : class, IBaseNavigationPageModel
		{
			var page = SextantCore.Instance.GetPageByModel(pageModel);

			if (page != null)
				return SextantCore.Instance.PopModalPageAsync(page, animated);

			return Task.FromResult(false);
		}

		public static Task<bool> RemovePageAsync<TCurrentPageModel, TPageModel>(this TCurrentPageModel currentPageModel, IBaseNavigationPage<TPageModel> pageToRemove) where TCurrentPageModel : class, IBaseNavigationPageModel where TPageModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currentPageModel);

			if (currentPage != null)
				return SextantCore.Instance.RemovePageAsync(currentPage, pageToRemove);

			return Task.FromResult(false);
		}

		public static Task<bool> PopPagesToRootAsync<TCurrentPageModel>(this TCurrentPageModel currentPageModel, bool animated = true) where TCurrentPageModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currentPageModel);

			if (currentPage != null)
				return SextantCore.Instance.PopPagesToRootAsync(currentPage, animated);

			return Task.FromResult(false);
		}


		public static Task<bool> SetNewRootAndResetAsync<TNewRootPageModel>(this IBaseNavigationPageModel currentPageModel) where TNewRootPageModel : class, IBaseNavigationPageModel
		{
			return SextantCore.Instance.SetNewRootAndResetAsync<TNewRootPageModel>();
		}

		public static Task<bool> PushPageAsync<TPageNavigationModel, TViewModel>(this IBaseNavigationPageModel currenTViewModel, Action<TViewModel> executeOnPageModel = null, bool animated = true) where TViewModel : class, IBaseNavigationPageModel where TPageNavigationModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currenTViewModel);

			if (currentPage != null)
			{
				var pageToPush = SextantCore.Instance.GetPage<TViewModel>();
				var navigationPageToPush = SextantCore.Instance.GetPage<TPageNavigationModel>();

				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.Instance.PushPageAsync(currentPage, navigationPageToPush, animated);
			}

			return Task.FromResult(false);
		}

		public static Task<bool> PushModalPageAsync<TPageNavigationModel, TViewModel>(this IBaseNavigationPageModel currenTViewModel, Action<TViewModel> executeOnPageModel = null, bool animated = true) where TViewModel : class, IBaseNavigationPageModel where TPageNavigationModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currenTViewModel);

			if (currentPage != null)
			{
				var navigationPageToPush = SextantCore.Instance.GetNavigationPage<TPageNavigationModel>();
				return SextantCore.Instance.PushModalPageAsync(currentPage, navigationPageToPush, animated);
			}

			return Task.FromResult(false);
		}
	}
}