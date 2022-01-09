using System.Reflection;
using Microsoft.UI.Xaml;
using ReactiveUI;
using Sextant;
using Sextant.WinUI;
using Sextant.WinUI.Mixins;
using SextantSample.ViewModels;
using SextantSample.WinUI.Views;
using Splat;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SextantSample.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();

            RxApp.DefaultExceptionHandler = new SextantDefaultExceptionHandler();
            Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
            Locator
                .CurrentMutable
                .RegisterWindowManager()
                .RegisterWinUIViewLocator()
                .RegisterParameterViewStackService()
                .RegisterNavigationView(() => new BlueNavigationView())
                .RegisterViewWinUI<HomeView, HomeViewModel>()
                .RegisterViewWinUI<FirstModalView, FirstModalViewModel>()
                .RegisterViewWinUI<SecondModalView, SecondModalViewModel>()
                .RegisterViewWinUI<RedView, RedViewModel>()
                .RegisterViewWinUI<GreenView, GreenViewModel>()
                .RegisterViewForNavigation(() => new FirstModalView(), () => new FirstModalViewModel(Locator.Current.GetService<IParameterViewStackService>()))
                .RegisterViewForNavigation(() => new SecondModalView(), () => new SecondModalViewModel(
                    Locator.Current.GetService<IParameterViewStackService>()))
                .RegisterViewModel(() => new GreenViewModel(Locator.Current.GetService<IViewStackService>()));
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();

            Locator.Current.GetService<IWindowManager>()!.CurrentWindow = _window;
            
            _window.Activate();
        }

        private Window _window;
    }
}
