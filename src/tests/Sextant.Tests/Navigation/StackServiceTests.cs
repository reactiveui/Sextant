// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Sextant.Mocks;

namespace Sextant.Tests;

/// <summary>Checks navigation stack updates and lifecycle notifications.</summary>
public sealed class StackServiceTests
{
    /// <summary>The contract for modal registrations.</summary>
    private const string ModalContract = "modal";

    /// <summary>The number of pages after all page overloads.</summary>
    private const int PageOverloadCount = 9;

    /// <summary>The number of modals left after two pops.</summary>
    private const int RemainingModalCount = 4;

    /// <summary>The number of parameterized page pushes.</summary>
    private const int ParameterPageCount = 8;

    /// <summary>The number of parameterized modal pushes.</summary>
    private const int ParameterModalCount = 6;

    /// <summary>Checks typed factory results and contract forwarding through generic navigation.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task GenericNavigationForwardsTheSelectedTypeAndContract()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        var factory = new ViewModelFactoryMock(model);
        IViewModelFactory factoryContract = factory;
        var defaultModel = factoryContract.Create<NavigableViewModelMock>();
        var contractedModel = factoryContract.Create<NavigableViewModelMock>(ModalContract);
        await Assert.That(defaultModel).IsSameReferenceAs(model);
        await Assert.That(contractedModel).IsSameReferenceAs(model);
        await Assert.That(factory.RequestedType).IsEqualTo(typeof(NavigableViewModelMock));
        await Assert.That(factory.RequestedContract).IsEqualTo(ModalContract);
        using var service = new ParameterViewStackService(view, factory, new Splat.WrappingFullLogger(new Splat.NullLogger()));
        await service.PushPage<NavigableViewModelMock>(ModalContract);
        await Assert.That(await service.TopPage()).IsSameReferenceAs(model);
        await Assert.That(factory.RequestedContract).IsEqualTo(ModalContract);
        await service.PushModal<NavigableViewModelMock>(new NavigationParameter(), ModalContract);
        await Assert.That(await service.TopModal()).IsSameReferenceAs(model);
        await Assert.That(factory.RequestedType).IsEqualTo(typeof(NavigableViewModelMock));
        await Assert.That(factory.RequestedContract).IsEqualTo(ModalContract);
    }

    /// <summary>Checks every page overload against the resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task PageOverloadsPreserveModelsAndResetTheStack()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        await service.PushPage((IViewModel)model);
        await service.PushPage((IViewModel)model, "page");
        await service.PushPage((IViewModel)model, "page", false);
        await service.PushPage((IViewModel)model, "page", true, false);
        await Assert.That((await service.PageStack.FirstAsync()).Count).IsEqualTo(1);
        await service.PushPage(model);
        await service.PushPage(model, "page");
        await service.PushPage(model, "page", false);
        await service.PushPage(model, "page", false, false);
        await service.PushPage(typeof(NavigableViewModelMock));
        await service.PushPage(typeof(NavigableViewModelMock), "page");
        await service.PushPage(typeof(NavigableViewModelMock), "page", false);
        await service.PushPage(typeof(NavigableViewModelMock), "page", false, false);
        await Assert.That((await service.PageStack.FirstAsync()).Count).IsEqualTo(PageOverloadCount);
        await Assert.That(await service.TopPage()).IsSameReferenceAs(model);
        await service.PopToRootPage();
        await Assert.That((await service.PageStack.FirstAsync()).Count).IsEqualTo(1);
        await service.PopPage();
        await Assert.That((await service.PageStack.FirstAsync()).Count).IsEqualTo(0);
        await Assert.That(() => service.PushPage((IViewModel)null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Checks every page overload against the resulting stack.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task GenericPageOverloadsPreserveModelsAndResetTheStack()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        await service.PushPage((IViewModel)model);
        await service.PushPage((IViewModel)model, "page");
        await service.PushPage((IViewModel)model, "page", false);
        await service.PushPage((IViewModel)model, "page", true, false);
        await Assert.That((await service.PageStack.FirstAsync()).Count).IsEqualTo(1);
        await service.PushPage(model);
        await service.PushPage(model, "page");
        await service.PushPage(model, "page", false);
        await service.PushPage(model, "page", false, false);
        await service.PushPage<NavigableViewModelMock>();
        await service.PushPage<NavigableViewModelMock>("page");
        await service.PushPage<NavigableViewModelMock>("page", false);
        await service.PushPage<NavigableViewModelMock>("page", false, false);
        await Assert.That((await service.PageStack.FirstAsync()).Count).IsEqualTo(PageOverloadCount);
        await Assert.That(await service.TopPage()).IsSameReferenceAs(model);
        await service.PopToRootPage();
        await Assert.That((await service.PageStack.FirstAsync()).Count).IsEqualTo(1);
        await service.PopPage();
        await Assert.That((await service.PageStack.FirstAsync()).Count).IsEqualTo(0);
        await Assert.That(() => service.PushPage((IViewModel)null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Checks modal overloads and destruction.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task ModalOverloadsPreserveModelsAndDestroyRemovedModels()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        await service.PushModal(model);
        await service.PushModal(model, ModalContract);
        await service.PushModal(typeof(NavigableViewModelMock));
        await service.PushModal(typeof(NavigableViewModelMock), ModalContract);
        await service.PushModal(typeof(NavigableViewModelMock), ModalContract, false);
        var destructible = new LifecycleModel();
        await service.PushModal(destructible, null, false);
        await Assert.That(await service.TopModal()).IsSameReferenceAs(destructible);
        await service.PopModal();
        await Assert.That(destructible.DestroyCount).IsEqualTo(1);
        await service.PopModal(false);
        await Assert.That((await service.ModalStack.FirstAsync()).Count).IsEqualTo(RemainingModalCount);
        await Assert.That(() => service.PushModal((IViewModel)null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Checks modal overloads and destruction.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task GenericModalOverloadsPreserveModelsAndDestroyRemovedModels()
    {
        using var view = new NavigationViewMock();
        using var model = new NavigableViewModelMock();
        using var service = new ViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        await service.PushModal(model);
        await service.PushModal(model, ModalContract);
        await service.PushModal<NavigableViewModelMock>();
        await service.PushModal<NavigableViewModelMock>(ModalContract);
        await service.PushModal<NavigableViewModelMock>(ModalContract, false);
        var destructible = new LifecycleModel();
        await service.PushModal(destructible, null, false);
        await Assert.That(await service.TopModal()).IsSameReferenceAs(destructible);
        await service.PopModal();
        await Assert.That(destructible.DestroyCount).IsEqualTo(1);
        await service.PopModal(false);
        await Assert.That((await service.ModalStack.FirstAsync()).Count).IsEqualTo(RemainingModalCount);
        await Assert.That(() => service.PushModal((IViewModel)null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Checks parameter overloads and both remaining-page lifecycle paths.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task ParameterOverloadsDeliverLifecycleNotifications()
    {
        using var view = new NavigationViewMock();
        var model = new LifecycleModel();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var parameter = new NavigationParameter();
        await service.PushPage(model, parameter);
        await service.PushPage(model, parameter, "page");
        await service.PushPage(model, parameter, "page", false);
        await service.PushPage(model, parameter, "page", false, false);
        await service.PushPage(typeof(LifecycleModel), parameter);
        await service.PushPage(typeof(LifecycleModel), parameter, "page");
        await service.PushPage(typeof(LifecycleModel), parameter, "page", false);
        await service.PushPage(typeof(LifecycleModel), parameter, "page", false, false);
        await Assert.That(model.NavigatingCount).IsEqualTo(ParameterPageCount);
        await Assert.That(model.NavigatedCount).IsEqualTo(ParameterPageCount);
        await service.PopPage(parameter);
        await Assert.That(model.FromCount).IsEqualTo(1);
        await Assert.That(model.NavigatedCount).IsEqualTo(ParameterPageCount + 1);
        await service.PushPage(model, parameter, null, true, false);
        await service.PopPage(parameter, false);
        await Assert.That((await service.PageStack.FirstAsync()).Count).IsEqualTo(0);
        await service.PushPage(new PlainModel());
        await service.PushPage(model, parameter);
        await service.PopPage(parameter);
        await service.PopPage(parameter);
        await service.PushModal(model, parameter);
        await service.PushModal(model, parameter, "page");
        await service.PushModal(model, parameter, "page", false);
        await service.PushModal(typeof(LifecycleModel), parameter);
        await service.PushModal(typeof(LifecycleModel), parameter, "page");
        await service.PushModal(typeof(LifecycleModel), parameter, "page", false);
        await Assert.That((await service.ModalStack.FirstAsync()).Count).IsEqualTo(ParameterModalCount);
        await Assert.That(async () => await service.PushPage((INavigable)null!, parameter)).Throws<ArgumentNullException>();
        await Assert.That(async () => await service.PushPage(model, (INavigationParameter)null!)).Throws<ArgumentNullException>();
        await Assert.That(async () => await service.PushModal((INavigable)null!, parameter)).Throws<ArgumentNullException>();
        await Assert.That(async () => await service.PushModal(model, (INavigationParameter)null!)).Throws<ArgumentNullException>();
        await Assert.That(async () => await service.PopPage((INavigationParameter)null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Checks parameter overloads and both remaining-page lifecycle paths.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task GenericParameterOverloadsDeliverLifecycleNotifications()
    {
        using var view = new NavigationViewMock();
        var model = new LifecycleModel();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var parameter = new NavigationParameter();
        await service.PushPage(model, parameter);
        await service.PushPage(model, parameter, "page");
        await service.PushPage(model, parameter, "page", false);
        await service.PushPage(model, parameter, "page", false, false);
        await service.PushPage<LifecycleModel>(parameter);
        await service.PushPage<LifecycleModel>(parameter, "page");
        await service.PushPage<LifecycleModel>(parameter, "page", false);
        await service.PushPage<LifecycleModel>(parameter, "page", false, false);
        await Assert.That(model.NavigatingCount).IsEqualTo(ParameterPageCount);
        await Assert.That(model.NavigatedCount).IsEqualTo(ParameterPageCount);
        await service.PopPage(parameter);
        await Assert.That(model.FromCount).IsEqualTo(1);
        await Assert.That(model.NavigatedCount).IsEqualTo(ParameterPageCount + 1);
        await service.PushPage(model, parameter, null, true, false);
        await service.PopPage(parameter, false);
        await Assert.That((await service.PageStack.FirstAsync()).Count).IsEqualTo(0);
        await service.PushPage(new PlainModel());
        await service.PushPage(model, parameter);
        await service.PopPage(parameter);
        await service.PopPage(parameter);
        await service.PushModal(model, parameter);
        await service.PushModal(model, parameter, "page");
        await service.PushModal(model, parameter, "page", false);
        await service.PushModal<LifecycleModel>(parameter);
        await service.PushModal<LifecycleModel>(parameter, "page");
        await service.PushModal<LifecycleModel>(parameter, "page", false);
        await Assert.That((await service.ModalStack.FirstAsync()).Count).IsEqualTo(ParameterModalCount);
        await Assert.That(async () => await service.PushPage((INavigable)null!, parameter)).Throws<ArgumentNullException>();
        await Assert.That(async () => await service.PushPage(model, (INavigationParameter)null!)).Throws<ArgumentNullException>();
        await Assert.That(async () => await service.PushModal((INavigable)null!, parameter)).Throws<ArgumentNullException>();
        await Assert.That(async () => await service.PushModal(model, (INavigationParameter)null!)).Throws<ArgumentNullException>();
        await Assert.That(async () => await service.PopPage((INavigationParameter)null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Checks empty stacks, unmatched device events and protected stack operations.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task ImmediateNavigationCompletesLifecycleSubscriptions()
    {
        using var view = new EventView();
        var model = new LifecycleModel();
        using var service = new ParameterViewStackService(view, new ViewModelFactoryMock(model), new Splat.WrappingFullLogger(new Splat.NullLogger()));
        var parameter = new NavigationParameter();
        await service.PushPage(model, parameter);
        await service.PushModal(model, parameter);
        await service.PopPage(parameter);
        const int pageAndModalCount = 2;
        await Assert.That(model.NavigatingCount).IsEqualTo(pageAndModalCount);
        await Assert.That(model.FromCount).IsEqualTo(1);
    }

    /// <summary>Checks empty stacks, unmatched device events and protected stack operations.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task StackOperationsHandleEmptyAndDestructibleEntries()
    {
        using var view = new EventView();
        using var service = new StackService(view);
        var root = new PlainModel();
        var removed = new LifecycleModel();
        view.Popped.OnNext(root);
        await service.PushPage(root);
        view.Popped.OnNext(removed);
        await Assert.That(await service.TopPage()).IsSameReferenceAs(root);
        await service.PushPage(removed);
        await service.PushPage(new PlainModel());
        await service.PopToRootPage(false);
        await Assert.That(removed.DestroyCount).IsEqualTo(1);
        await Assert.That((await service.PageStack.FirstAsync()).Count).IsEqualTo(1);
        view.Popped.OnNext(root);
        await Assert.That((await service.PageStack.FirstAsync()).Count).IsEqualTo(0);
        using var stack = new BehaviorSubject<IImmutableList<IViewModel>>(ImmutableList<IViewModel>.Empty);
        await Assert.That(() => StackService.Pop(stack)).Throws<InvalidOperationException>();
        await Assert.That(static () => StackService.Pop(null!)).Throws<ArgumentNullException>();
        await Assert.That(() => StackService.Add(null!, root)).Throws<ArgumentNullException>();
        await Assert.That(static () => StackService.Root(null!)).Throws<InvalidOperationException>();
        await Assert.That(() => StackService.Root(stack)).Throws<InvalidOperationException>();
        StackService.Add(stack, root);
        await Assert.That(StackService.Pop(stack)).IsSameReferenceAs(root);
        service.Release(false);
        await service.PushPage(root);
        await Assert.That(await service.TopPage()).IsSameReferenceAs(root);
        await Assert.That(static () => new ViewStackService(null!)).Throws<ArgumentNullException>();
        await Assert.That(() => root.InvokeViewModelAction<IDestructible>(null!)).Throws<ArgumentNullException>();
    }

    /// <summary>Records navigation lifecycle calls.</summary>
    private sealed class LifecycleModel : INavigable, IDestructible
    {
        /// <inheritdoc />
        public string Id => nameof(LifecycleModel);

        /// <summary>Gets the number of navigation starts.</summary>
        public int NavigatingCount { get; private set; }

        /// <summary>Gets the number of arrivals.</summary>
        public int NavigatedCount { get; private set; }

        /// <summary>Gets the number of departures.</summary>
        public int FromCount { get; private set; }

        /// <summary>Gets the number of destruction calls.</summary>
        public int DestroyCount { get; private set; }

        /// <inheritdoc />
        public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter)
        {
            NavigatingCount++;
            return Observable.Return(Unit.Default);
        }

        /// <inheritdoc />
        public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter)
        {
            NavigatedCount++;
            return Observable.Return(Unit.Default);
        }

        /// <inheritdoc />
        public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter)
        {
            FromCount++;
            return Observable.Return(Unit.Default);
        }

        /// <inheritdoc />
        public void Destroy() => DestroyCount++;
    }

    /// <summary>A view model without optional lifecycle contracts.</summary>
    private sealed class PlainModel : IViewModel
    {
        /// <inheritdoc />
        public string Id => nameof(PlainModel);
    }

    /// <summary>Exposes protected stack operations for boundary checks.</summary>
    /// <param name="view">The event source.</param>
    private sealed class StackService(IView view) : ViewStackServiceBase(view, new ViewModelFactoryMock(), new Splat.WrappingFullLogger(new Splat.NullLogger()))
    {
        /// <summary>Adds a model.</summary>
        /// <param name="stack">The stack subject.</param>
        /// <param name="model">The model.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add(BehaviorSubject<IImmutableList<IViewModel>> stack, IViewModel model) => AddToStackAndTick(stack, model, false);

        /// <summary>Removes the top model.</summary>
        /// <param name="stack">The stack subject.</param>
        /// <returns>The removed model.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IViewModel Pop(BehaviorSubject<IImmutableList<IViewModel>> stack) => PopStackAndTick(stack);

        /// <summary>Keeps the root model.</summary>
        /// <param name="stack">The stack subject.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Root(BehaviorSubject<IImmutableList<IViewModel>> stack) => PopRootAndTick(stack);

        /// <summary>Exercises managed and unmanaged disposal paths.</summary>
        /// <param name="disposing">Whether to release managed resources.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Release(bool disposing) => Dispose(disposing);
    }

    /// <summary>Allows device pop events to be sent independently of service operations.</summary>
    private sealed class EventView : IView, IDisposable
    {
        /// <summary>Gets the device event source.</summary>
        public Subject<IViewModel?> Popped { get; } = new();

        /// <inheritdoc />
        public IScheduler MainThreadScheduler => ImmediateScheduler.Instance;

        /// <inheritdoc />
        public IObservable<IViewModel?> PagePopped => Popped;

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PopModal() => Observable.Return(Unit.Default);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PopPage(bool animate) => Observable.Return(Unit.Default);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PopToRootPage(bool animate) => PopPage(animate);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract, bool withNavigationPage) => Observable.Return(Unit.Default);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IObservable<Unit> PushPage(IViewModel viewModel, string? contract, bool resetStack, bool animate) => Observable.Return(Unit.Default);

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose() => Popped.Dispose();
    }
}
