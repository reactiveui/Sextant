## Sextant

[![NuGet Stats](https://img.shields.io/nuget/v/sextant.svg)](https://www.nuget.org/packages/sextant) ![Build](https://github.com/reactiveui/Sextant/workflows/Build/badge.svg) [![Code Coverage](https://codecov.io/gh/reactiveui/sextant/branch/main/graph/badge.svg)](https://codecov.io/gh/reactiveui/sextant)
[![](https://img.shields.io/nuget/dt/sextant.svg) ](https://www.nuget.org/packages/sextant) [![](https://img.shields.io/badge/chat-slack-blue.svg)](https://reactiveui.net/slack)

<p align="left"><img src="https://github.com/reactiveui/styleguide/blob/master/logo_sextant/vertical.png?raw=true" alt="Sextant" height="180px"></p>

## A ReactiveUI view model based navigation library

Sextant provides a simple, reactive, view model first navigation service for ReactiveUI applications. It originated from ideas in Kent Boogaart's “Custom routing in ReactiveUI” and has evolved to support modern platforms and the latest ReactiveUI/Splat patterns.

Sextant focuses on:
- ViewModel-first navigation with reactive APIs (IObservable<Unit>)
- Lifecycle hooks for parameterized navigation (INavigable)
- Uniform abstractions across platforms with pluggable IView/IViewLocator

## Packages

Install the packages into your app and platform projects, depending on target UI stack.

- Core (required): Sextant
- .NET MAUI: Sextant.Maui (NavigationView + DI helpers)
- Avalonia: Sextant.Avalonia (NavigationView + DI helpers)
- Optional: Sextant.Plugins.Popup (MAUI, Mopups based modal plugin)

Minimum platform versions follow ReactiveUI platform minimums: https://reactiveui.net/docs/getting-started/minimum-versions#platform-minimums

## Bootstrapping with AppLocator (and AppBuilder)

ReactiveUI/Splat provide AppLocator (and AppBuilder) to configure dependency resolution for your app. Sextant integrates with AppLocator and auto-initializes when the resolver is ready.

At app startup, register your navigation view, view models, and view locator, then push the initial page.

### .NET MAUI quick start

- Ensure you’ve added ReactiveUI.Maui and Sextant.Maui.
- Register views and services using AppLocator.
- Set MainPage to the registered NavigationView.

Example (in App constructor or after DI setup):

```csharp
using ReactiveUI;
using ReactiveUI.Maui;
using Sextant;
using Sextant.Maui;
using Splat;

// Register all navigation components
AppLocator.CurrentMutable
    // IViewLocator should be registered in your app; many apps use ReactiveUI.ViewLocator.Current
    .RegisterConstant(ReactiveUI.ViewLocator.Current, typeof(IViewLocator))
    .RegisterNavigationView() // Sextant.Maui NavigationView
    .RegisterViewModelFactory(() => new DefaultViewModelFactory())
    .RegisterParameterViewStackService()
    // Views and VMs for navigation
    .RegisterViewForNavigation<HomeView, HomeViewModel>(() => new HomeView(), () => new HomeViewModel())
    .RegisterViewForNavigation<DetailsView, DetailsViewModel>(() => new DetailsView(), () => new DetailsViewModel());

// Push initial page
AppLocator.Current
    .GetService<IParameterViewStackService>()
    .PushPage<HomeViewModel>(resetStack: true, animate: false)
    .Subscribe();

// Hook MAUI MainPage to Sextant NavigationView
MainPage = AppLocator.Current.GetNavigationView();
```

Notes
- If you don’t pass parameters, IViewStackService is sufficient. If you pass parameters or use INavigable lifecycle, prefer IParameterViewStackService.
- Sextant.Maui exposes GetNavigationView() to fetch the registered NavigationView.

### Avalonia quick start

Register navigation and show a window containing the NavigationView.

```csharp
using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using Sextant;
using Sextant.Avalonia;
using Splat;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .UseReactiveUI();
}

public class App : Application
{
    public override void OnFrameworkInitializationCompleted()
    {
        // DI registrations
        AppLocator.CurrentMutable
            .RegisterConstant(ReactiveUI.ViewLocator.Current, typeof(IViewLocator))
            .RegisterNavigationView(() => new Sextant.Avalonia.NavigationView())
            .RegisterViewModelFactory(() => new DefaultViewModelFactory())
            .RegisterViewForNavigation<HomeView, HomeViewModel>(() => new HomeView(), () => new HomeViewModel());

        var viewStack = AppLocator.Current.GetService<IViewStackService>()!;
        viewStack.PushPage<HomeViewModel>(resetStack: true).Subscribe();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new Window { Content = AppLocator.Current.GetNavigationView() };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
```

## Using the navigation services

Sextant provides two main services:
- IViewStackService – view model first navigation
- IParameterViewStackService – navigation with parameters and lifecycle

Common methods (both services)
- IObservable<Unit> PushPage<TViewModel>(string? contract = null, bool resetStack = false, bool animate = true)
- IObservable<Unit> PushPage(IViewModel viewModel, string? contract = null, bool resetStack = false, bool animate = true)
- IObservable<Unit> PushModal<TViewModel>(string? contract = null, bool withNavigationPage = true)
- IObservable<Unit> PushModal(IViewModel modal, string? contract = null, bool withNavigationPage = true)
- IObservable<Unit> PopPage(bool animate = true)
- IObservable<Unit> PopModal(bool animate = true)
- IObservable<Unit> PopToRootPage(bool animate = true)
- IObservable<IImmutableList<IViewModel>> PageStack / ModalStack
- IObservable<IViewModel> TopPage() / TopModal()

Parameter navigation (IParameterViewStackService)
- IObservable<Unit> PushPage<TViewModel>(INavigationParameter parameter, string? contract = null, bool resetStack = false, bool animate = true) where TViewModel : INavigable
- IObservable<Unit> PushPage(INavigable vm, INavigationParameter parameter, string? contract = null, bool resetStack = false, bool animate = true)
- IObservable<Unit> PushModal<TViewModel>(INavigationParameter parameter, string? contract = null, bool withNavigationPage = true) where TViewModel : INavigable
- IObservable<Unit> PushModal(INavigable modal, INavigationParameter parameter, string? contract = null, bool withNavigationPage = true)
- IObservable<Unit> PopPage(INavigationParameter parameter, bool animate = true)

## Parameters and lifecycle (INavigable)

Implement INavigable on your view models to receive lifecycle notifications and parameters.

```csharp
using System;
using System.Reactive;
using Sextant;

public sealed class DetailsViewModel : INavigable
{
    public string Id => nameof(DetailsViewModel);

    public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter)
    {
        // Called before the page is pushed
        return Observable.Return(Unit.Default);
    }

    public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter)
    {
        // Read parameters
        parameter.TryGetValue("itemId", out string id);
        return Observable.Return(Unit.Default);
    }

    public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter)
        => Observable.Return(Unit.Default);
}
```

Passing parameters

```csharp
var param = new NavigationParameter { ["itemId"] = someId };
_appLocator.GetService<IParameterViewStackService>()
    .PushPage<DetailsViewModel>(param)
    .Subscribe();
```

## View model usage example

```csharp
using System.Reactive;
using ReactiveUI;
using Sextant;

public class HomeViewModel : ViewModelBase
{
    public HomeViewModel(IViewStackService nav)
    {
        OpenDetails = ReactiveCommand.CreateFromObservable(
            () => nav.PushPage<DetailsViewModel>(),
            outputScheduler: RxApp.MainThreadScheduler);

        OpenModal = ReactiveCommand.CreateFromObservable(
            () => nav.PushModal<AboutViewModel>(),
            outputScheduler: RxApp.MainThreadScheduler);

        Back = ReactiveCommand.CreateFromObservable(
            () => nav.PopPage(),
            outputScheduler: RxApp.MainThreadScheduler);
    }

    public ReactiveCommand<Unit, Unit> OpenDetails { get; }
    public ReactiveCommand<Unit, Unit> OpenModal { get; }
    public ReactiveCommand<Unit, Unit> Back { get; }

    public override string Id => nameof(HomeViewModel);
}
```

## Contracts and modal options

- contract lets you register/resolve alternate views for the same ViewModel.
- withNavigationPage (PushModal) wraps the modal in a navigation page on platforms that support it (e.g., MAUI).
- resetStack replaces the navigation stack with the pushed page.

## Best practices

- Register IViewLocator. If you already use ReactiveUI’s default, register ReactiveUI.ViewLocator.Current as IViewLocator.
- Use AppLocator.CurrentMutable to register views, services, and factories during boot.
- Prefer IParameterViewStackService when passing data or using lifecycle hooks (INavigable).
- Dispose resources in view models implementing IDestructible; Sextant calls Destroy() when a VM is popped.
- Observe on RxApp.MainThreadScheduler when updating UI; Sextant uses IView.MainThreadScheduler internally.

## Samples

This repository contains MAUI and Avalonia samples demonstrating full setups (registration, navigation, parameters, and modal flows). Explore the Samples folder in the repo.

## Contribute

Sextant is developed under an OSI-approved open source license, making it freely usable and distributable, even for commercial use. We appreciate contributions of all kinds: docs, samples, bug reports, and features.

- Answer questions on StackOverflow: https://stackoverflow.com/questions/tagged/sextant
- Share knowledge and help new contributors
- Submit documentation updates where needed
- Make code contributions
