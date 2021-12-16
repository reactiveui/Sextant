using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Microsoft.Maui.Controls.Xaml;
using Sextant;
using Sextant.Maui;
using SextantSample.ViewModels;
using Splat;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace SextantSample.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Locator
                .Current
                .GetService<IViewStackService>()
                .PushPage(new HomeViewModel(), null, true, false)
                .Subscribe();

            MainPage = Locator.Current.GetNavigationView();
        }
    }
}
