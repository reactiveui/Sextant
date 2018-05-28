using System;
using ReactiveUI;
using Sextant;
using Sextant.Abstraction;
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

            Locator.CurrentMutable.Register(CreateView<HomeView>, typeof(IViewFor<HomeViewModel>));

			Locator.CurrentMutable.Register(CreateView<FirstModalView>, typeof(IViewFor<FirstModalViewModel>));         
			//Locator.CurrentMutable.Register(CreateView<FirstModalNavigationView>, typeof(IViewFor<FirstModalNavigationViewModel>));         

			Locator.CurrentMutable.Register(CreateView<SecondModalView>, typeof(IViewFor<SecondModalViewModel>));
			Locator.CurrentMutable.Register(CreateView<RedView>, typeof(IViewFor<RedViewModel>));

                     
			var navigationView = new NavigationView(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, Locator.Current.GetService<IViewLocator>());         
			var viewStackService = new ViewStackService(navigationView);

			Locator.CurrentMutable.Register<IViewStackService>(() => viewStackService);
            
			MainPage = navigationView;         
			navigationView.PushPage(new HomeViewModel(viewStackService), null, true, false).Subscribe();
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

		protected T CreateView<T>()
                where T : new()
        {
            return new T();
        }
    }
}
