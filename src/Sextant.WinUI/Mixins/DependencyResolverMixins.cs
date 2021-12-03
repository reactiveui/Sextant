// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Concurrency;
using ReactiveUI;
using Splat;

namespace Sextant.WinUI
{
    /// <summary>
    /// Extension methods associated with the IMutableDependencyResolver interface.
    /// </summary>
    public static class DependencyResolverMixins
    {
        /// <summary>
        /// Gets the navigation view key.
        /// </summary>
        [SuppressMessage("Design", "CA1721: Confusing name, should be method.", Justification = "Deliberate usage.")]
        public static string NavigationView => nameof(NavigationView);

        /// <summary>
        /// Initializes the sextant.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <returns>The dependencyResolver.</returns>
        public static IMutableDependencyResolver RegisterNavigationView(
            this IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterLazySingleton(
                () => new NavigationView(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, ViewLocator.Current), typeof(IView), DependencyResolverMixins.NavigationView);
            return dependencyResolver;
        }

        /// <summary>
        /// Initializes sextant.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <param name="mainThreadScheduler">The main scheduler.</param>
        /// <param name="backgroundScheduler">The background scheduler.</param>
        /// <returns>The dependencyResolver.</returns>
        public static IMutableDependencyResolver RegisterNavigationView(
            this IMutableDependencyResolver dependencyResolver, IScheduler mainThreadScheduler, IScheduler backgroundScheduler)
        {
            dependencyResolver.RegisterLazySingleton(
                () => new NavigationView(mainThreadScheduler, backgroundScheduler, ViewLocator.Current), typeof(IView), NavigationView);
            return dependencyResolver;
        }

        /// <summary>
        /// Registers a value for navigation.
        /// </summary>
        /// <typeparam name="TView">The type of view to register.</typeparam>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <param name="navigationViewFactory">The navigation view factory.</param>
        /// <returns>The dependencyResolver.</returns>
        [SuppressMessage("Design", "CA2000: Dispose ViewStackService", Justification = "Long lived object.")]
        public static IMutableDependencyResolver RegisterNavigationView<TView>(
            this IMutableDependencyResolver dependencyResolver, Func<TView> navigationViewFactory)
            where TView : IView
        {
            if (dependencyResolver is null)
            {
                throw new ArgumentNullException(nameof(dependencyResolver));
            }

            if (navigationViewFactory is null)
            {
                throw new ArgumentNullException(nameof(navigationViewFactory));
            }

            Lazy<(TView, ViewStackService)> lazy = new Lazy<(TView, ViewStackService)>(() =>
            {
                var navigationView = navigationViewFactory();
                var viewStackService = new ViewStackService(navigationView);

                return (navigationView, viewStackService);
            });

            dependencyResolver.RegisterLazySingleton<IViewStackService>(() => lazy.Value.Item2);
            dependencyResolver.RegisterLazySingleton<IView>(() => lazy.Value.Item1, NavigationView);
            return dependencyResolver;
        }

        /// <summary>
        /// Gets the navigation view.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>The navigation view.</returns>
        public static NavigationView? GetNavigationView(
            this IReadonlyDependencyResolver dependencyResolver,
            string? contract = null) =>
            dependencyResolver.GetService<IView>(contract ?? NavigationView) as NavigationView;

        /// <summary>
        /// Initializes UWP-specific view locator.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <returns>The dependencyResolver.</returns>
        public static IMutableDependencyResolver RegisterWinUIViewLocator(
            this IMutableDependencyResolver dependencyResolver)
        {
            if (dependencyResolver is null)
            {
                throw new ArgumentNullException(nameof(dependencyResolver));
            }

            dependencyResolver.RegisterConstant(new ViewTypeResolver(), typeof(ViewTypeResolver));
            return dependencyResolver;
        }

        /// <summary>
        /// Register view for viewmodel, but only return view type for UWP frame.
        /// </summary>
        /// <typeparam name="TView">The view type.</typeparam>
        /// <typeparam name="TViewModel">The viewmodel type.</typeparam>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>
        /// The dependencyResolver.
        /// </returns>
        public static IMutableDependencyResolver RegisterViewWinUI<TView, TViewModel>(
            this IMutableDependencyResolver dependencyResolver, string? contract = null)
            where TView : IViewFor<TViewModel>
            where TViewModel : class, IViewModel
        {
            if (dependencyResolver is null)
            {
                throw new ArgumentNullException(nameof(dependencyResolver));
            }

            var uwpViewTypeResolver = Locator.Current.GetService<ViewTypeResolver>();
            if (uwpViewTypeResolver is null)
            {
                throw new InvalidOperationException("UWP view type resolver not registered.");
            }

            uwpViewTypeResolver.Register<TView, TViewModel>();
            dependencyResolver.Register(() => Locator.Current.GetService<TView>()!, typeof(IViewFor<TViewModel>), contract);
            return dependencyResolver;
        }

        /// <summary>
        /// Helper method to get view type for viewmodel.
        /// </summary>
        /// <typeparam name="TViewModel">The viewmodel Type.</typeparam>
        /// <param name="dependencyResolver">The dependencyResolver.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>The view Type again.</returns>
        public static Type? ResolveView<TViewModel>(this IReadonlyDependencyResolver dependencyResolver, string? contract = null)
            where TViewModel : class
        {
            if (dependencyResolver is null)
            {
                throw new ArgumentNullException(nameof(dependencyResolver));
            }

            var uwpViewTypeResolver = dependencyResolver.GetService<ViewTypeResolver>(contract);
            return uwpViewTypeResolver?.ResolveViewType<TViewModel>();
        }

        /// <summary>
        /// Helper method to get view type for viewmodel.
        /// </summary>
        /// <typeparam name="TViewModel">The viewmodel Type.</typeparam>
        /// <param name="dependencyResolver">The dependencyResolver.</param>
        /// <param name="viewModel">The viewmodel.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>The view Type again.</returns>
        public static Type? ResolveView<TViewModel>(this IReadonlyDependencyResolver dependencyResolver, TViewModel viewModel, string? contract = null)
            where TViewModel : class, IViewModel
        {
            if (dependencyResolver is null)
            {
                throw new ArgumentNullException(nameof(dependencyResolver));
            }

            if (viewModel is null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            var uwpViewTypeResolver = dependencyResolver.GetService<ViewTypeResolver>(contract);
            return uwpViewTypeResolver?.ResolveViewType<TViewModel>();
        }
    }
}
