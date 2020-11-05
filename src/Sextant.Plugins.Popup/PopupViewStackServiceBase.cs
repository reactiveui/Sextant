// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Events;
using Rg.Plugins.Popup.Pages;

namespace Sextant.Plugins.Popup
{
    /// <summary>
    /// Represents a popup view stack service implementation.
    /// </summary>
    public abstract class PopupViewStackServiceBase : ParameterViewStackServiceBase, IPopupViewStackService
    {
        private readonly IPopupNavigation _popupNavigation;
        private readonly IViewLocator _viewLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupViewStackServiceBase"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="popupNavigation">The popup navigation.</param>
        /// <param name="viewLocator">The view locator.</param>
        /// <param name="viewModelFactory">The view model factory.</param>
        protected PopupViewStackServiceBase(IView view, IPopupNavigation popupNavigation, IViewLocator viewLocator, IViewModelFactory viewModelFactory)
            : base(view, viewModelFactory)
        {
            _popupNavigation = popupNavigation;
            _viewLocator = viewLocator;
            PopupSubject = new BehaviorSubject<IImmutableList<IViewModel>>(ImmutableList<IViewModel>.Empty);

            Pushing = Observable.FromEvent<EventHandler<PopupNavigationEventArgs>, PopupNavigationEventArgs>(
                eventHandler =>
                {
                    void Handler(object sender, PopupNavigationEventArgs args) => eventHandler(args);

                    return Handler;
                },
                x => _popupNavigation.Pushing += x,
                x => _popupNavigation.Pushing -= x)
                .Select(x => new PopupNavigationEvent((IViewFor)x.Page, x.IsAnimated));

            Pushed = Observable.FromEvent<EventHandler<PopupNavigationEventArgs>, PopupNavigationEventArgs>(
                eventHandler =>
                {
                    void Handler(object sender, PopupNavigationEventArgs args)
                        => eventHandler(args);

                    return Handler;
                },
                x => _popupNavigation.Pushed += x,
                x => _popupNavigation.Pushed -= x)
                .Select(x => new PopupNavigationEvent((IViewFor)x.Page, x.IsAnimated));

            Popping = Observable.FromEvent<EventHandler<PopupNavigationEventArgs>, PopupNavigationEventArgs>(
                eventHandler =>
                {
                    void Handler(object sender, PopupNavigationEventArgs args)
                        => eventHandler(args);

                    return Handler;
                },
                x => _popupNavigation.Popping += x,
                x => _popupNavigation.Popping -= x)
                .Select(x => new PopupNavigationEvent((IViewFor)x.Page, x.IsAnimated));

            Popped = Observable.FromEvent<EventHandler<PopupNavigationEventArgs>, PopupNavigationEventArgs>(
                eventHandler =>
                {
                    void Handler(object sender, PopupNavigationEventArgs args) => eventHandler(args);

                    return Handler;
                },
                x => _popupNavigation.Popped += x,
                x => _popupNavigation.Popped -= x)
                .Select(x => new PopupNavigationEvent((IViewFor)x.Page, x.IsAnimated));

            Popped
                .Subscribe(popped => popped.ViewModel.InvokeViewModelAction<IDestructible>(x => x.Destroy()))
                .DisposeWith(NavigationDisposables);

            PopupSubject.DisposeWith(NavigationDisposables);
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
        public IReadOnlyList<IViewModel> PopupStack =>
#pragma warning disable 8619
            _popupNavigation
                .PopupStack
                .Cast<IViewFor<IViewModel>>()
                .Where(x => x != null)
                .Select(x => x.ViewModel)
                .ToList();
#pragma warning restore 8619

        /// <summary>
        /// Gets the popup subject that contains the stack history.
        /// </summary>
        protected BehaviorSubject<IImmutableList<IViewModel>> PopupSubject { get; }

        /// <inheritdoc/>
        public IObservable<Unit> PushPopup(IViewModel viewModel, string? contract = null, bool animate = true)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            PopupPage popupPage = LocatePopupFor(viewModel, contract);

            return Observable
                .FromAsync(() => _popupNavigation.PushAsync(popupPage, animate))
                .Do(_ =>
                {
                    AddToStackAndTick(PopupSubject, viewModel, false);
                    Logger.Debug($"Added page '{viewModel.Id}' (contract '{contract}') to stack.");
                });
        }

        /// <inheritdoc/>
        public IObservable<Unit> PushPopup<TViewModel>(string? contract = null, bool animate = true)
            where TViewModel : IViewModel
        {
            var viewModel = Factory.Create<TViewModel>();
            return PushPopup(viewModel, contract, animate);
        }

        /// <inheritdoc/>
        public IObservable<Unit> PushPopup(
            INavigable viewModel,
            INavigationParameter navigationParameter,
            string? contract = null,
            bool animate = true)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            if (navigationParameter == null)
            {
                throw new ArgumentNullException(nameof(navigationParameter));
            }

            return Observable
                .Start(() => LocatePopupFor(viewModel, contract), CurrentThreadScheduler.Instance)
                .ObserveOn(CurrentThreadScheduler.Instance)
                .Do(popup =>
                    popup.ViewModel.InvokeViewModelAction<INavigating>(x => x.WhenNavigatingTo(navigationParameter)))
                .Select(popup =>
                    Observable
                        .FromAsync(() => _popupNavigation.PushAsync(popup, animate))
                        .Do(_ => popup.ViewModel.InvokeViewModelAction<INavigated>(x =>
                            x.WhenNavigatedTo(navigationParameter))))
                .Switch()
                .Do(_ =>
                {
                    AddToStackAndTick(PopupSubject, viewModel, false);
                    Logger.Debug($"Added page '{viewModel.Id}' (contract '{contract}') to stack.");
                });
        }

        /// <inheritdoc/>
        public IObservable<Unit> PushPopup<TViewModel>(
            INavigationParameter navigationParameter,
            string? contract = null,
            bool animate = true)
            where TViewModel : INavigable
        {
            var viewModel = Factory.Create<TViewModel>(contract);

            return PushPopup(viewModel, navigationParameter, contract, animate);
        }

        /// <inheritdoc/>
        public IObservable<Unit> PopPopup(bool animate = true) =>
            Observable.FromAsync(() => _popupNavigation.PopAsync(animate));

        /// <inheritdoc/>
        public IObservable<Unit> PopAllPopups(bool animate = true) =>
            Observable
                .FromAsync(() => _popupNavigation.PopAllAsync(animate))
                .Do(_ => PopRootAndTick(PopupSubject, NavigationDisposables));

        /// <inheritdoc/>
        public IObservable<Unit> RemovePopup(IViewModel viewModel, string? contract = null, bool animate = true)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            PopupPage popupPage = LocatePopupFor(viewModel, contract);

            return Observable.FromAsync(() => _popupNavigation.RemovePageAsync(popupPage, animate))
                             .Do(_ => RemoveFromStackAndTick(PopupSubject, viewModel));
        }

        private static void RemoveFromStackAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject, T item)
        {
            if (stackSubject == null)
            {
                throw new ArgumentNullException(nameof(stackSubject));
            }

            var stack = stackSubject.Value;

            stack = stack.Remove(item);

            stackSubject.OnNext(stack);
        }

        private SextantPopupPage LocatePopupFor(IViewModel viewModel, string? contract)
        {
            IViewFor? view = _viewLocator.ResolveView(viewModel, contract);
            if (view == null)
            {
                throw new InvalidOperationException($"No view could be located for type '{viewModel.GetType().FullName}', contract '{contract}'. Be sure Splat has an appropriate registration.");
            }

            if (!(view is SextantPopupPage page))
            {
                throw new InvalidOperationException($"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' is not a {nameof(SextantPopupPage)}.");
            }

            page.ViewModel = viewModel;

            return page;
        }
    }
}
