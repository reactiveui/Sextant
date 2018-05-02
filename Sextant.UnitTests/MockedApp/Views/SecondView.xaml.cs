using System;
using System.Collections.Generic;
using Sextant.UnitTests.MockedApp.ViewModels;
using Xamarin.Forms;

namespace Sextant.UnitTests.MockedApp.Views
{
    public partial class SecondView : ContentPage, IBaseNavigationPage<SecondViewModel>
    {
        public SecondView()
        {
            InitializeComponent();
        }
    }
}
