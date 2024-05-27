using Microsoft.Extensions.Logging;

namespace SextantSample.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            RxApp.DefaultExceptionHandler = new SextantDefaultExceptionHandler();
            var resolver = Locator.CurrentMutable;
            resolver.InitializeSplat();
            resolver.InitializeReactiveUI();
            ////Instance.InitializeMaui();
            Locator
                .CurrentMutable
                .RegisterViewForNavigation<HomeView, HomeViewModel>()
                .RegisterViewForNavigation<FirstModalView, FirstModalViewModel>(() => new(), () => new(Locator.Current.GetService<IViewStackService>()))
                .RegisterViewForNavigation<SecondModalView, SecondModalViewModel>(() => new(), () => new(Locator.Current.GetService<IViewStackService>()))
                .RegisterViewForNavigation<RedView, RedViewModel>(() => new(), () => new(Locator.Current.GetService<IViewStackService>()))
                .RegisterViewForNavigation<GreenView, GreenViewModel>(() => new(), () => new(Locator.Current.GetService<IViewStackService>()))
                .RegisterNavigationView(() => new BlueNavigationView());

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
