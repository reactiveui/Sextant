// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;
using ReactiveUI;
using Splat;

namespace Sextant.XamForms
{
    /// <summary>
    /// A set of helper methods associated with the Sextant UI.
    /// </summary>
    public static class SextantHelper
    {
        /// <summary>
        /// Registers the specified view with the Splat locator.
        /// </summary>
        /// <typeparam name="TView">The type of view to register.</typeparam>
        /// <typeparam name="TViewModel">The type of view model to register.</typeparam>
        /// <param name="contract">An optional contract which will only provide a value if this contract is passed.</param>
        [Obsolete("Use the dependency resolver mixins.")]
        public static void RegisterView<TView, TViewModel>(string? contract = null)
            where TView : IViewFor, new()
            where TViewModel : class, IViewModel
        {
            Locator.CurrentMutable.Register(() => new TView(), typeof(IViewFor<TViewModel>), contract);
        }

        /// <summary>
        /// Registers a value for navigation.
        /// </summary>
        /// <typeparam name="TView">The type of view to register.</typeparam>
        /// <typeparam name="TViewModel">The type of view model to register.</typeparam>
        /// <param name="mainThreadScheduler">The scheduler which schedules tasks on the main UI thread.</param>
        /// <param name="backgroundScheduler">The scheduler which schedules tasks in the background.</param>
        /// <param name="viewLocator">A view locator which is responsible for finding a view for a view model pair.</param>
        [Obsolete("Use the dependency resolver mixins.")]
        public static void RegisterNavigation<TView, TViewModel>(IScheduler? mainThreadScheduler = null, IScheduler? backgroundScheduler = null, IViewLocator? viewLocator = null)
            where TView : IViewFor
            where TViewModel : class, IViewModel
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
        /// Initializes the main UI view navigation system and navigates to the specified view model view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of view model to register.</typeparam>
        /// <param name="mainThreadScheduler">The scheduler which schedules tasks on the main UI thread.</param>
        /// <param name="backgroundScheduler">The scheduler which schedules tasks in the background.</param>
        /// <param name="viewLocator">A view locator which is responsible for finding a view for a view model pair.</param>
        /// <returns>The navigation view.</returns>
        [Obsolete("Use the dependency resolver mixins.")]
        public static NavigationView Initialize<TViewModel>(IScheduler? mainThreadScheduler = null, IScheduler? backgroundScheduler = null, IViewLocator? viewLocator = null)
            where TViewModel : class, IViewModel
        {
            var bgScheduler = mainThreadScheduler ?? RxApp.TaskpoolScheduler;
            var mScheduler = backgroundScheduler ?? RxApp.MainThreadScheduler;
            var vLocator = viewLocator ?? Locator.Current.GetService<IViewLocator>();

            var navigationView = new NavigationView(mScheduler, bgScheduler, vLocator);
            var viewStackService = new ViewStackService(navigationView);

            Locator.CurrentMutable.Register<IViewStackService>(() => viewStackService);
            var instance = Activator.CreateInstance(typeof(TViewModel), viewStackService) as TViewModel;

            if (instance is null)
            {
                throw new InvalidOperationException($"Could not initialize a view for view model {typeof(TViewModel)}");
            }

            navigationView.PushPage(instance, null, true, false).Subscribe();

            return navigationView;
        }

        /// <summary>
        /// Initializes the main UI view navigation system and navigates to the specified view model view.
        /// </summary>
        /// <typeparam name="TViewModel">The type of view model to register.</typeparam>
        /// <param name="mainThreadScheduler">The scheduler which schedules tasks on the main UI thread.</param>
        /// <param name="backgroundScheduler">The scheduler which schedules tasks in the background.</param>
        /// <param name="viewLocator">A view locator which is responsible for finding a view for a view model pair.</param>
        /// <returns>The navigation view.</returns>
        [Obsolete("Use the " + nameof(Initialize) + " method.")]
        public static NavigationView Initialise<TViewModel>(IScheduler? mainThreadScheduler = null, IScheduler? backgroundScheduler = null, IViewLocator? viewLocator = null)
            where TViewModel : class, IViewModel
        {
            return Initialize<TViewModel>(mainThreadScheduler, backgroundScheduler, viewLocator);
        }
    }
}
