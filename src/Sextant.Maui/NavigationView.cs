// Copyright (c) 2025 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;

namespace Sextant.Maui;

/// <summary>
/// The main navigation view.
/// </summary>
public class NavigationView : NavigationPage, IView, IEnableLogger
{
    private readonly ISubject<NavigationSource> _navigationSource =
        new BehaviorSubject<NavigationSource>(NavigationSource.Device);

    private readonly IScheduler _backgroundScheduler;
    private readonly IViewLocator _viewLocator;
    private readonly IFullLogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationView"/> class.
    /// </summary>
    public NavigationView()
        : this(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, ViewLocator.Current)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationView"/> class.
    /// </summary>
    /// <param name="mainScheduler">The main scheduler to scheduler UI tasks on.</param>
    /// <param name="backgroundScheduler">The background scheduler.</param>
    /// <param name="viewLocator">The view locator which will find views associated with view models.</param>
    /// <param name="rootPage">The starting root page.</param>
    public NavigationView(IScheduler mainScheduler, IScheduler backgroundScheduler, IViewLocator viewLocator, Page rootPage)
        : base(rootPage)
    {
        _backgroundScheduler = backgroundScheduler;
        MainThreadScheduler = mainScheduler;
        _viewLocator = viewLocator;
        _logger = this.Log();

        PagePopped =
            Observable
                .FromEvent<EventHandler<NavigationEventArgs>, IViewModel>(
                    handler =>
                    {
                        void Handler(object? sender, NavigationEventArgs args)
                        {
                            if (args.Page.BindingContext is IViewModel viewModel)
                            {
                                handler(viewModel);
                            }
                        }

                        return Handler;
                    },
                    x => Popped += x,
                    x => Popped -= x);

        Behaviors.Add(new NavigationPageSystemPopBehavior(_navigationSource.AsObservable()));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationView"/> class.
    /// </summary>
    /// <param name="mainScheduler">The main scheduler to scheduler UI tasks on.</param>
    /// <param name="backgroundScheduler">The background scheduler.</param>
    /// <param name="viewLocator">The view locator which will find views associated with view models.</param>
    public NavigationView(IScheduler mainScheduler, IScheduler backgroundScheduler, IViewLocator viewLocator)
    {
        MainThreadScheduler = mainScheduler;
        _backgroundScheduler = backgroundScheduler;
        _viewLocator = viewLocator;
        _logger = this.Log();

        PagePopped =
            Observable
                .FromEvent<EventHandler<NavigationEventArgs>, IViewModel>(
                    handler =>
                    {
                        void Handler(object? sender, NavigationEventArgs args)
                        {
                            if (args.Page.BindingContext is IViewModel viewModel)
                            {
                                handler(viewModel);
                            }
                        }

                        return Handler;
                    },
                    x => Popped += x,
                    x => Popped -= x);

        Behaviors.Add(new NavigationPageSystemPopBehavior(_navigationSource.AsObservable()));
    }

    /// <inheritdoc />
    public IScheduler MainThreadScheduler { get; }

    /// <inheritdoc />
    public IObservable<IViewModel> PagePopped { get; }

    /// <inheritdoc />
    public IObservable<Unit> PopModal() =>
        Navigation
            .PopModalAsync()
            .ToObservable()
            .Select(_ => Unit.Default)
            .ObserveOn(MainThreadScheduler); // XF completes the pop operation on a background thread :/

    /// <inheritdoc />
    public IObservable<Unit> PopPage(bool animate)
    {
        _navigationSource.OnNext(NavigationSource.NavigationService);

        return Navigation
            .PopAsync(animate)
            .ToObservable()
            .Select(_ => Unit.Default)
            .ObserveOn(MainThreadScheduler)
            .Finally(() => _navigationSource.OnNext(NavigationSource.Device));
    }

    /// <inheritdoc />
    public IObservable<Unit> PopToRootPage(bool animate) =>
        Navigation
            .PopToRootAsync(animate)
            .ToObservable()
            .Select(_ => Unit.Default)
            .ObserveOn(MainThreadScheduler);

    /// <inheritdoc />
    public IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract, bool withNavigationPage = true) =>
        Observable
            .Start(
                () =>
                {
                    var page = LocatePageFor(modalViewModel, contract);
                    SetPageTitle(page, modalViewModel.Id);
                    return withNavigationPage ? new NavigationPage(page) : page;
                },
                CurrentThreadScheduler.Instance)
            .ObserveOn(CurrentThreadScheduler.Instance)
            .SelectMany(
                page =>
                    Navigation
                        .PushModalAsync(page)
                        .ToObservable());

    /// <inheritdoc />
    public IObservable<Unit> PushPage(
        IViewModel viewModel,
        string? contract,
        bool resetStack,
        bool animate) =>
        Observable
            .Start(
                () =>
                {
                    var page = LocatePageFor(viewModel, contract);
                    SetPageTitle(page, viewModel.Id);
                    return page;
                },
                CurrentThreadScheduler.Instance)
            .ObserveOn(CurrentThreadScheduler.Instance)
            .SelectMany(
                page =>
                {
                    if (resetStack)
                    {
                        if (Navigation.NavigationStack.Count == 0)
                        {
                            return Navigation.PushAsync(page, false).ToObservable();
                        }

                        if (Navigation.NavigationStack.Count == 1)
                        {
                            // If there's only the root page, we need to use a different approach.
                            // We'll insert the new page before the root, then remove the root.
                            Navigation.InsertPageBefore(page, Navigation.NavigationStack[0]);
                            var oldRoot = Navigation.NavigationStack[1]; // The old root is now at index 1
                            Navigation.RemovePage(oldRoot);
                            return Observable.Return(Unit.Default);
                        }

                        // For multiple pages, first pop to root, then replace the root
                        return Navigation.PopToRootAsync(false).ToObservable()
                            .Do(_ =>
                            {
                                // Now there should only be the root page left
                                if (Navigation.NavigationStack.Count == 1)
                                {
                                    Navigation.InsertPageBefore(page, Navigation.NavigationStack[0]);
                                    var oldRoot = Navigation.NavigationStack[1];
                                    Navigation.RemovePage(oldRoot);
                                }
                            })
                            .Select(_ => Unit.Default);
                    }

                    return Navigation
                        .PushAsync(page, animate)
                        .ToObservable();
                });

    private static void SetPageTitle(Page page, string? resourceKey) =>

        // var title = Localize.GetString(resourceKey);
        // TODO: ensure resourceKey isn't null and is localized.
        page.Title = resourceKey;

    private Page LocatePageFor(object viewModel, string? contract)
    {
        var view = _viewLocator.ResolveView(viewModel, contract) ?? throw new InvalidOperationException(
                $"No view could be located for type '{viewModel.GetType().FullName}', contract '{contract}'. Be sure Splat has an appropriate registration.");
        if (view is not Page page)
        {
            throw new InvalidOperationException(
                $"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' is not a Page.");
        }

        view.ViewModel = viewModel;

        return page;
    }
}
