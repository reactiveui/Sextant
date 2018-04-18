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
		public static TPageModel GetPageModel<TPageModel>(this IBasePage<TPageModel> page) where TPageModel : class, IBasePageModel
		{
			return SextantCore.CurrentFactory.GetPageModel(page);
		}

		/// <summary>
		/// Sets the page page model.
		/// </summary>
		/// <returns>The page model.</returns>
		/// <param name="page">Page.</param>
		/// <param name="newPageModel">New page model.</param>
		/// <typeparam name="TPageModel">The 1st type parameter.</typeparam>
		public static IBasePage<TPageModel> SetPageModel<TPageModel>(this IBasePage<TPageModel> page, TPageModel newPageModel) where TPageModel : class, IBasePageModel
		{
			SextantCore.CurrentFactory.SetPageModel(page, newPageModel);

			return page;
		}

		/// <summary>
		/// Executes the action on the page model.
		/// </summary>
		/// <returns>The on page model.</returns>
		/// <param name="page">Page.</param>
		/// <param name="action">Action.</param>
		/// <typeparam name="TPageModel">The 1st type parameter.</typeparam>
		public static IBasePage<TPageModel> ExecuteOnPageModel<TPageModel>(this IBasePage<TPageModel> page, Action<TPageModel> action) where TPageModel : class, IBasePageModel
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
		public static IBasePage<TPageModel> GetPageAsNewInstance<TPageModel>(this IBasePage<IBasePageModel> currentPage, TPageModel setPageModel = null) where TPageModel : class, IBasePageModel
		{
			return SextantCore.CurrentFactory.GetPage(setPageModel);
		}

		/// <summary>
		/// Gets the page as new instance.
		/// </summary>
		/// <returns>The page as new instance.</returns>
		/// <param name="currentPage">Current page.</param>
		/// <param name="pageModelType">Page model type.</param>
		public static IBasePage<IBasePageModel> GetPageAsNewInstance(this IBasePage<IBasePageModel> currentPage, Type pageModelType)
		{
			return SextantCore.CurrentFactory.GetPage(pageModelType);
		}
	}
}
