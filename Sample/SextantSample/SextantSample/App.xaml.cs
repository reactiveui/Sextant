using System;
using Sextant;
using SextantSample.ViewModels;
using SextantSample.Views;
using Splat;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SextantSample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


			var navigationService = new XamarinFormsSextantNavigationService(this, false)
            {
                Logger = new BaseLogger()
            };

			SextantCore.SetCurrentFactory(navigationService);

			navigationService.RegisterPage<HomeView, HomeViewModel, HomeNavigationView, HomeNavigationViewModel>();
			navigationService.RegisterPage<FirstModalView, FirstModalViewModel, FirstModalNavigationView, FirstModalNavigationViewModel>();
			navigationService.RegisterPage<SecondModalView, SecondModalViewModel, SecondModalNavigationView, SecondModalNavigationViewModel>();

			MainPage = SextantCore.Instance.GetNavigationPage<HomeNavigationViewModel>() as NavigationPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
