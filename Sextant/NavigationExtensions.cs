using System;
using System.Threading.Tasks;

namespace Sextant
{
	/// <summary>
	/// Navigation extensions.
	/// </summary>
	public static class NavigationExtensions
	{
		/// <summary>
		/// Pushes the page into current navigation stack.
		/// </summary>
		/// <returns>The page async.</returns>
		/// <param name="currentPageModel">Current page model.</param>
		/// <param name="pageToPush">Page to push.</param>
		/// <param name="executeOnPageModel">Execute on page model.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		/// <typeparam name="TCurrentPageModel">The 1st type parameter.</typeparam>
		/// <typeparam name="TPageModel">The 2nd type parameter.</typeparam>        
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

		/// <summary>
		/// Pushs the page as new instance into current navigation stack.
		/// </summary>
		/// <returns>The page as new instance async.</returns>
		/// <param name="currentPageModel">Current page model.</param>
		/// <param name="executeOnPageModel">Execute on page model.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		/// <typeparam name="TPageModel">The 1st type parameter.</typeparam>
		public static Task<bool> PushPageAsNewInstanceAsync<TPageModel>(this IBaseNavigationPageModel currentPageModel, Action<TPageModel> executeOnPageModel = null, bool animated = true) where TPageModel : class, IBaseNavigationPageModel
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

		/// <summary>
		/// Pushs the page as new instance into current navigation stack.
		/// </summary>
		/// <returns>The page as new instance async.</returns>
		/// <param name="currentPageModel">Current page model.</param>
		/// <param name="executeOnPageModel">Execute on page model.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		/// <typeparam name="TPageModel">The 1st type parameter.</typeparam>
		public static Task<bool> PushModalPageAsNewInstanceAsync<TPageModel>(this IBaseNavigationPageModel currentPageModel, Action<TPageModel> executeOnPageModel = null, bool animated = true) where TPageModel : class, IBaseNavigationPageModel
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

		/// <summary>
		/// Pushes the modal page into current navigation stack.
		/// </summary>
		/// <returns>The modal page async.</returns>
		/// <param name="currentPageModel">Current page model.</param>
		/// <param name="pageToPush">Page to push.</param>
		/// <param name="executeOnPageModel">Execute on page model.</param> 
		/// <param name="animated">If set to <c>true</c> animated.</param>
		/// <typeparam name="TCurrentPageModel">The 1st type parameter.</typeparam>
		/// <typeparam name="TPageModel">The 2nd type parameter.</typeparam>
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

		/// <summary>
		/// Pops the page from current navigation stack.
		/// </summary>
		/// <returns>The page async.</returns>
		/// <param name="pageModel">Page model.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		/// <typeparam name="TPageModel">The 1st type parameter.</typeparam>
		public static Task<bool> PopPageAsync<TPageModel>(this TPageModel pageModel, bool animated = true) where TPageModel : class, IBaseNavigationPageModel
		{
			var page = SextantCore.Instance.GetPageByModel(pageModel);

			if (page != null)
				return SextantCore.Instance.PopPageAsync(page, animated);

			return Task.FromResult(false);
		}

		/// <summary>
		/// Pops the modal page from current navigation stack.
		/// </summary>
		/// <returns>The modal page async.</returns>
		/// <param name="pageModel">Page model.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		/// <typeparam name="TPageModel">The 1st type parameter.</typeparam>
		public static Task<bool> PopModalPageAsync<TPageModel>(this TPageModel pageModel, bool animated = true) where TPageModel : class, IBaseNavigationPageModel
		{
			var page = SextantCore.Instance.GetPageByModel(pageModel);

			if (page != null)
				return SextantCore.Instance.PopModalPageAsync(page, animated);

			return Task.FromResult(false);
		}

		/// <summary>
		/// Removes the page from current navigation stack.
		/// </summary>
		/// <returns>The page.</returns>
		/// <param name="currentPageModel">Current page model.</param>
		/// <param name="pageToRemove">Page to remove.</param>
		/// <typeparam name="TCurrentPageModel">The 1st type parameter.</typeparam>
		/// <typeparam name="TPageModel">The 2nd type parameter.</typeparam>
		public static Task<bool> RemovePageAsync<TCurrentPageModel, TPageModel>(this TCurrentPageModel currentPageModel, IBaseNavigationPage<TPageModel> pageToRemove) where TCurrentPageModel : class, IBaseNavigationPageModel where TPageModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currentPageModel);

			if (currentPage != null)
				return SextantCore.Instance.RemovePageAsync(currentPage, pageToRemove);

			return Task.FromResult(false);
		}

		/// <summary>
		/// Pops all pages to root in current navigation stack.
		/// </summary>
		/// <returns>The pages to root async.</returns>
		/// <param name="currentPageModel">Current page model.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		/// <typeparam name="TCurrentPageModel">The 1st type parameter.</typeparam>
		public static Task<bool> PopPagesToRootAsync<TCurrentPageModel>(this TCurrentPageModel currentPageModel, bool animated = true) where TCurrentPageModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currentPageModel);

			if (currentPage != null)
				return SextantCore.Instance.PopPagesToRootAsync(currentPage, animated);

			return Task.FromResult(false);
		}


		/// <summary>
		/// Sets the new root and resets based on PageModel
		/// </summary>
		/// <param name="currentPageModel">Current page model.</param>
		public static Task<bool> SetNewRootAndResetAsync<TNewRootPageModel>(this IBaseNavigationPageModel currentPageModel) where TNewRootPageModel : class, IBaseNavigationPageModel
		{
			return SextantCore.Instance.SetNewRootAndResetAsync<TNewRootPageModel>();
		}

		public static Task<bool> PushPageAsNewInstanceAsync<TPageNavigationModel, TViewModel>(this IBaseNavigationPageModel currenTViewModel, Action<TViewModel> executeOnPageModel = null, bool animated = true) where TViewModel : class, IBaseNavigationPageModel where TPageNavigationModel : class, IBaseNavigationPageModel
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

		public static Task<bool> PushModalPageAsNewInstanceAsync<TPageNavigationModel, TViewModel>(this IBaseNavigationPageModel currenTViewModel, Action<TViewModel> executeOnPageModel = null, bool animated = true) where TViewModel : class, IBaseNavigationPageModel where TPageNavigationModel : class, IBaseNavigationPageModel
		{
			var currentPage = SextantCore.Instance.GetPageByModel(currenTViewModel);

			if (currentPage != null)
			{
				var pageToPush = SextantCore.Instance.GetPage<TViewModel>();
				var navigationPageToPush = SextantCore.Instance.GetPage<TPageNavigationModel>();

				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.Instance.PushModalPageAsync(currentPage, navigationPageToPush, animated);
			}

			return Task.FromResult(false);
		}
	}
}