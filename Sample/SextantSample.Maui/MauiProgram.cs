using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using ReactiveUI;
using Sextant;
using Sextant.Maui;
using SextantSample.Maui.Views;
using SextantSample.ViewModels;
using Splat;
using static Sextant.Sextant;

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
                });

            RxApp.DefaultExceptionHandler = new SextantDefaultExceptionHandler();
            var resolver = Locator.CurrentMutable;
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

            return builder.Build();
        }
    }
}
