using static Sextant.Sextant;

namespace SextantSample.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        RxApp.DefaultExceptionHandler = new SextantDefaultExceptionHandler();
        IMutableDependencyResolver resolver = Locator.CurrentMutable;
        resolver.InitializeSplat();
        resolver.InitializeReactiveUI();
        Instance.InitializeMaui();
        Locator
            .CurrentMutable
            .RegisterView<HomeView, HomeViewModel>()
            .RegisterView<FirstModalView, FirstModalViewModel>()
            .RegisterView<SecondModalView, SecondModalViewModel>()
            .RegisterView<RedView, RedViewModel>()
            .RegisterView<GreenView, GreenViewModel>()
            .RegisterNavigationView(() => new BlueNavigationView())
            .RegisterViewModel(() => new GreenViewModel(Locator.Current.GetService<IViewStackService>()));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
