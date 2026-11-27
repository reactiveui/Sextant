// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Sextant;

/// <summary>Abstract base class for view stack services.</summary>
/// <remarks>
/// Initializes a new instance of the <see cref = "ParameterViewStackServiceBase"/> class.
/// </remarks>
public class ParameterViewStackServiceBase : ViewStackServiceBase, IParameterViewStackService
{
    /// <summary>Initializes a new instance of the <see cref="ParameterViewStackServiceBase"/> class.</summary>
    /// <param name="view">The navigation view.</param>
    /// <param name="viewModelFactory">The view model factory.</param>
    public ParameterViewStackServiceBase(IView view, IViewModelFactory viewModelFactory)
        : base(view, viewModelFactory)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="ParameterViewStackServiceBase"/> class.</summary>
    /// <param name="view">The navigation view.</param>
    /// <param name="viewModelFactory">The view model factory.</param>
    /// <param name="logger">The logger, or null to resolve the application logger.</param>
    internal ParameterViewStackServiceBase(IView view, IViewModelFactory viewModelFactory, Splat.IFullLogger? logger)
        : base(view, viewModelFactory, logger)
    {
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(INavigable navigableViewModel, INavigationParameter parameter, string? contract, bool resetStack) =>
        PushPage(navigableViewModel, parameter, contract, resetStack, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(INavigable navigableViewModel, INavigationParameter parameter, string? contract) => PushPage(navigableViewModel, parameter, contract, false, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(INavigable navigableViewModel, INavigationParameter parameter) => PushPage(navigableViewModel, parameter, null, false, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(INavigable navigableViewModel, INavigationParameter parameter, string? contract, bool resetStack, bool animate)
    {
        ArgumentNullException.ThrowIfNull(navigableViewModel);
        ArgumentNullException.ThrowIfNull(parameter);

        // The view model's callbacks and the platform push run on the main thread, because a callback that sets a
        // property updates the controls bound to it. A platform push can finish on another thread, so the stack update
        // and WhenNavigatedTo move back to the main thread too.
        return Observable.Create<Unit>(observer =>
        {
            var composite = new CompositeDisposable();
            _ = navigableViewModel.WhenNavigatingTo(parameter).ObserveOn(View.MainThreadScheduler).Subscribe(unit =>
                Logger.Debug($"Called `WhenNavigatingTo` on '{navigableViewModel.Id}' passing parameter {parameter}")).DisposeWith(composite);
            _ = View.PushPage(navigableViewModel, contract, resetStack, animate).ObserveOn(View.MainThreadScheduler).Do(unit =>
            {
                AddToStackAndTick(PageSubject, navigableViewModel, resetStack);
                Logger.Debug($"Added page '{navigableViewModel.Id}' (contract '{contract}') to stack.");
                _ = navigableViewModel.WhenNavigatedTo(parameter).ObserveOn(View.MainThreadScheduler).Subscribe(navigated =>
                    Logger.Debug($"Called `WhenNavigatedTo` on '{navigableViewModel.Id}' passing parameter {parameter}")).DisposeWith(composite);
            }).Subscribe(observer).DisposeWith(composite);
            return Disposable.Create(composite, static disposable => disposable.Dispose());
        }).SubscribeOn(View.MainThreadScheduler);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(Type viewModelType, INavigationParameter parameter, string? contract, bool resetStack) => PushPage(viewModelType, parameter, contract, resetStack, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(Type viewModelType, INavigationParameter parameter, string? contract) => PushPage(viewModelType, parameter, contract, false, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(Type viewModelType, INavigationParameter parameter) => PushPage(viewModelType, parameter, null, false, true);

    /// <inheritdoc/>
    public IObservable<Unit> PushPage(Type viewModelType, INavigationParameter parameter, string? contract, bool resetStack, bool animate)
    {
        var viewModel = (INavigable)Factory.Create(viewModelType, contract);
        return PushPage(viewModel, parameter, contract, resetStack, animate);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage<TViewModel>(INavigationParameter parameter, string? contract, bool resetStack)
        where TViewModel : INavigable => PushPage<TViewModel>(parameter, contract, resetStack, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage<TViewModel>(INavigationParameter parameter, string? contract)
        where TViewModel : INavigable => PushPage<TViewModel>(parameter, contract, false, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage<TViewModel>(INavigationParameter parameter)
        where TViewModel : INavigable => PushPage<TViewModel>(parameter, null, false, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage<TViewModel>(INavigationParameter parameter, string? contract, bool resetStack, bool animate)
        where TViewModel : INavigable => PushPage(Factory.Create<TViewModel>(contract), parameter, contract, resetStack, animate);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(INavigable navigableModal, INavigationParameter parameter, string? contract) => PushModal(navigableModal, parameter, contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(INavigable navigableModal, INavigationParameter parameter) => PushModal(navigableModal, parameter, null, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(INavigable navigableModal, INavigationParameter parameter, string? contract, bool withNavigationPage)
    {
        ArgumentNullException.ThrowIfNull(navigableModal);
        ArgumentNullException.ThrowIfNull(parameter);

        // Runs on the main thread for the same reason as PushPage.
        return Observable.Create<Unit>(observer =>
        {
            var composite = new CompositeDisposable();
            _ = navigableModal.WhenNavigatingTo(parameter).ObserveOn(View.MainThreadScheduler).Subscribe(navigating =>
                Logger.Debug($"Called `WhenNavigatingTo` on '{navigableModal.Id}' passing parameter {parameter}")).DisposeWith(composite);
            _ = View.PushModal(navigableModal, contract, withNavigationPage).ObserveOn(View.MainThreadScheduler).Do(unit =>
            {
                AddToStackAndTick(ModalSubject, navigableModal, false);
                Logger.Debug($"Added modal '{navigableModal.Id}' (contract '{contract}') to stack.");
                _ = navigableModal.WhenNavigatedTo(parameter).ObserveOn(View.MainThreadScheduler).Subscribe(navigated =>
                    Logger.Debug($"Called `WhenNavigatedTo` on '{navigableModal.Id}' passing parameter {parameter}")).DisposeWith(composite);
            }).Subscribe(observer).DisposeWith(composite);
            return Disposable.Create(composite, static disposable => disposable.Dispose());
        }).SubscribeOn(View.MainThreadScheduler);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(Type viewModelType, INavigationParameter parameter, string? contract) => PushModal(viewModelType, parameter, contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(Type viewModelType, INavigationParameter parameter) => PushModal(viewModelType, parameter, null, true);

    /// <inheritdoc/>
    public IObservable<Unit> PushModal(Type viewModelType, INavigationParameter parameter, string? contract, bool withNavigationPage)
    {
        var viewModel = (INavigable)Factory.Create(viewModelType, contract);
        return PushModal(viewModel, parameter, contract, withNavigationPage);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal<TViewModel>(INavigationParameter parameter, string? contract)
        where TViewModel : INavigable => PushModal<TViewModel>(parameter, contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal<TViewModel>(INavigationParameter parameter)
        where TViewModel : INavigable => PushModal<TViewModel>(parameter, null, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal<TViewModel>(INavigationParameter parameter, string? contract, bool withNavigationPage)
        where TViewModel : INavigable => PushModal(Factory.Create<TViewModel>(contract), parameter, contract, withNavigationPage);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopPage(INavigationParameter parameter) => PopPage(parameter, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopPage(INavigationParameter parameter, bool animate)
    {
        ArgumentNullException.ThrowIfNull(parameter);

        // Runs on the main thread for the same reason as PushPage: WhenNavigatedFrom, Destroy and WhenNavigatedTo all
        // reach the view models after a platform pop that can finish on another thread.
        return Observable.Create<Unit>(observer =>
        {
            var stack = PageSubject.Value;
            var poppedPage = stack[stack.Count - 1];
            var composite = new CompositeDisposable();
            _ = View.PopPage(animate).ObserveOn(View.MainThreadScheduler).Do(unit =>
            {
                _ = poppedPage.InvokeViewModelAction<INavigable>(x =>
                    x.WhenNavigatedFrom(parameter).ObserveOn(View.MainThreadScheduler).Subscribe(navigatedFrom =>
                        Logger.Debug($"Called `WhenNavigatedFrom` on '{poppedPage.Id}' passing parameter {parameter}"))
                        .DisposeWith(composite)).InvokeViewModelAction<IDestructible>(static x => x.Destroy());
                var remaining = PageSubject.Value;
                if (remaining.Count > 0 && remaining[remaining.Count - 1] is INavigated navigated)
                {
                    _ = navigated.WhenNavigatedTo(parameter).ObserveOn(View.MainThreadScheduler).Subscribe(navigatedTo =>
                        Logger.Debug($"Called `WhenNavigatedTo` passing parameter {parameter}")).DisposeWith(composite);
                }
            }).Subscribe(observer).DisposeWith(composite);
            return Disposable.Create(composite, static disposable => disposable.Dispose());
        }).SubscribeOn(View.MainThreadScheduler);
    }
}
