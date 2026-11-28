// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using Mopups.Hosting;
using Mopups.Services;
using Sextant.Plugins.Popup;

namespace SextantSample.Maui;

/// <summary>Configures the MAUI sample and its navigation services.</summary>
public static class MauiProgram
{
    /// <summary>Creates the sample application.</summary>
    /// <returns>The configured MAUI application.</returns>
    /// <exception cref="InvalidOperationException">Thrown when <c>AppLocator.Current.GetService&lt;IViewStackService&gt;()</c> is <see langword="null"/>.</exception>
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        _ = builder.UseMauiApp<App>().ConfigureMopups().ConfigureFonts(static fonts =>
        {
            _ = fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular").AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });
        _ = RxAppBuilder.CreateReactiveUIBuilder()
            .WithMaui()
            .WithExceptionHandler(new SextantDefaultExceptionHandler())
            .WithCoreServices()
            .BuildApp();
        _ = AppLocator.CurrentMutable.RegisterNavigationView(static () => new BlueNavigationView());
        AppLocator.CurrentMutable.RegisterLazySingleton<IPopupViewStackService>(static () => new PopupViewStackService(
            AppLocator.Current.GetNavigationView() ?? throw new InvalidOperationException("The navigation view was not registered."),
            MopupService.Instance,
            ReactiveUI.Binding.Reactive.ViewLocator.GetCurrent(),
            new DefaultViewModelFactory()));
        var navigation = AppLocator.Current.GetService<IViewStackService>()
            ?? throw new InvalidOperationException("The navigation service was not registered.");
        _ = AppLocator.CurrentMutable
            .RegisterViewForNavigation(static () => new HomeView(), static () => new HomeViewModel())
            .RegisterViewForNavigation(static () => new DeliveryNotePopup(), static () => new DeliveryNote("Leave the parcel at reception. Tap outside to close."))
            .RegisterViewForNavigation(static () => new FirstModalView(), () => new FirstModalViewModel(navigation))
            .RegisterViewForNavigation(static () => new SecondModalView(), () => new SecondModalViewModel(navigation))
            .RegisterViewForNavigation(static () => new RedView(), () => new RedViewModel(navigation))
            .RegisterViewForNavigation(static () => new GreenView(), () => new GreenViewModel(navigation));
#if DEBUG
        _ = builder.Logging.AddDebug();
#endif
        return builder.Build();
    }
}
