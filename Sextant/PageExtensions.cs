using System;

namespace Sextant
{
	/// <summary>
	/// Page extensions.
	/// </summary>
	public static class PageExtensions
	{
		/// <summary>
		/// Gets the page page model.
		/// </summary>
		/// <returns>The page model.</returns>
		/// <param name="page">Page.</param>
		/// <typeparam name="TPageModel">The 1st type parameter.</typeparam>
		public static TPageModel GetPageModel<TPageModel>(this IBaseNavigationPage<TPageModel> page) where TPageModel : class, IBaseNavigationPageModel
		{
			return SextantCore.Instance.GetPageModel(page);
		}

		/// <summary>
		/// Sets the page page model.
		/// </summary>
		/// <returns>The page model.</returns>
		/// <param name="page">Page.</param>
		/// <param name="newPageModel">New page model.</param>
		/// <typeparam name="TPageModel">The 1st type parameter.</typeparam>
		public static IBaseNavigationPage<TPageModel> SetPageModel<TPageModel>(this IBaseNavigationPage<TPageModel> page, TPageModel newPageModel) where TPageModel : class, IBaseNavigationPageModel
		{
			SextantCore.Instance.SetPageModel(page, newPageModel);

			return page;
		}

		/// <summary>
		/// Executes the action on the page model.
		/// </summary>
		/// <returns>The on page model.</returns>
		/// <param name="page">Page.</param>
		/// <param name="action">Action.</param>
		/// <typeparam name="TPageModel">The 1st type parameter.</typeparam>
		public static IBaseNavigationPage<TPageModel> ExecuteOnPageModel<TPageModel>(this IBaseNavigationPage<TPageModel> page, Action<TPageModel> action) where TPageModel : class, IBaseNavigationPageModel
		{
			var model = page.GetPageModel();
			action?.Invoke(model);

			return page;
		}

		/// <summary>
		/// Gets the page as new instance.
		/// Optionally provide a page model (else will be set automatically)
		/// </summary>
		/// <returns>The page as new instance.</returns>
		/// <param name="setPageModel">Page model.</param>
		/// <param name="currentPage">Current page.</param>
		/// <typeparam name="TPageModel">The 1st type parameter.</typeparam>
		public static IBaseNavigationPage<TPageModel> GetPageAsNewInstance<TPageModel>(this IBaseNavigationPage<IBaseNavigationPageModel> currentPage, TPageModel setPageModel = null) where TPageModel : class, IBaseNavigationPageModel
		{
			return SextantCore.Instance.GetPage(setPageModel);
		}

		/// <summary>
		/// Gets the page as new instance.
		/// </summary>
		/// <returns>The page as new instance.</returns>
		/// <param name="currentPage">Current page.</param>
		/// <param name="pageModelType">Page model type.</param>
		//public static IBaseNavigationPage<IBaseNavigationPageModel> GetPageAsNewInstance(this IBaseNavigationPage<IBaseNavigationPageModel> currentPage, Type pageModelType)
		//{
		//	return SextantCore.Instance.GetPage(pageModelType);
		//}
	}
}
