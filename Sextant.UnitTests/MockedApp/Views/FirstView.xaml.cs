using System;
using System.Collections.Generic;
using Sextant.UnitTests.MockedApp.ViewModels;
using Xamarin.Forms;

namespace Sextant.UnitTests.MockedApp.Views
{
    public partial class FirstView : ContentPage, IBaseNavigationPage<FirstViewModel>
    {
        public FirstView()
        {
            InitializeComponent();
        }
    }
}
