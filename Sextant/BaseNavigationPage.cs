using System;
using Xamarin.Forms;

namespace Sextant
{
	public class BaseNavigationPage<TPageModel> : NavigationPage, IBaseNavigationPage<TPageModel> where TPageModel : class, IBaseNavigationPageModel
	{
		public BaseNavigationPage()
		{
		}

		public BaseNavigationPage(Page page) : base(page)
		{
		}
	}
}