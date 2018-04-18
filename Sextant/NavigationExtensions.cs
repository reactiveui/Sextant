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
		public static Task<bool> PushPageAsync<TCurrentPageModel, TPageModel>(this TCurrentPageModel currentPageModel, IBasePage<TPageModel> pageToPush, Action<TPageModel> executeOnPageModel = null, bool animated = true) where TCurrentPageModel : class, IBasePageModel where TPageModel : class, IBasePageModel
		{
			var currentPage = SextantCore.CurrentFactory.GetPageByModel(currentPageModel);

			if (currentPage != null)
			{
				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.CurrentFactory.PushPageAsync(currentPage, pageToPush, animated);
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
		public static Task<bool> PushPageAsNewInstanceAsync<TPageModel>(this IBasePageModel currentPageModel, Action<TPageModel> executeOnPageModel = null, bool animated = true) where TPageModel : class, IBasePageModel
		{
			var currentPage = SextantCore.CurrentFactory.GetPageByModel(currentPageModel);

			if (currentPage != null)
			{
				var pageToPush = SextantCore.CurrentFactory.GetPage<TPageModel>();

				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.CurrentFactory.PushPageAsync(currentPage, pageToPush, animated);
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
		public static Task<bool> PushModalPageAsNewInstanceAsync<TPageModel>(this IBasePageModel currentPageModel, Action<TPageModel> executeOnPageModel = null, bool animated = true) where TPageModel : class, IBasePageModel
		{
			var currentPage = SextantCore.CurrentFactory.GetPageByModel(currentPageModel);

			if (currentPage != null)
			{
				var pageToPush = SextantCore.CurrentFactory.GetPage<TPageModel>();

				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.CurrentFactory.PushModalPageAsync(currentPage, pageToPush, animated);
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
		public static Task<bool> PushModalPageAsync<TCurrentPageModel, TPageModel>(this TCurrentPageModel currentPageModel, IBasePage<TPageModel> pageToPush, Action<TPageModel> executeOnPageModel = null, bool animated = true) where TCurrentPageModel : class, IBasePageModel where TPageModel : class, IBasePageModel
		{
			var currentPage = SextantCore.CurrentFactory.GetPageByModel(currentPageModel);

			if (currentPage != null)
			{
				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.CurrentFactory.PushModalPageAsync(currentPage, pageToPush, animated);
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
		public static Task<bool> PopPageAsync<TPageModel>(this TPageModel pageModel, bool animated = true) where TPageModel : class, IBasePageModel
		{
			var page = SextantCore.CurrentFactory.GetPageByModel(pageModel);

			if (page != null)
				return SextantCore.CurrentFactory.PopPageAsync(page, animated);

			return Task.FromResult(false);
		}

		/// <summary>
		/// Pops the modal page from current navigation stack.
		/// </summary>
		/// <returns>The modal page async.</returns>
		/// <param name="pageModel">Page model.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		/// <typeparam name="TPageModel">The 1st type parameter.</typeparam>
		public static Task<bool> PopModalPageAsync<TPageModel>(this TPageModel pageModel, bool animated = true) where TPageModel : class, IBasePageModel
		{
			var page = SextantCore.CurrentFactory.GetPageByModel(pageModel);

			if (page != null)
				return SextantCore.CurrentFactory.PopModalPageAsync(page, animated);

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
		public static Task<bool> RemovePageAsync<TCurrentPageModel, TPageModel>(this TCurrentPageModel currentPageModel, IBasePage<TPageModel> pageToRemove) where TCurrentPageModel : class, IBasePageModel where TPageModel : class, IBasePageModel
		{
			var currentPage = SextantCore.CurrentFactory.GetPageByModel(currentPageModel);

			if (currentPage != null)
				return SextantCore.CurrentFactory.RemovePageAsync(currentPage, pageToRemove);

			return Task.FromResult(false);
		}

		/// <summary>
		/// Pops all pages to root in current navigation stack.
		/// </summary>
		/// <returns>The pages to root async.</returns>
		/// <param name="currentPageModel">Current page model.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		/// <typeparam name="TCurrentPageModel">The 1st type parameter.</typeparam>
		public static Task<bool> PopPagesToRootAsync<TCurrentPageModel>(this TCurrentPageModel currentPageModel, bool animated = true) where TCurrentPageModel : class, IBasePageModel
		{
			var currentPage = SextantCore.CurrentFactory.GetPageByModel(currentPageModel);

			if (currentPage != null)
				return SextantCore.CurrentFactory.PopPagesToRootAsync(currentPage, animated);

			return Task.FromResult(false);
		}


		/// <summary>
		/// Sets the new root and resets based on PageModel
		/// </summary>
		/// <param name="currentPageModel">Current page model.</param>
		public static Task<bool> SetNewRootAndResetAsync<TNewRootPageModel>(this IBasePageModel currentPageModel) where TNewRootPageModel : class, IBasePageModel
		{
			return SextantCore.CurrentFactory.SetNewRootAndResetAsync<TNewRootPageModel>();
		}

		public static Task<bool> PushPageAsNewInstanceAsync<TPageNavigationModel, TViewModel>(this IBasePageModel currenTViewModel, Action<TViewModel> executeOnPageModel = null, bool animated = true) where TViewModel : class, IBasePageModel where TPageNavigationModel : class, IBasePageModel
		{
			var currentPage = SextantCore.CurrentFactory.GetPageByModel(currenTViewModel);

			if (currentPage != null)
			{
				var pageToPush = SextantCore.CurrentFactory.GetPage<TViewModel>();
				var navigationPageToPush = SextantCore.CurrentFactory.GetPage<TPageNavigationModel>();

				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.CurrentFactory.PushPageAsync(currentPage, navigationPageToPush, animated);
			}

			return Task.FromResult(false);
		}

		public static Task<bool> PushModalPageAsNewInstanceAsync<TPageNavigationModel, TViewModel>(this IBasePageModel currenTViewModel, Action<TViewModel> executeOnPageModel = null, bool animated = true) where TViewModel : class, IBasePageModel where TPageNavigationModel : class, IBasePageModel
		{
			var currentPage = SextantCore.CurrentFactory.GetPageByModel(currenTViewModel);

			if (currentPage != null)
			{
				var pageToPush = SextantCore.CurrentFactory.GetPage<TViewModel>();
				var navigationPageToPush = SextantCore.CurrentFactory.GetPage<TPageNavigationModel>();

				if (executeOnPageModel != null)
					pageToPush.ExecuteOnPageModel(executeOnPageModel);

				return SextantCore.CurrentFactory.PushModalPageAsync(currentPage, navigationPageToPush, animated);
			}

			return Task.FromResult(false);
		}
	}
}