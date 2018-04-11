using System;
using Xamarin.Forms;

namespace Sextant
{
	public class BaseNavigationPage<TPageModel> : NavigationPage, IBasePage<TPageModel> where TPageModel : class, IBasePageModel
	{
		public BaseNavigationPage()
		{
		}

		public BaseNavigationPage(Page page) : base(page)
		{
		}
	}
}