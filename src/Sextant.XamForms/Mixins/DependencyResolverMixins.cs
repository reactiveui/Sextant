// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Text;
using ReactiveUI;
using Sextant.XamForms;
using Splat;

namespace Sextant.XamForms
{
    /// <summary>
    /// Extension methods associated with the IMutableDependencyResolver interface.
    /// </summary>
    public static class DependencyResolverMixins
    {
        /// <summary>
        /// Gets the navigation view key.
        /// </summary>
        public static string NavigationView => nameof(NavigationView);

        /// <summary>
        /// Initializes the sextant.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        public static void InitializeSextant(this IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterNavigationView().RegisterViewStackService();
        }

        /// <summary>
        /// Initializes the sextant.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <returns>The dependencyResovler.</returns>
        public static IMutableDependencyResolver RegisterNavigationView(this IMutableDependencyResolver dependencyResolver)
        {
            var vLocator = Locator.Current.GetService<IViewLocator>();

            dependencyResolver.RegisterLazySingleton(() => new NavigationView(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, vLocator), typeof(IView), NavigationView);
            return dependencyResolver;
        }

        /// <summary>
        /// Initializes sextant.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <param name="mainScheudler">The main scheudler.</param>
        /// <param name="backgroundScheduler">The background scheduler.</param>
        /// <returns>The dependencyResovler.</returns>
        public static IMutableDependencyResolver RegisterNavigationView(this IMutableDependencyResolver dependencyResolver, IScheduler mainScheudler, IScheduler backgroundScheduler)
        {
            var vLocator = Locator.Current.GetService<IViewLocator>();

            dependencyResolver.RegisterLazySingleton(() => new NavigationView(mainScheudler, backgroundScheduler, vLocator), typeof(IView), NavigationView);
            return dependencyResolver;
        }

        /// <summary>
        /// Registers the view stack service.
        /// </summary>
        /// <typeparam name="T">The view stack service type.</typeparam>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <returns>The dependencyResovler.</returns>
        public static IMutableDependencyResolver RegisterViewStackService(this IMutableDependencyResolver dependencyResolver)
        {
            var view = Locator.Current.GetService<IView>(NavigationView);
            dependencyResolver.Register<IViewStackService>(() => new ViewStackService(view));
            return dependencyResolver;
        }

        /// <summary>
        /// Registers the view stack service.
        /// </summary>
        /// <typeparam name="T">The view stack service type.</typeparam>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>The dependencyResovler.</returns>
        public static IMutableDependencyResolver RegisterViewStackService<T>(this IMutableDependencyResolver dependencyResolver, Func<IView, T> factory)
            where T : IViewStackService
        {
            var view = Locator.Current.GetService<IView>(NavigationView);
            dependencyResolver.RegisterLazySingleton<T>(() => factory(view));
            return dependencyResolver;
        }

        /// <summary>
        /// Registers the specified view with the Splat locator.
        /// </summary>
        /// <typeparam name="TView">The type of the view.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>The dependencyResovler.</returns>
        public static IMutableDependencyResolver RegisterView<TView, TViewModel>(this IMutableDependencyResolver dependencyResolver, string contract = null)
            where TView : IViewFor<TViewModel>, new()
            where TViewModel : class, IPageViewModel
        {
            dependencyResolver.Register(() => new TView(), typeof(IViewFor<TViewModel>), contract);
            return dependencyResolver;
        }
    }
}
