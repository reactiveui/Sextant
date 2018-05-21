using System;
using Sextant;
using SextantSample.ViewModels;
using Xamarin.Forms;

namespace SextantSample.Views
{
	public class HomeNavigationView : NavigationPage, IBaseNavigationPage<HomeNavigationViewModel>
	{
		public HomeNavigationView()
		{
        }

		public HomeNavigationView(Page root) : base(root)
        {
        }
	}
}
