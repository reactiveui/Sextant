<div align="center">

<a href="https://github.com/reactiveui/Sextant"><img src="https://raw.githubusercontent.com/reactiveui/styleguide/master/logo_sextant/logo.png" alt="Sextant" width="128"></a>

# Sextant

[![Build](https://img.shields.io/github/actions/workflow/status/reactiveui/Sextant/ci-build.yml?branch=main&label=build&logo=githubactions&logoColor=white)](https://github.com/reactiveui/Sextant/actions/workflows/ci-build.yml)
[![NuGet](https://img.shields.io/nuget/v/Sextant?label=nuget&logo=nuget&logoColor=white&color=004880)](https://www.nuget.org/packages/Sextant)
[![Downloads](https://img.shields.io/nuget/dt/Sextant?label=downloads&logo=nuget&logoColor=white&color=004880)](https://www.nuget.org/packages/Sextant)
[![Coverage](https://img.shields.io/codecov/c/github/reactiveui/Sextant?label=coverage&logo=codecov&logoColor=white)](https://codecov.io/gh/reactiveui/Sextant)
[![License](https://img.shields.io/github/license/reactiveui/Sextant?color=blue)](https://github.com/reactiveui/Sextant/blob/main/LICENSE)
[![Discussions](https://img.shields.io/badge/discussions-ask%20a%20question-8A2BE2?logo=github&logoColor=white)](https://github.com/reactiveui/Sextant/discussions)

</div>

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
            outputScheduler: RxSchedulers.MainThreadScheduler);

        OpenModal = ReactiveCommand.CreateFromObservable(
            () => nav.PushModal<AboutViewModel>(),
            outputScheduler: RxSchedulers.MainThreadScheduler);

        Back = ReactiveCommand.CreateFromObservable(
            () => nav.PopPage(),
            outputScheduler: RxSchedulers.MainThreadScheduler);
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

This repository contains MAUI and Avalonia samples demonstrating full setups (registration, navigation, parameters, and modal flows). Explore the src/examples folder in the repo.

## Contribute

Sextant is part of [ReactiveUI](https://github.com/reactiveui) and is released under the [MIT license](https://github.com/reactiveui/Sextant/blob/main/LICENSE). We welcome contributions of every size, from bug reports and documentation to features and tests.

- **Questions:** ask in [GitHub Discussions](https://github.com/reactiveui/Sextant/discussions).
- **Bugs and feature requests:** open a [GitHub issue](https://github.com/reactiveui/Sextant/issues/new/choose).
- **Larger changes:** open an issue or discussion first, so we can agree on the approach before you start.
- **Contributor guide:** read [reactiveui.net/contribute](https://www.reactiveui.net/contribute/) for [building and testing](https://www.reactiveui.net/contribute/building-and-testing/) and [pull requests](https://www.reactiveui.net/contribute/pull-requests/).

### Coding standards

Every pull request must meet these standards. The build checks most of them.

- Follow the [ReactiveUI C# style guide](https://www.reactiveui.net/contribute/software-style-guide/code-style/).
- The repository's `.editorconfig` and the StyleSharp, PerformanceSharp and SecuritySharp analyzers run on every build. Fix each finding in the code. Do not suppress it or relax the `.editorconfig`.
- Add tests for every bug fix and new feature.
- Document public APIs with XML documentation comments.
- Write commit messages in the [Conventional Commits](https://www.reactiveui.net/contribute/software-style-guide/commit-message-convention/) format.
- Follow the [Code of Conduct](https://github.com/reactiveui/.github/blob/main/CODE_OF_CONDUCT.md).

### Support the project

- Sponsor the maintainers on GitHub Sponsors: [reactivemarbles](https://github.com/sponsors/reactivemarbles), [glennawatson](https://github.com/sponsors/glennawatson) and [chrispulman](https://github.com/sponsors/chrispulman).
- Ask your employer to support open source, for example with the [balanced employee IP agreement](https://github.com/github/balanced-employee-ip-agreement).

## Sponsors

[JetBrains](https://www.jetbrains.com/) gives ReactiveUI's maintainers licences for its tools through its
[open source support programme](https://www.jetbrains.com/community/opensource/).
[Anthropic](https://www.anthropic.com/) supports them with [Claude](https://claude.com/) through
[Claude for Open Source](https://claude.com/contact-sales/claude-for-oss).
[OpenAI](https://openai.com/) supports them with [Codex](https://openai.com/codex/) through
[Codex for Open Source](https://developers.openai.com/community/codex-for-oss).
[Snyk](https://snyk.io/) gives the maintainers access to security tools through its
[Secure Developer Program](https://snyk.io/open-source/).

[![JetBrains](https://raw.githubusercontent.com/reactiveui/website/main/docs/images/sponsors/jetbrains.svg)](https://www.jetbrains.com/)
[![Claude by Anthropic](https://raw.githubusercontent.com/reactiveui/website/main/docs/images/sponsors/claude.svg)](https://claude.com/)
[![OpenAI](https://raw.githubusercontent.com/reactiveui/website/main/docs/images/sponsors/openai.svg)](https://openai.com/codex/)
[![Sponsored by Snyk](https://raw.githubusercontent.com/reactiveui/website/e37a112f97ea2e9ea54b21bb7ed9e56df034d618/docs/images/sponsors/snyk.svg)](https://snyk.io/)

See [our sponsors](https://www.reactiveui.net/sponsors/) for more information.
JetBrains, Claude, Anthropic, OpenAI and Codex names and logos are trademarks of their respective owners.
Snyk and the Snyk logo are trademarks of Snyk Ltd.
