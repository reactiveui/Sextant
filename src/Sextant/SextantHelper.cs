// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;
using ReactiveUI;
using Sextant.Abstraction;
using Splat;
using Xamarin.Forms;

namespace Sextant
{
    /// <summary>
    /// Static helper to register dependencies against the splat container.
    /// </summary>
    public static class SextantHelper
    {
        /// <summary>
        /// Registers the view.
        /// </summary>
        /// <typeparam name="TView">The type of the view.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="contract">The contract.</param>
        public static void RegisterView<TView, TViewModel>(string contract = null)
            where TView : IViewFor, new()
            where TViewModel : class, IPageViewModel
        {
            Locator.CurrentMutable.Register(() => new TView(), typeof(IViewFor<TViewModel>), contract);
        }

        /// <summary>
        /// Registers the navigation.
        /// </summary>
        /// <typeparam name="TView">The type of the view.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="mainThreadScheduler">The main thread scheduler.</param>
        /// <param name="backgroundScheduler">The background scheduler.</param>
        /// <param name="viewLocator">The view locator.</param>
        public static void RegisterNavigation<TView, TViewModel>(IScheduler mainThreadScheduler = null, IScheduler backgroundScheduler = null, IViewLocator viewLocator = null)
            where TView : IViewFor
            where TViewModel : class, IPageViewModel
        {
            var bgScheduler = mainThreadScheduler ?? RxApp.TaskpoolScheduler;
            var mScheduler = backgroundScheduler ?? RxApp.MainThreadScheduler;
            var vLocator = viewLocator ?? Locator.Current.GetService<IViewLocator>();

            Locator.CurrentMutable.Register(
                () => Activator.CreateInstance(typeof(TView), mScheduler, bgScheduler, vLocator),
                typeof(IViewFor<TViewModel>),
                "NavigationView");
        }

        /// <summary>
        /// Initialises the specified main thread scheduler.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="mainThreadScheduler">The main thread scheduler.</param>
        /// <param name="backgroundScheduler">The background scheduler.</param>
        /// <param name="viewLocator">The view locator.</param>
        /// <returns>The navigation view.</returns>
        public static NavigationView Initialise<TViewModel>(IScheduler mainThreadScheduler = null, IScheduler backgroundScheduler = null, IViewLocator viewLocator = null)
            where TViewModel : class, IPageViewModel
        {
            var bgScheduler = mainThreadScheduler ?? RxApp.TaskpoolScheduler;
            var mScheduler = backgroundScheduler ?? RxApp.MainThreadScheduler;
            var vLocator = viewLocator ?? Locator.Current.GetService<IViewLocator>();

            var navigationView = new NavigationView(mScheduler, bgScheduler, vLocator);
            var viewStackService = new ViewStackService(navigationView);

            Locator.CurrentMutable.Register<IViewStackService>(() => viewStackService);
            navigationView.PushPage(Activator.CreateInstance(typeof(TViewModel), viewStackService) as TViewModel, null, true, false).Subscribe();

            return navigationView;
        }
    }
}
