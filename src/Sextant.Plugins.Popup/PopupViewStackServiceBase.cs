// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Mopups.Events;
using Mopups.Interfaces;
using Mopups.Pages;

namespace Sextant.Plugins.Popup;

/// <summary>Represents a popup view stack service implementation.</summary>
[System.Diagnostics.DebuggerDisplay("PopupViewStackServiceBase: {Pushing}")]
public class PopupViewStackServiceBase : ParameterViewStackServiceBase, IPopupViewStackService
{
    /// <summary>The platform popup navigation provider.</summary>
    private readonly IPopupNavigation _popupNavigation;

    /// <summary>Resolves popup pages for registered view models.</summary>
    private readonly IViewLocator _viewLocator;

    /// <summary>Initializes a new instance of the <see cref="PopupViewStackServiceBase"/> class.</summary>
    /// <param name="view">The navigation view.</param>
    /// <param name="popupNavigation">The popup provider.</param>
    /// <param name="viewLocator">The view locator.</param>
    /// <param name="viewModelFactory">The view model factory.</param>
    /// <param name="logger">The logger, or null to resolve the application logger.</param>
    internal PopupViewStackServiceBase(IView view, IPopupNavigation popupNavigation, IViewLocator viewLocator, IViewModelFactory viewModelFactory, Splat.IFullLogger? logger)
        : base(view, viewModelFactory, logger)
    {
        _popupNavigation = popupNavigation;
        _viewLocator = viewLocator;
        PopupSubject = new(ImmutableList<IViewModel>.Empty);
        Pushing = Observable.FromEventPattern<PopupNavigationEventArgs>(
            x => _popupNavigation.Pushing += x,
            x => _popupNavigation.Pushing -= x).Select(static x => new PopupNavigationEvent((IViewFor)x.EventArgs.Page, x.EventArgs.IsAnimated));
        Pushed = Observable.FromEventPattern<PopupNavigationEventArgs>(
            x => _popupNavigation.Pushed += x,
            x => _popupNavigation.Pushed -= x).Select(static x => new PopupNavigationEvent((IViewFor)x.EventArgs.Page, x.EventArgs.IsAnimated));
        Popping = Observable.FromEventPattern<PopupNavigationEventArgs>(
            x => _popupNavigation.Popping += x,
            x => _popupNavigation.Popping -= x).Select(static x => new PopupNavigationEvent((IViewFor)x.EventArgs.Page, x.EventArgs.IsAnimated));
        Popped = Observable.FromEventPattern<PopupNavigationEventArgs>(
            x => _popupNavigation.Popped += x,
            x => _popupNavigation.Popped -= x).Select(static x => new PopupNavigationEvent((IViewFor)x.EventArgs.Page, x.EventArgs.IsAnimated));
        _ = Popped.Subscribe(static popped => popped.ViewModel.InvokeViewModelAction<IDestructible>(static x => x.Destroy())).DisposeWith(NavigationDisposables);
        _ = PopupSubject.DisposeWith(NavigationDisposables);
    }

    /// <summary>Initializes a new instance of the <see cref = "PopupViewStackServiceBase"/> class.</summary>
    /// <param name = "view">The view.</param>
    /// <param name = "popupNavigation">The popup navigation.</param>
    /// <param name = "viewLocator">The view locator.</param>
    /// <param name = "viewModelFactory">The view model factory.</param>
    protected PopupViewStackServiceBase(IView view, IPopupNavigation popupNavigation, IViewLocator viewLocator, IViewModelFactory viewModelFactory)
        : this(view, popupNavigation, viewLocator, viewModelFactory, null)
    {
    }

    /// <inheritdoc/>
    public IObservable<PopupNavigationEvent> Pushing { get; }

    /// <inheritdoc/>
    public IObservable<PopupNavigationEvent> Pushed { get; }

    /// <inheritdoc/>
    public IObservable<PopupNavigationEvent> Popping { get; }

    /// <inheritdoc/>
    public IObservable<PopupNavigationEvent> Popped { get; }

    /// <inheritdoc/>
    public IReadOnlyList<IViewModel> PopupStack
    {
        get
        {
            List<IViewModel> stack = [];
            foreach (var page in _popupNavigation.PopupStack)
            {
                if (page is IViewFor<IViewModel> { ViewModel: { } viewModel })
                {
                    stack.Add(viewModel);
                }
            }

            return stack;
        }
    }

    /// <summary>Gets the popup subject that contains the stack history.</summary>
    protected BehaviorSubject<IImmutableList<IViewModel>> PopupSubject { get; }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup(IViewModel viewModel, string? contract) => PushPopup(viewModel, contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup(IViewModel viewModel) => PushPopup(viewModel, null, true);

    /// <inheritdoc/>
    public IObservable<Unit> PushPopup(IViewModel viewModel, string? contract, bool animate)
    {
        ArgumentNullException.ThrowIfNull(viewModel);
        PopupPage popupPage = LocatePopupFor(viewModel, contract);
        return Observable.FromAsync(() => _popupNavigation.PushAsync(popupPage, animate)).Do(_ =>
        {
            AddToStackAndTick(PopupSubject, viewModel, false);
            Logger.Debug($"Added page '{viewModel.Id}' (contract '{contract}') to stack.");
        });
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup(Type viewModelType, string? contract) => PushPopup(viewModelType, contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup(Type viewModelType) => PushPopup(viewModelType, null, true);

    /// <inheritdoc/>
    public IObservable<Unit> PushPopup(Type viewModelType, string? contract, bool animate)
    {
        var viewModel = Factory.Create(viewModelType, contract);
        return PushPopup(viewModel, contract, animate);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup(INavigable viewModel, INavigationParameter navigationParameter, string? contract) => PushPopup(viewModel, navigationParameter, contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup(INavigable viewModel, INavigationParameter navigationParameter) => PushPopup(viewModel, navigationParameter, null, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup(INavigable viewModel, INavigationParameter navigationParameter, string? contract, bool animate) => Observable.Create<Unit>(observer =>
    {
        ArgumentNullException.ThrowIfNull(viewModel);
        ArgumentNullException.ThrowIfNull(navigationParameter);
        var compositeDisposable = new CompositeDisposable();
        var popup = LocatePopupFor(viewModel, contract);
        _ = viewModel.WhenNavigatingTo(navigationParameter).Subscribe().DisposeWith(compositeDisposable);
        _ = Observable.FromAsync(() => _popupNavigation.PushAsync(popup, animate)).Do(unit =>
        {
            AddToStackAndTick(PopupSubject, viewModel, false);
            Logger.Debug($"Added page '{viewModel.Id}' (contract '{contract}') to stack.");
            _ = viewModel.WhenNavigatedTo(navigationParameter).Subscribe().DisposeWith(compositeDisposable);
        }).Select(static _ => Unit.Default).Subscribe(observer).DisposeWith(compositeDisposable);
        return Disposable.Create(compositeDisposable, static disposable => disposable.Dispose());
    });

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup(Type viewModelType, INavigationParameter navigationParameter, string? contract) => PushPopup(viewModelType, navigationParameter, contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup(Type viewModelType, INavigationParameter navigationParameter) => PushPopup(viewModelType, navigationParameter, null, true);

    /// <inheritdoc/>
    public IObservable<Unit> PushPopup(Type viewModelType, INavigationParameter navigationParameter, string? contract, bool animate)
    {
        var viewModel = (INavigable)Factory.Create(viewModelType, contract);
        return PushPopup(viewModel, navigationParameter, contract, animate);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup<TViewModel>(string? contract)
        where TViewModel : IViewModel => PushPopup<TViewModel>(contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup<TViewModel>()
        where TViewModel : IViewModel => PushPopup<TViewModel>(null, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup<TViewModel>(string? contract, bool animate)
        where TViewModel : IViewModel => PushPopup(Factory.Create<TViewModel>(contract), contract, animate);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup<TViewModel>(INavigationParameter navigationParameter, string? contract)
        where TViewModel : INavigable => PushPopup<TViewModel>(navigationParameter, contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup<TViewModel>(INavigationParameter navigationParameter)
        where TViewModel : INavigable => PushPopup<TViewModel>(navigationParameter, null, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup<TViewModel>(INavigationParameter navigationParameter, string? contract, bool animate)
        where TViewModel : INavigable => PushPopup(Factory.Create<TViewModel>(contract), navigationParameter, contract, animate);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopPopup() => PopPopup(true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopPopup(bool animate) => Observable.FromAsync(() => _popupNavigation.PopAsync(animate));

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopAllPopups() => PopAllPopups(true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopAllPopups(bool animate) => Observable.FromAsync(() => _popupNavigation.PopAllAsync(animate)).Do(_ => PopRootAndTick(PopupSubject));

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> RemovePopup(IViewModel viewModel, string? contract) => RemovePopup(viewModel, contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> RemovePopup(IViewModel viewModel) => RemovePopup(viewModel, null, true);

    /// <inheritdoc/>
    public IObservable<Unit> RemovePopup(IViewModel viewModel, string? contract, bool animate)
    {
        ArgumentNullException.ThrowIfNull(viewModel);
        PopupPage popupPage = LocatePopupFor(viewModel, contract);
        return Observable.FromAsync(() => _popupNavigation.RemovePageAsync(popupPage, animate)).Do(_ => RemoveFromStackAndTick(PopupSubject, viewModel));
    }

    /// <summary>Removes a view model and publishes the remaining stack.</summary>
    /// <typeparam name="T">The stack element type.</typeparam>
    /// <param name="stackSubject">The stack and its change notifications.</param>
    /// <param name="item">The view model to remove.</param>
    private static void RemoveFromStackAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject, T item)
    {
        ArgumentNullException.ThrowIfNull(stackSubject);
        var stack = stackSubject.Value;
        stack = stack.Remove(item);
        stackSubject.OnNext(stack);
    }

    /// <summary>Resolves a popup page and assigns its view model.</summary>
    /// <param name="viewModel">The view model to display.</param>
    /// <param name="contract">The optional view registration contract.</param>
    /// <returns>The initialized popup page.</returns>
    /// <exception cref="InvalidOperationException">Thrown when <c>_viewLocator.ResolveView(viewModel, contract)</c> is <see langword="null"/>.</exception>
    private SextantPopupPage LocatePopupFor(IViewModel viewModel, string? contract)
    {
        var view = _viewLocator.ResolveView(viewModel, contract) ?? throw new InvalidOperationException(
            $"No view could be located for type '{viewModel.GetType().FullName}', contract '{contract}'. Be sure Splat has an appropriate registration.");
        if (view is not SextantPopupPage page)
        {
            throw new InvalidOperationException($"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' is not a {nameof(SextantPopupPage)}.");
        }

        page.ViewModel = viewModel;
        return page;
    }
}
