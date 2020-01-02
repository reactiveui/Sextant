// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
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

namespace Sextant.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class NavigationView : Page, IView, IEnableLogger
    {
        private readonly IScheduler _backgroundScheduler;
        private readonly IScheduler _mainScheduler;
        private readonly IViewLocator _viewLocator;
        private IFullLogger _logger;

        private IViewModel _currentViewModel;
        private ContentDialog _contentDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationView"/> class.
        /// </summary>
        /// <param name="mainScheduler">The main scheduler to scheduler UI tasks on.</param>
        /// <param name="backgroundScheduler">The background scheduler.</param>
        /// <param name="viewLocator">The view locator which will find views associated with view models.</param>
        /// <param name="rootPage">The starting root page.</param>
        public NavigationView(IScheduler mainScheduler, IScheduler backgroundScheduler, IViewLocator viewLocator, Page rootPage)
        {
            InitializeComponent();

            _backgroundScheduler = backgroundScheduler;
            _mainScheduler = mainScheduler;
            _viewLocator = viewLocator;
            _logger = this.Log();

            PagePopped = Observable
                .FromEvent<NavigatedEventHandler, NavigationEventArgs>(x => mainFrame.Navigated += x, x => mainFrame.Navigated -= x)
                .Do(args =>
                {
                    if (mainFrame.CanGoBack)
                    {
                        backButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        backButton.Visibility = Visibility.Collapsed;
                    }
                })
                .Where(ep => ep.NavigationMode == NavigationMode.Back)
                .Select(ep => (ep.Content as IViewFor).ViewModel as IViewModel)
                .WhereNotNull();

            // Observable.FromEvent<NavigatedEventHandler, NavigationEventArgs>(x => mainFrame.Navigated += x, x => mainFrame.Navigated -= x)
            //    .Subscribe(args =>
            //    {
            //        if (mainFrame.CanGoBack)
            //        {
            //            backButton.Visibility = Visibility.Visible;
            //        }
            //        else
            //        {
            //            backButton.Visibility = Visibility.Collapsed;
            //        }
            //    });
            var rootPag = rootPage;

            throw new NotImplementedException("Need to figure out how to add an instanced page to Frame");
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

            _backgroundScheduler = backgroundScheduler;
            _mainScheduler = mainScheduler;
            _viewLocator = viewLocator;

            backButton.Visibility = Visibility.Collapsed;

            PagePopped = Observable
                .FromEvent<NavigatedEventHandler, NavigationEventArgs>(
                    handler =>
                    {
                        NavigatedEventHandler navHandler = (sender, e) => handler(e);
                        return navHandler;
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
                    // Since view stack doesn't contain instances (only types), we have to store the latest viewmodel and return it on a back nav.
                    return _currentViewModel;
                })
                .WhereNotNull();

            // Observable.FromEvent<NavigatedEventHandler, NavigationEventArgs>(
            //       handler =>
            //       {
            //           NavigatedEventHandler navHandler = (sender, e) => handler(e);
            //           return navHandler;
            //       },
            //       x => mainFrame.Navigated += x,
            //       x => mainFrame.Navigated -= x)
            //   .Subscribe(args =>
            //   {
            //       if (mainFrame.CanGoBack)
            //       {
            //           IsBackButtonVisible = true;
            //       }
            //       else
            //       {
            //           IsBackButtonVisible = false;
            //       }
            //   });
            BackRequested.Subscribe();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the default back button is visible.
        /// </summary>
        public bool IsBackButtonVisible
        {
            get { return (bool)GetValue(IsBackButtonVisibleProperty); }
            set { SetValue(IsBackButtonVisibleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsBackButtonVisible.  This enables animation, styling, binding, etc...
#pragma warning disable SA1201 // Elements should appear in the correct order
#pragma warning disable SA1600 // Elements should be documented
        public static readonly DependencyProperty IsBackButtonVisibleProperty =
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore SA1201 // Elements should appear in the correct order
            DependencyProperty.Register("IsBackButtonVisible", typeof(bool), typeof(NavigationView), new PropertyMetadata(true));

        /// <summary>
        /// Gets a value indicating whether a modal (ContentDialog) is visible.
        /// </summary>
        public bool ModalVisible
        {
            get
            {
                var popups = VisualTreeHelper.GetOpenPopups(Window.Current);
                return popups.FirstOrDefault(x => x.Child is ContentDialog) != null;
            }
        }

        /// <inheritdoc />
        public IScheduler MainThreadScheduler => _mainScheduler;

        /// <inheritdoc />
        public IObservable<IViewModel> PagePopped { get; }

        /// <summary>
        /// Gets combined backrequested observable from system, backbutton, and xbox controller sources.
        /// </summary>
        public IObservable<Unit> BackRequested => Observable.Merge(
            Observable
                .FromEventPattern<BackRequestedEventArgs>(
                x => SystemNavigationManager.GetForCurrentView().BackRequested += x,
                x => SystemNavigationManager.GetForCurrentView().BackRequested -= x)
                .Do(ev =>
                {
                    if (mainFrame.CanGoBack)
                    {
                        PopPage(true);
                        ev.EventArgs.Handled = true;
                    }
                })
                .ToSignal(),
            Observable
                .FromEvent<PointerEventHandler, PointerRoutedEventArgs>(
                handler =>
                {
                    PointerEventHandler pointerHandler = (sender, e) => handler(e);
                    return pointerHandler;
                },
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
                    RoutedEventHandler routedHandler = (sender, e) => handler(e);
                    return routedHandler;
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
                _contentDialog.Hide();
                var viewStackService = Locator.Current.GetService<IViewStackService>();

                viewStackService.ModalStack.Skip(1).Take(1).Subscribe(stack =>
                {
                    if (stack.Count > 0)
                    {
                        _contentDialog = new ContentDialog();
                        _contentDialog.FullSizeDesired = true;
                        _contentDialog.IsPrimaryButtonEnabled = false;
                        _contentDialog.IsSecondaryButtonEnabled = false;
                        var page = LocatePageFor(stack[stack.Count - 1], null);

                        _contentDialog.Content = page;

                        _ = _contentDialog.ShowAsync();
                    }
                });
            }

            return Observable.Return(Unit.Default).ObserveOn(_mainScheduler);
        }

        /// <inheritdoc />
        public IObservable<Unit> PopPage(bool animate)
        {
            NavigationTransitionInfo animation;
            if (animate)
            {
                animation = new Windows.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo();
            }
            else
            {
                animation = new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo();
            }

            _currentViewModel = (IViewModel)(mainFrame.Content as IViewFor).ViewModel;

            mainFrame.GoBack(animation);

            return Observable.Return(Unit.Default).ObserveOn(_mainScheduler);
        }

        /// <inheritdoc />
        public IObservable<Unit> PopToRootPage(bool animate)
        {
            NavigationTransitionInfo animation;
            if (animate)
            {
                animation = new Windows.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo();
            }
            else
            {
                animation = new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo();
            }

            while (mainFrame.BackStackDepth > 1)
            {
                mainFrame.BackStack.Remove(mainFrame.BackStack.Last());
            }

            mainFrame.GoBack(animation);

            return Observable.Return(Unit.Default).ObserveOn(_mainScheduler);
        }

        /// <inheritdoc />
        public IObservable<Unit> PushModal(IViewModel modalViewModel, string contract, bool withNavigationPage = true) =>
            Observable
                .Start(
                    () =>
                    {
                        // ignore withNavigationPage, not necessary for UWP.
                        var page = LocatePageFor(modalViewModel, contract);

                        return page;
                    },
                    CurrentThreadScheduler.Instance)
                .ObserveOn(CurrentThreadScheduler.Instance)
                .SelectMany(
                    page =>
                    {
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
            string contract,
            bool resetStack,
            bool animate) =>
            Observable
                .Start(
                    () =>
                    {
                        // ignore withNavigationPage, not necessary for UWP.
                        var pageType = LocatePageTypeFor(viewModel, contract);

                        return pageType;
                    },
                    CurrentThreadScheduler.Instance)
                .ObserveOn(CurrentThreadScheduler.Instance)
                .SelectMany(
                    pageType =>
                    {
                        if (resetStack)
                        {
                            if (mainFrame.BackStackDepth == 0)
                            {
                                mainFrame.Navigate(pageType, null, new SuppressNavigationTransitionInfo());
                                (mainFrame.Content as IViewFor).ViewModel = viewModel;
                                return Observable.Return(Unit.Default);
                            }

                            mainFrame.Navigate(pageType, null, new SuppressNavigationTransitionInfo());
                            (mainFrame.Content as IViewFor).ViewModel = viewModel;
                            mainFrame.BackStack.Clear();

                            return Observable.Return(Unit.Default);
                        }

                        NavigationTransitionInfo animation;
                        if (animate)
                        {
                            animation = new Windows.UI.Xaml.Media.Animation.EntranceNavigationTransitionInfo();
                        }
                        else
                        {
                            animation = new Windows.UI.Xaml.Media.Animation.SuppressNavigationTransitionInfo();
                        }

                        mainFrame.Navigate(pageType, null, animation);
                        (mainFrame.Content as IViewFor).ViewModel = viewModel;
                        return Observable.Return(Unit.Default);
                    });

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

        private Type LocatePageTypeFor(object viewModel, string contract)
        {
            var uwpViewTypeResolver = Locator.Current.GetService<ViewTypeResolver>(contract);

            var viewType = uwpViewTypeResolver.ResolveViewType(viewModel.GetType());

            if (viewType == null)
            {
                throw new InvalidOperationException($"No view could be located for type '{viewModel.GetType().FullName}', contract '{contract}'. Be sure Splat has an appropriate registration.");
            }

            return viewType;
        }

        private Page LocatePageFor(object viewModel, string contract)
        {
            var view = _viewLocator.ResolveView(viewModel, contract);
            var page = view as Page;

            if (view == null)
            {
                throw new InvalidOperationException($"No view could be located for type '{viewModel.GetType().FullName}', contract '{contract}'. Be sure Splat has an appropriate registration.");
            }

            if (view == null)
            {
                throw new InvalidOperationException($"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' does not implement IViewFor.");
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
