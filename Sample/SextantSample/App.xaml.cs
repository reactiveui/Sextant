using System;
using ReactiveUI;
using Sextant;
using Sextant.XamForms;
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

            RxApp.DefaultExceptionHandler = new SextantDefaultExceptionHandler();

            Sextant.Sextant.Instance.InitializeForms();

            Locator
                .CurrentMutable
                .RegisterView<HomeView, HomeViewModel>()
                .RegisterView<FirstModalView, FirstModalViewModel>()
                .RegisterView<SecondModalView, SecondModalViewModel>()
                .RegisterView<RedView, RedViewModel>()
                .RegisterNavigation(() => new BlueNavigationView());


            Locator
                .Current
                .GetService<IViewStackService>()
                .PushPage(new HomeViewModel(), null, true, false)
                .Subscribe();

            MainPage = Locator.Current.GetService<IView>(nameof(NavigationView)) as NavigationView;
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
