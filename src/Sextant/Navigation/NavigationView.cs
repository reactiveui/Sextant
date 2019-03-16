// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using ReactiveUI;
using Sextant.Abstraction;
using Xamarin.Forms;

namespace Sextant
{
    /// <summary>
    /// The view frame that displays the current page.
    /// </summary>
    /// <seealso cref="NavigationPage" />
    /// <seealso cref="IView" />
    public class NavigationView : NavigationPage, IView
    {
        private readonly IScheduler _backgroundScheduler;
        private readonly IScheduler _mainScheduler;
        private readonly IObservable<IPageViewModel> _pagePopped;
        private readonly IViewLocator _viewLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationView"/> class.
        /// </summary>
        /// <param name="mainScheduler">The main scheduler.</param>
        /// <param name="backgroundScheduler">The background scheduler.</param>
        /// <param name="viewLocator">The view locator.</param>
        /// <param name="rootPage">The root page.</param>
        public NavigationView(IScheduler mainScheduler, IScheduler backgroundScheduler, IViewLocator viewLocator, Page rootPage)
            : base(rootPage)
        {
            _backgroundScheduler = backgroundScheduler;
            _mainScheduler = mainScheduler;
            _viewLocator = viewLocator;

            _pagePopped = Observable
                .FromEventPattern<NavigationEventArgs>(x => Popped += x, x => Popped -= x)
                .Select(ep => ep.EventArgs.Page.BindingContext as IPageViewModel)
                .WhereNotNull();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationView"/> class.
        /// </summary>
        /// <param name="mainScheduler">The main scheduler.</param>
        /// <param name="backgroundScheduler">The background scheduler.</param>
        /// <param name="viewLocator">The view locator.</param>
        public NavigationView(IScheduler mainScheduler, IScheduler backgroundScheduler, IViewLocator viewLocator)
        {
            _backgroundScheduler = backgroundScheduler;
            _mainScheduler = mainScheduler;
            _viewLocator = viewLocator;

            _pagePopped = Observable
                .FromEventPattern<NavigationEventArgs>(x => Popped += x, x => Popped -= x)
                .Select(ep => ep.EventArgs.Page.BindingContext as IPageViewModel)
                .WhereNotNull();
        }

        /// <inheritdoc />
        public IObservable<IPageViewModel> PagePopped => _pagePopped;

        /// <inheritdoc />
        public IObservable<Unit> PopModal() =>
            Navigation
                .PopModalAsync()
                .ToObservable()
                .ToSignal()
                .ObserveOn(_mainScheduler); // XF completes the pop operation on a background thread :/

        /// <inheritdoc />
        public IObservable<Unit> PopPage(bool animate) =>
            Navigation
                .PopAsync(animate)
                .ToObservable()
                .ToSignal()
                .ObserveOn(_mainScheduler); // XF completes the pop operation on a background thread :/

        /// <inheritdoc />
        public IObservable<Unit> PopToRootPage(bool animate) =>
             Navigation
                .PopToRootAsync(animate)
                .ToObservable()
                .ToSignal()
                .ObserveOn(_mainScheduler);

        /// <inheritdoc />
        public IObservable<Unit> PushModal(IPageViewModel modalViewModel, string contract)
        {
            return Observable
                .Start(
                    () =>
                    {
                        var page = LocatePageFor(modalViewModel, contract);
                        SetPageTitle(page, modalViewModel.Id);

                        var navigation = LocateNavigationFor(modalViewModel);
                        navigation.PushPage(modalViewModel, contract, true, false).Subscribe();

                        return navigation as NavigationPage;
                    },
                    CurrentThreadScheduler.Instance)
                .ObserveOn(CurrentThreadScheduler.Instance)
                .SelectMany(
                    page =>
                        Navigation
                            .PushModalAsync(page)
                            .ToObservable());
        }

        /// <inheritdoc />
        public IObservable<Unit> PushPage(IPageViewModel pageViewModel, string contract, bool resetStack, bool animate)
        {
            return Observable
                .Start(
                    () =>
                    {
                        var page = LocatePageFor(pageViewModel, contract);
                        SetPageTitle(page, pageViewModel.Id);
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
                                return Navigation
                                    .PushAsync(page, false)
                                    .ToObservable();
                            }

                            // XF does not allow us to pop to a new root page. Instead, we need to inject the new root page and then pop to it.
                            Navigation
                                .InsertPageBefore(page, Navigation.NavigationStack[0]);

                            return Navigation
                                .PopToRootAsync(false)
                                .ToObservable();
                        }

                        return Navigation
                            .PushAsync(page, animate)
                            .ToObservable();
                    });
        }

        private IView LocateNavigationFor(IPageViewModel viewModel)
        {
            var view = _viewLocator.ResolveView(viewModel, "NavigationView");
            var navigationPage = view as IView;

            if (navigationPage is null)
            {
                Debug.WriteLine($"No navigation view could be located for type '{viewModel.GetType().FullName}', using the default navigation page.");
                navigationPage = new NavigationView(_mainScheduler, _backgroundScheduler, _viewLocator);
            }

            return navigationPage;
        }

        private Page LocatePageFor(object viewModel, string contract)
        {
            var view = _viewLocator.ResolveView(viewModel, contract);
            var viewFor = view as IViewFor;
            var page = view as Page;

            if (view == null)
            {
                throw new InvalidOperationException($"No view could be located for type '{viewModel.GetType().FullName}', contract '{contract}'. Be sure Splat has an appropriate registration.");
            }

            if (viewFor == null)
            {
                throw new InvalidOperationException($"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' does not implement IViewFor.");
            }

            if (page == null)
            {
                throw new InvalidOperationException($"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' is not a Page.");
            }

            viewFor.ViewModel = viewModel;

            return page;
        }

        private void SetPageTitle(Page page, string resourceKey)
        {
            // var title = Localize.GetString(resourceKey);
            // TODO: ensure resourceKey isn't null and is localized.
            page.Title = resourceKey;
        }
    }
}
