// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Sextant.Mocks;

namespace Sextant.Tests.Navigation;

/// <summary>Checks default view and factory overloads through their interfaces.</summary>
public sealed class DefaultViewTests
{
    /// <summary>Checks forwarding defaults against real navigation operations.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task ViewDefaultsPushAndPopPagesAndModals()
    {
        using var inner = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var view = new DefaultView(inner);
        IViewModel? popped = null;
        using var subscription = view.PagePopped.Subscribe(value => popped = value);
        await ((IView)view).PushPage(model, null, false);
        await ((IView)view).PushPage(model, null, false);
        await ((IView)view).PopToRootPage();
        await Assert.That(popped).IsSameReferenceAs(model);
        popped = null;
        await ((IView)view).PopPage();
        await Assert.That(popped).IsSameReferenceAs(model);
        await ((IView)view).PushModal(model, null);
        await view.PopModal();
        await Assert.That(view.MainThreadScheduler).IsSameReferenceAs(inner.MainThreadScheduler);
    }

    /// <summary>Checks factory interface default contract selection.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task FactoryDefaultUsesTheUncontractedRegistration()
    {
        using var model = new NavigableViewModelMock();
        var factory = new DefaultFactory(model);
        await Assert.That(((IViewModelFactory)factory).Create(typeof(NavigableViewModelMock))).IsSameReferenceAs(model);
    }

    /// <summary>Uses default overloads and delegates complete view operations.</summary>
    /// <param name="view">The in-memory navigation view.</param>
    private sealed class DefaultView(NavigationViewMock view) : IView
    {
        /// <inheritdoc />
        public IScheduler MainThreadScheduler => view.MainThreadScheduler;

        /// <inheritdoc />
        public IObservable<IViewModel?> PagePopped => view.PagePopped;

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PopModal() => view.PopModal();

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PopPage(bool animate) => view.PopPage(animate);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PopToRootPage(bool animate) => view.PopToRootPage(animate);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract, bool withNavigationPage) => view.PushModal(modalViewModel, contract, withNavigationPage);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PushPage(IViewModel viewModel, string? contract, bool resetStack, bool animate) => view.PushPage(viewModel, contract, resetStack, animate);
    }

    /// <summary>Uses the interface default to select the uncontracted view model.</summary>
    /// <param name="model">The view model owned by this test.</param>
    private sealed class DefaultFactory(IViewModel model) : IViewModelFactory
    {
        /// <summary>The registered view model factory.</summary>
        private readonly ViewModelFactoryMock _factory = new(model);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IViewModel Create(Type viewModelType, string? contract) => _factory.Create(viewModelType, contract);
    }
}
