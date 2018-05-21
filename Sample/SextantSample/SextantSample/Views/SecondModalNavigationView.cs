using System;
using Sextant;
using SextantSample.ViewModels;
using Xamarin.Forms;

namespace SextantSample.Views
{
	public class SecondModalNavigationView : NavigationPage, IBaseNavigationPage<SecondModalNavigationViewModel>
	{
		public SecondModalNavigationView()
		{
        }

		public SecondModalNavigationView(Page root) : base(root)
        {
        }
	}
}
