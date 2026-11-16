// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace Sextant.Maui;

/// <summary>The main navigation view.</summary>
[System.Diagnostics.DebuggerDisplay("NavigationView: {MainThreadScheduler}")]
public class NavigationView : NavigationPage, IView, IEnableLogger, IDisposable
{
    /// <summary>Distinguishes programmatic navigation from device back actions.</summary>
    private readonly BehaviorSubject<NavigationSource> _navigationSource = new(NavigationSource.Device);

    /// <summary>Resolves registered views for navigation.</summary>
    private readonly IViewLocator _viewLocator;

    /// <summary>The owned behavior that observes device back actions.</summary>
    private readonly NavigationPageSystemPopBehavior _popBehavior;

    /// <summary>Ensures navigation subscriptions are released once.</summary>
    private int _disposed;

    /// <summary>Initializes a new instance of the <see cref = "NavigationView"/> class.</summary>
    public NavigationView()
        : this(RxSchedulers.MainThreadScheduler, ReactiveUI.Binding.Reactive.ViewLocator.GetCurrent())
    {
    }

    /// <summary>Initializes a new instance of the <see cref = "NavigationView"/> class.</summary>
    /// <param name = "mainScheduler">The main scheduler to scheduler UI tasks on.</param>
    /// <param name = "viewLocator">The view locator which will find views associated with view models.</param>
    /// <param name = "rootPage">The starting root page.</param>
    public NavigationView(IScheduler mainScheduler, IViewLocator viewLocator, Page rootPage)
        : base(rootPage)
    {
        MainThreadScheduler = mainScheduler;
        _viewLocator = viewLocator;
        PagePopped = NavigationEvents.ObserveViewModelPops(handler => Popped += handler, handler => Popped -= handler);
        _popBehavior = new(_navigationSource.AsObservable());
        Behaviors.Add(_popBehavior);
    }

    /// <summary>Initializes a new instance of the <see cref = "NavigationView"/> class.</summary>
    /// <param name = "mainScheduler">The main scheduler to scheduler UI tasks on.</param>
    /// <param name = "viewLocator">The view locator which will find views associated with view models.</param>
    public NavigationView(IScheduler mainScheduler, IViewLocator viewLocator)
    {
        MainThreadScheduler = mainScheduler;
        _viewLocator = viewLocator;
        PagePopped = NavigationEvents.ObserveViewModelPops(handler => Popped += handler, handler => Popped -= handler);
        _popBehavior = new(_navigationSource.AsObservable());
        Behaviors.Add(_popBehavior);
    }

    /// <inheritdoc/>
    public IScheduler MainThreadScheduler { get; }

    /// <inheritdoc/>
    public IObservable<IViewModel> PagePopped { get; }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopModal() => Navigation.PopModalAsync().ToObservable()
        .Select(static _ => Unit.Default).ObserveOn(MainThreadScheduler);

    /// <inheritdoc/>
    public IObservable<Unit> PopPage(bool animate)
    {
        _navigationSource.OnNext(NavigationSource.NavigationService);
        return Navigation.PopAsync(animate).ToObservable().Select(static _ => Unit.Default).ObserveOn(MainThreadScheduler).Finally(() => _navigationSource.OnNext(NavigationSource.Device));
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopToRootPage(bool animate) => Navigation.PopToRootAsync(animate).ToObservable().Select(static _ => Unit.Default).ObserveOn(MainThreadScheduler);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract) => PushModal(modalViewModel, contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract, bool withNavigationPage) => Observable.Start(
        () =>
    {
        var page = LocatePageFor(modalViewModel, contract);
        SetPageTitle(page, modalViewModel.Id);
        return withNavigationPage ? new NavigationPage(page) : page;
    },
        CurrentThreadScheduler.Instance).ObserveOn(CurrentThreadScheduler.Instance).SelectMany(page => Navigation.PushModalAsync(page).ToObservable());

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(IViewModel viewModel, string? contract, bool resetStack, bool animate) => Observable.Start(
        () =>
    {
        var page = LocatePageFor(viewModel, contract);
        SetPageTitle(page, viewModel.Id);
        return page;
    },
        CurrentThreadScheduler.Instance).ObserveOn(CurrentThreadScheduler.Instance).SelectMany(page =>
NavigationOperations.PushPage(Navigation, page, resetStack, animate));

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Releases navigation notifications and the owned pop behavior.</summary>
    /// <param name="disposing">True to release managed navigation resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        _popBehavior.Dispose();
        _ = Behaviors.Remove(_popBehavior);
        _navigationSource.Dispose();
    }

    /// <summary>Sets the page title from its view model identifier.</summary>
    /// <param name="page">The page to label.</param>
    /// <param name="resourceKey">The view model identifier.</param>
    private static void SetPageTitle(Page page, string? resourceKey) => page.Title = resourceKey;

    /// <summary>Resolves and initializes the MAUI page for a view model.</summary>
    /// <param name="viewModel">The view model to display.</param>
    /// <param name="contract">The optional view registration contract.</param>
    /// <returns>The initialized page.</returns>
    /// <exception cref="InvalidOperationException">Thrown when <c>_viewLocator.ResolveView(viewModel, contract)</c> is <see langword="null"/>.</exception>
    private Page LocatePageFor(object viewModel, string? contract)
    {
        var view = _viewLocator.ResolveView(viewModel, contract) ?? throw new InvalidOperationException(
            $"No view could be located for type '{viewModel.GetType().FullName}', contract '{contract}'. Be sure Splat has an appropriate registration.");
        if (view is not Page page)
        {
            throw new InvalidOperationException($"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' is not a Page.");
        }

        view.ViewModel = viewModel;
        return page;
    }
}
