// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Concurrency;
using Sextant.Mocks;
using Splat;
using AvaloniaNavigation = Sextant.Avalonia.NavigationView;
using AvaloniaRegistration = Sextant.Avalonia.DependencyResolverMixins;
using MauiNavigation = Sextant.Maui.NavigationView;
using MauiRegistration = Sextant.Maui.DependencyResolverMixins;

namespace Sextant.Tests.Navigation;

/// <summary>Checks the platform navigation adapters using managed controls.</summary>
public sealed class PlatformRegistrationTests
{
    /// <summary>Checks default navigation constructors and platform registrations.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PlatformRegistrationsReturnTheirNavigationViews()
    {
        using var navigation = new MauiNavigation();
        var resolver = AppLocator.CurrentMutable;
        _ = MauiRegistration.RegisterNavigationView(resolver, () => navigation);
        await Assert.That(MauiRegistration.GetNavigationView(AppLocator.Current)).IsSameReferenceAs(navigation);
        await Assert.That(MauiRegistration.GetNavigationView(AppLocator.Current, "missing")).IsNull();
        using var service = (IDisposable)AppLocator.Current.GetService<IViewStackService>()!;
        await Assert.That(((IViewStackService)service).View).IsSameReferenceAs(navigation);
        var avalonia = new AvaloniaNavigation();
        _ = AvaloniaRegistration.RegisterNavigationView(resolver, () => avalonia);
        await Assert.That(AvaloniaRegistration.GetNavigationView(AppLocator.Current)).IsSameReferenceAs(avalonia);
        using var avaloniaService = (IDisposable)AppLocator.Current.GetService<IViewStackService>()!;
        await Assert.That(((IViewStackService)avaloniaService).View).IsSameReferenceAs(avalonia);
        await Assert.That(static () => AvaloniaRegistration.GetNavigationView(AppLocator.Current, "missing")).Throws<InvalidOperationException>();
    }

    /// <summary>Checks registered MAUI schedulers and missing view locator failures.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task MauiRegistrationsRequireAViewLocatorAndPreserveSchedulers()
    {
        var resolver = AppLocator.CurrentMutable;
        resolver.UnregisterAll<IViewLocator>();
        _ = MauiRegistration.RegisterNavigationView(resolver);
        await Assert.That(static () => MauiRegistration.GetNavigationView(AppLocator.Current)).Throws<InvalidOperationException>();
        var locator = new PlatformViewLocator();
        resolver.RegisterConstant<IViewLocator>(locator);
        _ = MauiRegistration.RegisterNavigationView(resolver);
        using var navigation = MauiRegistration.GetNavigationView(AppLocator.Current)!;
        await Assert.That(navigation.MainThreadScheduler).IsSameReferenceAs(RxSchedulers.MainThreadScheduler);
        resolver.UnregisterAll<IViewLocator>();
        _ = MauiRegistration.RegisterNavigationView(resolver, ImmediateScheduler.Instance);
        await Assert.That(static () => MauiRegistration.GetNavigationView(AppLocator.Current)).Throws<InvalidOperationException>();
        resolver.RegisterConstant<IViewLocator>(locator);
        _ = MauiRegistration.RegisterNavigationView(resolver, ImmediateScheduler.Instance);
        using var scheduled = MauiRegistration.GetNavigationView(AppLocator.Current)!;
        await Assert.That(scheduled.MainThreadScheduler).IsSameReferenceAs(ImmediateScheduler.Instance);
        global::Sextant.Maui.SextantExtensions.InitializeMaui(global::Sextant.Sextant.Instance);
        using var initialized = MauiRegistration.GetNavigationView(AppLocator.Current)!;
        using var stack = (IDisposable)AppLocator.Current.GetService<IViewStackService>()!;
        using var parameterStack = (IDisposable)AppLocator.Current.GetService<IParameterViewStackService>()!;
        await Assert.That(((IViewStackService)stack).View).IsSameReferenceAs(initialized);
        await Assert.That(((IViewStackService)parameterStack).View).IsSameReferenceAs(initialized);
        await Assert.That(ViewModelFactory.Current).IsTypeOf<DefaultViewModelFactory>();
        await Assert.That(static () => global::Sextant.Maui.SextantExtensions.InitializeMaui(null!)).Throws<ArgumentNullException>();
    }
}
