﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using ReactiveUI;
using Splat;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Sextant
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class NavigationView : Page, IView, IEnableLogger
    {
        /// <summary>
        /// A depedendency property for the back button.
        /// </summary>
        public static readonly DependencyProperty IsBackButtonVisibleProperty =
            DependencyProperty.Register("IsBackButtonVisible", typeof(bool), typeof(NavigationView), new PropertyMetadata(true));

        private readonly IScheduler _backgroundScheduler;
        private readonly IScheduler _mainScheduler;
        private readonly IViewLocator _viewLocator;
        private IFullLogger _logger;

        private ContentDialog? _contentDialog;
        private IViewModel? _lastPoppedViewModel;
        private Stack<IViewModel?> _mirroredPageStack;
        private Stack<IViewModel?> _mirroredModalStack;

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
        public NavigationView(IScheduler mainScheduler, IScheduler backgroundScheduler, IViewLocator viewLocator)
        {
            InitializeComponent();

            _logger = this.Log();

            _backgroundScheduler = backgroundScheduler;
            _mainScheduler = mainScheduler;
            _viewLocator = viewLocator;

            _mirroredPageStack = new Stack<IViewModel?>();
            _mirroredModalStack = new Stack<IViewModel?>();

            backButton.Visibility = Visibility.Collapsed;

            PagePopped = Observable
                .FromEvent<NavigatedEventHandler, NavigationEventArgs>(
                    handler =>
                    {
                        return (sender, e) => handler(e);
                    },
                    x => mainFrame.Navigated += x,
                    x => mainFrame.Navigated -= x)
                .Do(args =>
                {
                    if (mainFrame.CanGoBack)
                    {
                        IsBackButtonVisible = true;
                    }
                    else
                    {
                        IsBackButtonVisible = false;
                    }
                })
                .Where(ep => ep.NavigationMode == NavigationMode.Back)
                .Select(ep =>
                {
                    var view = ep.Content as IViewFor;
                    if (view == null)
                    {
                        _logger.Debug($"The view ({ep.Content.GetType()}) does not implement IViewFor<>.  Cannot set ViewModel from a back navigation.");
                    }
                    else
                    {
                        view.ViewModel = _mirroredPageStack.Peek();
                    }

                    // Since view stack doesn't contain instances (only types), we have to store the latest viewmodel and return it on a back nav.
                    // ep.Content contains an instance of the new view, but it may have just been created and its ViewModel property will be null.
                    // But we want the view that was just removed.  We need to send the old view's viewmodel to IViewStackService so that the ViewModel can be removed from the stack.
                    return _lastPoppedViewModel;
                })
                .WhereNotNull();

            BackRequested.Subscribe();
        }

        /// <summary>
        /// Gets a value indicating whether a modal (ContentDialog) is visible.
        /// </summary>
        public static bool ModalVisible
        {
            get
            {
                var popups = VisualTreeHelper.GetOpenPopups(Window.Current);
                return popups.FirstOrDefault(x => x.Child is ContentDialog) != null;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the default back button is visible.
        /// </summary>
        public bool IsBackButtonVisible
        {
            get { return (bool)GetValue(IsBackButtonVisibleProperty); }
            set { SetValue(IsBackButtonVisibleProperty, value); }
        }

        /// <inheritdoc />
        public IScheduler MainThreadScheduler => _mainScheduler;

        /// <inheritdoc />
        public IObservable<IViewModel?> PagePopped { get; }

        /// <summary>
        /// Gets combined backrequested observable from system, backbutton, and xbox controller sources.
        /// </summary>
        public IObservable<Unit> BackRequested => Observable.Merge(
            Observable
                .FromEvent<EventHandler<BackRequestedEventArgs>, BackRequestedEventArgs>(
                    handler =>
                    {
                        return (_, e) => handler(e);
                    },
                    x => SystemNavigationManager.GetForCurrentView().BackRequested += x,
                    x => SystemNavigationManager.GetForCurrentView().BackRequested -= x)
                .Do(ev =>
                {
                    if (mainFrame.CanGoBack)
                    {
                        PopPage(true);
                        ev.Handled = true;
                    }
                })
                .ToSignal(),
            Observable
                .FromEvent<PointerEventHandler, PointerRoutedEventArgs>(
                handler => (_, e) => handler(e),
                x => PointerPressed += x,
                x => PointerPressed -= x)
                .Where(args => args.GetCurrentPoint(this as UIElement).Properties.PointerUpdateKind == Windows.UI.Input.PointerUpdateKind.XButton1Pressed)
                .Do(args =>
                {
                    if (mainFrame.CanGoBack)
                    {
                        PopPage(true);
                        args.Handled = true;
                    }
                })
                .ToSignal(),
            Observable
                .FromEvent<RoutedEventHandler, RoutedEventArgs>(
                handler =>
                {
                    void RoutedHandler(object sender, RoutedEventArgs e) => handler(e);
                    return RoutedHandler;
                },
                x => backButton.Click += x,
                x => backButton.Click -= x)
                .Do(args =>
                {
                    if (mainFrame.CanGoBack)
                    {
                        PopPage(true);
                    }
                })
                .ToSignal());

        /// <inheritdoc />
        public IObservable<Unit> PopModal()
        {
            if (_contentDialog != null)
            {
                _mirroredModalStack.TryPop(out _);
                _contentDialog.Hide();

                if (_mirroredModalStack.TryPeek(out var modal))
                {
                    if (modal == null)
                    {
                        return Observable.Return(Unit.Default).ObserveOn(_mainScheduler);
                    }

                    _contentDialog = new ContentDialog();
                    _contentDialog.FullSizeDesired = true;
                    _contentDialog.IsPrimaryButtonEnabled = false;
                    _contentDialog.IsSecondaryButtonEnabled = false;
                    _contentDialog.Content = LocatePageFor(modal, null);

                    _ = _contentDialog.ShowAsync();
                }
            }

            return Observable.Return(Unit.Default).ObserveOn(_mainScheduler);
        }

        /// <inheritdoc />
        public IObservable<Unit> PopPage(bool animate)
        {
            NavigationTransitionInfo animation;
            if (animate)
            {
                animation = new EntranceNavigationTransitionInfo();
            }
            else
            {
                animation = new SuppressNavigationTransitionInfo();
            }

            _mirroredPageStack.Pop();

            var view = mainFrame.Content as IViewFor;
            if (view == null)
            {
                _logger.Debug($"The view ({mainFrame.Content.GetType()}) does not implement IViewFor<>.  Cannot get ViewModel.");
                return Observable.Return(Unit.Default).ObserveOn(_mainScheduler);
            }

            _lastPoppedViewModel = view.ViewModel as IViewModel;

            mainFrame.GoBack(animation);

            return Observable.Return(Unit.Default).ObserveOn(_mainScheduler);
        }

        /// <inheritdoc />
        public IObservable<Unit> PopToRootPage(bool animate)
        {
            NavigationTransitionInfo animation;
            if (animate)
            {
                animation = new EntranceNavigationTransitionInfo();
            }
            else
            {
                animation = new SuppressNavigationTransitionInfo();
            }

            while (mainFrame.BackStackDepth > 1)
            {
                mainFrame.BackStack.Remove(mainFrame.BackStack.Last());
            }

            while (_mirroredPageStack.Count > 1)
            {
                _mirroredPageStack.Pop();
            }

            var view = mainFrame.Content as IViewFor;
            if (view == null)
            {
                _logger.Debug($"The view ({mainFrame.Content.GetType()}) does not implement IViewFor<>.  Cannot get ViewModel.");
                return Observable.Return(Unit.Default).ObserveOn(_mainScheduler);
            }

            _lastPoppedViewModel = view.ViewModel as IViewModel;

            mainFrame.GoBack(animation);

            return Observable.Return(Unit.Default).ObserveOn(_mainScheduler);
        }

        /// <inheritdoc />
        public IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract, bool withNavigationPage = true) =>
            Observable
                .Start(
                    () =>
                    {
                        // ignore withNavigationPage, not necessary for UWP.
                        return LocatePageFor(modalViewModel, contract);
                    },
                    CurrentThreadScheduler.Instance)
                .ObserveOn(CurrentThreadScheduler.Instance)
                .SelectMany(
                    page =>
                    {
                        _mirroredModalStack.Push(modalViewModel);

                        if (_contentDialog != null && ModalVisible)
                        {
                            _contentDialog.Hide();
                        }

                        _contentDialog = new ContentDialog();
                        _contentDialog.FullSizeDesired = true;
                        _contentDialog.IsPrimaryButtonEnabled = false;
                        _contentDialog.IsSecondaryButtonEnabled = false;
                        _contentDialog.Content = page;

                        _ = _contentDialog.ShowAsync();

                        return Observable.Return(Unit.Default);
                    });

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
                        // ignore withNavigationPage, not necessary for UWP.
                        return LocatePageTypeFor(viewModel, contract);
                    },
                    CurrentThreadScheduler.Instance)
                .ObserveOn(CurrentThreadScheduler.Instance)
                .SelectMany(
                    pageType =>
                    {
                        if (resetStack)
                        {
                            _mirroredPageStack.Clear();
                            _mirroredPageStack.Push(viewModel);

                            mainFrame.Navigate(pageType, null, new SuppressNavigationTransitionInfo());
                            if (mainFrame.Content is IViewFor)
                            {
                                ((IViewFor)mainFrame.Content).ViewModel = viewModel;
                            }
                            else
                            {
                               _logger.Debug($"The view ({mainFrame.Content.GetType()}) does not implement IViewFor<>.  Cannot set ViewModel of type, {viewModel.GetType()}, on view.");
                            }

                            mainFrame.BackStack.Clear();

                            return Observable.Return(Unit.Default);
                        }

                        NavigationTransitionInfo animation;
                        if (animate)
                        {
                            animation = new EntranceNavigationTransitionInfo();
                        }
                        else
                        {
                            animation = new SuppressNavigationTransitionInfo();
                        }

                        _mirroredPageStack.Push(viewModel);

                        mainFrame.Navigate(pageType, null, animation);
                        if (mainFrame.Content is IViewFor)
                        {
                            ((IViewFor)mainFrame.Content).ViewModel = viewModel;
                        }
                        else
                        {
                            _logger.Debug($"The view ({mainFrame.Content.GetType()}) does not implement IViewFor<>.  Cannot set ViewModel of type, {viewModel.GetType()}, on view.");
                        }

                        return Observable.Return(Unit.Default);
                    });

        private IViewModel? CurrentViewModel() => (IViewModel?)(mainFrame.Content as IViewFor)?.ViewModel;

        private IView LocateNavigationFor(IViewModel viewModel)
        {
            var view = _viewLocator.ResolveView(viewModel, "NavigationView");
            var navigationPage = view as IView;

            if (navigationPage is null)
            {
                _logger.Debug($"No navigation view could be located for type '{viewModel.GetType().FullName}', using the default navigation page.");
                navigationPage = Locator.Current.GetService<IView>(nameof(NavigationView)) ?? Locator.Current.GetService<IView>();
            }

            return navigationPage;
        }

        private Type LocatePageTypeFor(object viewModel, string? contract)
        {
            var uwpViewTypeResolver = Locator.Current.GetService<ViewTypeResolver>(contract);

            var viewType = uwpViewTypeResolver.ResolveViewType(viewModel.GetType());

            if (viewType == null)
            {
                throw new InvalidOperationException($"No view could be located for type '{viewModel.GetType().FullName}', contract '{contract}'. Be sure Splat has an appropriate registration.");
            }

            return viewType;
        }

        private Page LocatePageFor(object viewModel, string? contract)
        {
            var view = _viewLocator.ResolveView(viewModel, contract);
            var page = view as Page;

            if (view == null)
            {
                throw new InvalidOperationException($"No view could be located for type '{viewModel.GetType().FullName}', contract '{contract}'. Be sure Splat has an appropriate registration.");
            }

            if (view == null)
            {
                throw new InvalidOperationException($"Cannot find view for '{viewModel.GetType().FullName}', contract '{contract}' does not implement IViewFor.");
            }

            if (page == null)
            {
                throw new InvalidOperationException($"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' is not a Page.");
            }

            view.ViewModel = viewModel;

            return page;
        }
    }
}
