// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Internals;
using Sextant.Maui;
using Sextant.Mocks;

namespace Sextant.Tests.Navigation;

/// <summary>Checks lifecycle callbacks separately from device event wiring.</summary>
public sealed class NavigationLifecycleTests
{
    /// <summary>The stack size retained by an interrupted root pop.</summary>
    private const int InterruptedStackCount = 2;

    /// <summary>Checks lifecycle callbacks with and without a remaining page.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task DevicePopNotifiesBothModelsAndDestroysTheRemovedModel()
    {
        var previous = new LifecycleModel();
        var current = new LifecycleModel();
        using var subscriptions = new CompositeDisposable();
        var page = new ContentPage { BindingContext = previous };
        NavigationLifecycle.OnPagePopped(page, new ContentPage { BindingContext = current }, subscriptions);
        await Assert.That(previous.Departures).IsEqualTo(1);
        await Assert.That(previous.Destructions).IsEqualTo(1);
        await Assert.That(current.Arrivals).IsEqualTo(1);
        NavigationLifecycle.OnPagePopped(new ContentPage(), new ContentPage(), subscriptions);
        NavigationLifecycle.OnPagePopped(new ContentPage(), null, subscriptions);
        await Assert.That(previous.Destructions).IsEqualTo(1);
    }

    /// <summary>Checks that device events notify models while service events are ignored.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task DeviceEventsUseLifecycleCallbacksAndDisposeWithTheView()
    {
        var previous = new LifecycleModel();
        var current = new LifecycleModel();
        var locator = new PlatformViewLocator();
        var root = new ContentPage { BindingContext = current };
        using var navigation = new NavigationView(ImmediateScheduler.Instance, locator, root);
        var page = new ContentPage { BindingContext = previous };
        await navigation.Navigation.PushAsync(page, false);
        _ = await navigation.Navigation.PopAsync(false);
        using var popped = new Subject<NavigationEventArgs>();
        using var source = new BehaviorSubject<NavigationSource>(NavigationSource.NavigationService);
        using var subscriptions = new CompositeDisposable();
        NavigationEvents.SubscribeDevicePops(navigation, popped, source, subscriptions);
        popped.OnNext(new(page));
        await Assert.That(previous.Destructions).IsEqualTo(0);
        source.OnNext(NavigationSource.Device);
        popped.OnNext(new(page));
        await Assert.That(previous.Departures).IsEqualTo(1);
        await Assert.That(previous.Destructions).IsEqualTo(1);
        await Assert.That(current.Arrivals).IsEqualTo(1);
    }

    /// <summary>Checks event adaptation, view-model filtering and handler removal.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PopEventsPublishOnlyViewModelsAndRemoveTheirHandlers()
    {
        EventHandler<NavigationEventArgs>? handler = null;
        List<IViewModel> models = [];
        using (var subscription = NavigationEvents.ObserveViewModelPops(value => handler = value, _ => handler = null).Subscribe(models.Add))
        {
            handler!(null, new(new ContentPage()));
            var model = new TestModel();
            handler(null, new(new ContentPage { BindingContext = model }));
            await Assert.That(models.Count).IsEqualTo(1);
            await Assert.That(models[0]).IsSameReferenceAs(model);
        }

        await Assert.That(handler).IsNull();
    }

    /// <summary>Checks binding context propagation and detach disposal.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task BehaviorsFollowTheirPageUntilDetached()
    {
        var page = new ContentPage();
        using var behavior = new ContextBehavior();
        page.Behaviors.Add(behavior);
        var first = new object();
        page.BindingContext = first;
        await Assert.That(behavior.BindingContext).IsSameReferenceAs(first);
        behavior.Release(false);
        var second = new object();
        page.BindingContext = second;
        await Assert.That(behavior.BindingContext).IsSameReferenceAs(second);
        _ = page.Behaviors.Remove(behavior);
        page.BindingContext = first;
        await Assert.That(behavior.BindingContext).IsSameReferenceAs(second);
    }

    /// <summary>Exposes managed-resource disposal for a behavior.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task ResetDoesNotReplaceAnUnexpectedRemainingStack()
    {
        var navigation = new PreservingNavigation();
        await NavigationOperations.PushPage(navigation, new ContentPage(), true, false);
        await Assert.That(navigation.NavigationStack.Count).IsEqualTo(InterruptedStackCount);
        await Assert.That(navigation.PopRequested).IsTrue();
    }

    /// <summary>Preserves its stack to model an interrupted root pop.</summary>
    private sealed class PreservingNavigation : NavigationProxy
    {
        /// <summary>The pages still present after the pop request.</summary>
        private readonly List<Page> _pages = [new ContentPage(), new ContentPage()];

        /// <summary>Gets a value indicating whether a root pop was requested.</summary>
        internal bool PopRequested { get; private set; }

        /// <inheritdoc />
        protected override IReadOnlyList<Page> GetNavigationStack() => _pages;

        /// <inheritdoc />
        protected override Task OnPopToRootAsync(bool animated)
        {
            PopRequested = true;
            return Task.CompletedTask;
        }
    }

    /// <summary>Exposes managed-resource disposal for a behavior.</summary>
    private sealed class ContextBehavior : BehaviorBase<BindableObject>
    {
        /// <summary>Runs the protected disposal path.</summary>
        /// <param name="disposing">Whether to release managed resources.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Release(bool disposing) => Dispose(disposing);
    }

    /// <summary>A view model used to verify pop-event filtering.</summary>
    private sealed class TestModel : IViewModel
    {
        /// <inheritdoc />
        public string Id => nameof(TestModel);
    }

    /// <summary>Records navigation lifecycle callbacks.</summary>
    private sealed class LifecycleModel : INavigated, IDestructible
    {
        /// <summary>Gets the number of arrivals.</summary>
        public int Arrivals { get; private set; }

        /// <summary>Gets the number of departures.</summary>
        public int Departures { get; private set; }

        /// <summary>Gets the number of destruction calls.</summary>
        public int Destructions { get; private set; }

        /// <inheritdoc />
        public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter)
        {
            Arrivals++;
            return Observable.Return(Unit.Default);
        }

        /// <inheritdoc />
        public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter)
        {
            Departures++;
            return Observable.Return(Unit.Default);
        }

        /// <inheritdoc />
        public void Destroy() => Destructions++;
    }
}
