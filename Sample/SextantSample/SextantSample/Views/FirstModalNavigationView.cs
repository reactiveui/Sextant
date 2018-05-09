using System;
using Sextant;
using SextantSample.ViewModels;
using Xamarin.Forms;

namespace SextantSample.Views
{
	public class FirstModalNavigationView : NavigationPage, IBaseNavigationPage<FirstModalNavigationViewModel>
	{
		public FirstModalNavigationView()
		{
        }

		public FirstModalNavigationView(Page root) : base(root)
        {
        }
	}
}
