// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using Sextant.Mocks;
using Splat;

namespace Sextant.Tests;

/// <summary>Checks registration resolution and failure paths.</summary>
public sealed class RegistrationTests
{
    /// <summary>The contract for factory registrations.</summary>
    private const string FactoryContract = "contract";

    /// <summary>The contract for instance registrations.</summary>
    private const string InstanceContract = "instance";

    /// <summary>Checks that popup navigation resolves the application logger through its public constructor.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PopupNavigationUsesTheApplicationLogger()
    {
        using var view = new NavigationViewMock();
        var navigation = new PopupNavigationMock();
        using var service = new Plugins.Popup.PopupViewStackService(view, navigation, new PlatformViewLocator(), new ViewModelFactoryMock());
        await Assert.That(service.View).IsSameReferenceAs(view);
        await Assert.That(service.PopupStack.Count).IsEqualTo(0);
    }

    /// <summary>Checks default and contracted view model factories.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task FactoryResolvesInstancesAndFactories()
    {
        using var model = new NavigableViewModelMock();
        var resolver = AppLocator.CurrentMutable;
        _ = resolver.RegisterViewModel(() => model);
        _ = resolver.RegisterViewModel(() => model, FactoryContract);
        var factory = new DefaultViewModelFactory();
        await Assert.That(factory.Create(typeof(NavigableViewModelMock))).IsSameReferenceAs(model);
        await Assert.That(factory.Create(typeof(NavigableViewModelMock), FactoryContract)).IsSameReferenceAs(model);
        _ = resolver.RegisterViewModel(model);
        _ = resolver.RegisterViewModel(model, InstanceContract);
        await Assert.That(factory.Create(typeof(NavigableViewModelMock))).IsSameReferenceAs(model);
        await Assert.That(factory.Create(typeof(NavigableViewModelMock), InstanceContract)).IsSameReferenceAs(model);
        await Assert.That(() => factory.Create(typeof(NavigableViewModelMock), "missing")).Throws<InvalidOperationException>();
        await Assert.That(() => factory.Create(null!)).Throws<ArgumentNullException>();
        var inner = new InvalidOperationException("inner");
        await Assert.That(new ViewModelFactoryNotFoundException("outer", inner).InnerException).IsSameReferenceAs(inner);
        await Assert.That(new ViewModelFactoryNotFoundException().Message).IsNotNull();
    }

    /// <summary>Checks default and contracted view model factories.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task GenericFactoryResolvesInstancesAndFactories()
    {
        using var model = new NavigableViewModelMock();
        var resolver = AppLocator.CurrentMutable;
        _ = resolver.RegisterViewModel(() => model);
        _ = resolver.RegisterViewModel(() => model, FactoryContract);
        var factory = new DefaultViewModelFactory();
        await Assert.That(factory.Create<NavigableViewModelMock>()).IsSameReferenceAs(model);
        await Assert.That(factory.Create<NavigableViewModelMock>(FactoryContract)).IsSameReferenceAs(model);
        _ = resolver.RegisterViewModel(model);
        _ = resolver.RegisterViewModel(model, InstanceContract);
        await Assert.That(factory.Create<NavigableViewModelMock>()).IsSameReferenceAs(model);
        await Assert.That(factory.Create<NavigableViewModelMock>(InstanceContract)).IsSameReferenceAs(model);
        await Assert.That(() => factory.Create<NavigableViewModelMock>("missing")).Throws<InvalidOperationException>();
        await Assert.That(() => factory.Create(null!)).Throws<ArgumentNullException>();
        var inner = new InvalidOperationException("inner");
        await Assert.That(new ViewModelFactoryNotFoundException("outer", inner).InnerException).IsSameReferenceAs(inner);
        await Assert.That(new ViewModelFactoryNotFoundException().Message).IsNotNull();
    }

    /// <summary>Checks missing views and both explicit and default factory registrations.</summary>
    /// <param name="registerFactory">Whether a factory is explicitly registered.</param>
    /// <returns>The asynchronous test.</returns>
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task StackRegistrationsResolveViewsAndFactories(bool registerFactory)
    {
        var resolver = AppLocator.CurrentMutable;
        resolver.UnregisterAll<IViewModelFactory>();
        if (registerFactory)
        {
            _ = resolver.RegisterViewModelFactory();
        }

        _ = resolver.RegisterViewStackService();
        _ = resolver.RegisterParameterViewStackService();
        _ = resolver.RegisterViewStackService(static (view, factory) => new ViewStackService(view, factory));
        await Assert.That(static () => AppLocator.Current.GetService<IViewStackService>()).Throws<InvalidOperationException>();
        await Assert.That(static () => AppLocator.Current.GetService<IParameterViewStackService>()).Throws<InvalidOperationException>();
        await Assert.That(static () => AppLocator.Current.GetService<ViewStackService>()).Throws<InvalidOperationException>();
        using var view = new NavigationViewMock();
        resolver.RegisterConstant<IView>(view, DependencyResolverMixins.NavigationView);
        _ = resolver.RegisterViewStackService();
        _ = resolver.RegisterParameterViewStackService();
        _ = resolver.RegisterViewStackService(static (registeredView, factory) => new ViewStackService(registeredView, factory));
        using var stack = (IDisposable)AppLocator.Current.GetService<IViewStackService>()!;
        using var parameterStack = (IDisposable)AppLocator.Current.GetService<IParameterViewStackService>()!;
        using var customStack = AppLocator.Current.GetService<ViewStackService>()!;
        using var defaultParameterStack = new ParameterViewStackService(view);
        await Assert.That(((IViewStackService)stack).View).IsSameReferenceAs(view);
        await Assert.That(((IViewStackService)parameterStack).View).IsSameReferenceAs(view);
        await Assert.That(customStack.View).IsSameReferenceAs(view);
        await Assert.That(defaultParameterStack.View).IsSameReferenceAs(view);
    }

    /// <summary>Checks resolver callbacks and explicit initialization.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task InitializationRegistersNavigationServices()
    {
        var sextant = global::Sextant.Sextant.Instance;
        NavigationRegistration.Initialize(null);
        sextant.Initialize();
        await Assert.That(sextant.MutableLocator).IsSameReferenceAs(AppLocator.CurrentMutable);
        var resolver = AppLocator.CurrentMutable;
        NavigationRegistration.Initialize(resolver);
        using var view = new NavigationViewMock();
        resolver.RegisterConstant<IView>(view, DependencyResolverMixins.NavigationView);
        using var service = (IDisposable)AppLocator.Current.GetService<IViewStackService>()!;
        await Assert.That(((IViewStackService)service).View).IsSameReferenceAs(view);
        await Assert.That(static () => ((Sextant)null!).Initialize()).Throws<ArgumentNullException>();
    }

    /// <summary>Checks safe subscription completion and error reporting without attaching a debugger.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task SafeSubscriptionsReportErrorsAndComplete()
    {
        using var completed = Observable.Return(1).SubscribeSafe();
        var breaks = 0;
        using var failed = SubscribeSafeExtensions.SubscribeSafeCore(
            Observable.Throw<int>(new InvalidOperationException("failure")),
            nameof(SafeSubscriptionsReportErrorsAndComplete),
            null,
            0,
            () => breaks++);
        await Assert.That(breaks).IsEqualTo(1);
    }
}
