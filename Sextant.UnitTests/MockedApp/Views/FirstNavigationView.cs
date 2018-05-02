using System;
using Sextant.UnitTests.MockedApp.ViewModels;
using Xamarin.Forms;

namespace Sextant.UnitTests.MockedApp.Views
{
    public class FirstNavigationView : NavigationPage, IBaseNavigationPage<FirstNavigationViewModel>
    {
        public FirstNavigationView(Page page) : base(page)
        {
        }
    }
}
