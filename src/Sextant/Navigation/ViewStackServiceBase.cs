// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Splat;

namespace Sextant;

/// <summary>Abstract base class for view stack services.</summary>
/// <seealso cref = "IViewStackService"/>
/// <seealso cref = "IDisposable"/>
/// <seealso cref = "IEnableLogger"/>
[System.Diagnostics.DebuggerDisplay("ViewStackServiceBase: {ModalStack}")]
public class ViewStackServiceBase : IViewStackService, IDisposable, IEnableLogger
{
    /// <summary>Initializes a new instance of the <see cref="ViewStackServiceBase"/> class.</summary>
    /// <param name="view">The navigation view.</param>
    /// <param name="viewModelFactory">The view model factory.</param>
    /// <param name="logger">The logger, or null to resolve the application logger.</param>
    /// <exception cref="ArgumentNullException">The view is null.</exception>
    internal ViewStackServiceBase(IView view, IViewModelFactory viewModelFactory, IFullLogger? logger)
    {
        View = view ?? throw new ArgumentNullException(nameof(view));
        Logger = logger ?? this.Log();
        Factory = viewModelFactory;
        ModalSubject = new(ImmutableList<IViewModel>.Empty);
        PageSubject = new(ImmutableList<IViewModel>.Empty);
        _ = View.PagePopped.Subscribe(poppedPage =>
        {
            var currentPageStack = PageSubject.Value;
            if (currentPageStack.Count > 0 && poppedPage == currentPageStack[currentPageStack.Count - 1])
            {
                var removedPage = PopStackAndTick(PageSubject);
                Logger.Debug(CultureInfo.InvariantCulture, "Removed page '{0}' from stack.", removedPage.Id);
            }
        }).DisposeWith(NavigationDisposables);
        _ = ModalSubject.DisposeWith(NavigationDisposables);
        _ = PageSubject.DisposeWith(NavigationDisposables);
    }

    /// <summary>Initializes a new instance of the <see cref = "ViewStackServiceBase"/> class.</summary>
    /// <param name = "view">The view.</param>
    /// <param name = "viewModelFactory">The view model factory.</param>
    /// <exception cref = "ArgumentNullException">Thrown when <c>view</c> is <see langword="null"/>.</exception>
    protected ViewStackServiceBase(IView view, IViewModelFactory viewModelFactory)
        : this(view, viewModelFactory, null)
    {
    }

    /// <summary>Gets the modal navigation stack.</summary>
    public IObservable<IImmutableList<IViewModel>> ModalStack => ModalSubject.AsObservable();

    /// <summary>Gets the page navigation stack.</summary>
    public IObservable<IImmutableList<IViewModel>> PageStack => PageSubject.AsObservable();

    /// <summary>Gets the current view on the stack.</summary>
    public IView View { get; }

    /// <summary>Gets the logger.</summary>
    protected IFullLogger Logger { get; }

    /// <summary>Gets the view model factory.</summary>
    protected IViewModelFactory Factory { get; }

    /// <summary>Gets the modal subject.</summary>
    protected BehaviorSubject<IImmutableList<IViewModel>> ModalSubject { get; }

    /// <summary>Gets the page subject.</summary>
    protected BehaviorSubject<IImmutableList<IViewModel>> PageSubject { get; }

    /// <summary>Gets the navigation disposables.</summary>
    protected CompositeDisposable NavigationDisposables { get; } = [];

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopModal() => PopModal(true);

    /// <summary>Pops the <see cref = "INavigable"/> off the stack.</summary>
    /// <param name = "animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the pop is complete.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopModal(bool animate) => View.PopModal().Do(unit =>
    {
        var modal = PopStackAndTick(ModalSubject);
        _ = modal.InvokeViewModelAction<IDestructible>(static x => x.Destroy());
    });

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopPage() => PopPage(true);

    /// <inheritdoc/>
    public IObservable<Unit> PopPage(bool animate)
    {
        var stack = PageSubject.Value;
        var top = stack[stack.Count - 1];
        return View.PopPage(animate).Do(_ => top.InvokeViewModelAction<IDestructible>(static x => x.Destroy()));
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopToRootPage() => PopToRootPage(true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopToRootPage(bool animate) => View.PopToRootPage(animate).Do(_ => PopRootAndTick(PageSubject));

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(IViewModel modal, string? contract) => PushModal(modal, contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(IViewModel modal) => PushModal(modal, null, true);

    /// <inheritdoc/>
    public IObservable<Unit> PushModal(IViewModel modal, string? contract, bool withNavigationPage)
    {
        ArgumentNullException.ThrowIfNull(modal);
        return View.PushModal(modal, contract, withNavigationPage).Do(_ =>
        {
            AddToStackAndTick(ModalSubject, modal, false);
            Logger.Debug($"Added modal '{modal.Id}' (contract '{contract}') to stack.");
        });
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(Type viewModelType, string? contract) => PushModal(viewModelType, contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(Type viewModelType) => PushModal(viewModelType, null, true);

    /// <inheritdoc/>
    public IObservable<Unit> PushModal(Type viewModelType, string? contract, bool withNavigationPage)
    {
        var viewmodel = Factory.Create(viewModelType, contract);
        return PushModal(viewmodel, contract, withNavigationPage);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal<TViewModel>(string? contract)
        where TViewModel : IViewModel => PushModal<TViewModel>(contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal<TViewModel>()
        where TViewModel : IViewModel => PushModal<TViewModel>(null, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal<TViewModel>(string? contract, bool withNavigationPage)
        where TViewModel : IViewModel => PushModal(Factory.Create<TViewModel>(contract), contract, withNavigationPage);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(Type viewModelType, string? contract, bool resetStack) => PushPage(viewModelType, contract, resetStack, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(Type viewModelType, string? contract) => PushPage(viewModelType, contract, false, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(Type viewModelType) => PushPage(viewModelType, null, false, true);

    /// <inheritdoc/>
    public IObservable<Unit> PushPage(Type viewModelType, string? contract, bool resetStack, bool animate)
    {
        var viewModel = Factory.Create(viewModelType, contract);
        return PushPage(viewModel, contract, resetStack, animate);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(IViewModel page, string? contract, bool resetStack) => PushPage(page, contract, resetStack, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(IViewModel page, string? contract) => PushPage(page, contract, false, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(IViewModel page) => PushPage(page, null, false, true);

    /// <inheritdoc/>
    public IObservable<Unit> PushPage(IViewModel page, string? contract, bool resetStack, bool animate)
    {
        ArgumentNullException.ThrowIfNull(page);
        return View.PushPage(page, contract, resetStack, animate).Do(_ =>
        {
            AddToStackAndTick(PageSubject, page, resetStack);
            Logger.Debug($"Added page '{page.Id}' (contract '{contract}') to stack.");
        });
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(INavigable page, string? contract, bool resetStack) => PushPage(page, contract, resetStack, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(INavigable page, string? contract) => PushPage(page, contract, false, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(INavigable page) => PushPage(page, null, false, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(INavigable page, string? contract, bool resetStack, bool animate) => PushPage((IViewModel)page, contract, resetStack, animate);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage<TViewModel>(string? contract, bool resetStack)
        where TViewModel : IViewModel => PushPage<TViewModel>(contract, resetStack, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage<TViewModel>(string? contract)
        where TViewModel : IViewModel => PushPage<TViewModel>(contract, false, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage<TViewModel>()
        where TViewModel : IViewModel => PushPage<TViewModel>(null, false, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage<TViewModel>(string? contract, bool resetStack, bool animate)
        where TViewModel : IViewModel => PushPage(Factory.Create<TViewModel>(contract), contract, resetStack, animate);

    /// <summary>Returns the top modal from the current modal stack.</summary>
    /// <returns>An observable that signals the top modal view model.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<IViewModel> TopModal() => ModalSubject.FirstAsync().Select(static x => x[x.Count - 1]);

    /// <summary>Returns the top page from the current navigation stack.</summary>
    /// <returns>An observable that signals the top page view model.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<IViewModel> TopPage() => PageSubject.FirstAsync().Select(static x => x[x.Count - 1]);

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Adds to stack and tick.</summary>
    /// <typeparam name = "T">The view model type.</typeparam>
    /// <param name = "stackSubject">The stack subject.</param>
    /// <param name = "item">The item.</param>
    /// <param name = "reset">if set to <c>true</c> [reset].</param>
    protected static void AddToStackAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject, T item, bool reset)
    {
        ArgumentNullException.ThrowIfNull(stackSubject);
        var stack = stackSubject.Value;
        stack = reset ? ImmutableList.Create(item) : stack.Add(item);
        stackSubject.OnNext(stack);
    }

    /// <summary>Pops the stack and notifies observers.</summary>
    /// <typeparam name = "T">The view model type.</typeparam>
    /// <param name = "stackSubject">The stack subject.</param>
    /// <returns>The view model popped.</returns>
    /// <exception cref = "InvalidOperationException">Stack is empty.</exception>
    protected static T PopStackAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject)
    {
        ArgumentNullException.ThrowIfNull(stackSubject);
        var stack = stackSubject.Value;
        if (stack.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        var removedItem = stack[stack.Count - 1];
        stack = stack.RemoveAt(stack.Count - 1);
        stackSubject.OnNext(stack);
        return removedItem;
    }

    /// <summary>Pops the root and notifies observers.</summary>
    /// <typeparam name = "T">The view model type.</typeparam>
    /// <param name = "stackSubject">The stack subject.</param>
    /// <exception cref = "InvalidOperationException">Stack is empty.</exception>
    protected static void PopRootAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject)
    {
        if (stackSubject is null || stackSubject.Value.Count == 0)
        {
            throw new InvalidOperationException("Stack is empty.");
        }

        var stack = stackSubject.Value;
        for (var index = stack.Count - 1; index > 0; index--)
        {
            if (stack[index] is IDestructible destructible)
            {
                destructible.Destroy();
            }
        }

        stackSubject.OnNext(stack.RemoveRange(1, stack.Count - 1));
    }

    /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
    /// <param name = "disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            NavigationDisposables.Dispose();
        }
    }
}
