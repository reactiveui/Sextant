using System;
using Sextant.UnitTests.MockedApp.ViewModels;
using Xamarin.Forms;

namespace Sextant.UnitTests.MockedApp.Views
{
    public class SecondNavigationView : NavigationPage, IBaseNavigationPage<SecondNavigationViewModel>
    {
        public SecondNavigationView()
        {
        }

        public SecondNavigationView(Page page) : base(page)
        {
        }
    }
}
