// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Text;
using ReactiveUI;
using Splat;

namespace Sextant
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
        /// Registers the view stack service.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <returns>The dependencyResovler.</returns>
        public static IMutableDependencyResolver RegisterViewStackService(this IMutableDependencyResolver dependencyResolver)
        {
            IView view = Locator.Current.GetService<IView>(NavigationView);
            dependencyResolver.RegisterLazySingleton<IViewStackService>(() => new ParameterViewStackService(view));
            return dependencyResolver;
        }

        /// <summary>
        /// Registers the view stack service.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <returns>The dependencyResolver.</returns>
        public static IMutableDependencyResolver RegisterParameterViewStackService(this IMutableDependencyResolver dependencyResolver)
        {
            IView view = Locator.Current.GetService<IView>(NavigationView);
            dependencyResolver.RegisterLazySingleton<IParameterViewStackService>(() => new ParameterViewStackService(view));
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
            IView view = Locator.Current.GetService<IView>(NavigationView);
            dependencyResolver.RegisterLazySingleton(() => factory(view));
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
            where TViewModel : class, IViewModel
        {
            dependencyResolver.Register(() => new TView(), typeof(IViewFor<TViewModel>), contract);
            return dependencyResolver;
        }

        /// <summary>
        /// Registers the specified view with the Splat locator.
        /// </summary>
        /// <typeparam name="TView">The type of the view.</typeparam>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <param name="viewFactory">The view factory.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>
        /// The dependencyResovler.
        /// </returns>
        public static IMutableDependencyResolver RegisterView<TView, TViewModel>(this IMutableDependencyResolver dependencyResolver, Func<IViewFor<TViewModel>> viewFactory, string contract = null)
            where TView : IViewFor
            where TViewModel : class, IViewModel
        {
            dependencyResolver.Register(() => viewFactory(), typeof(IViewFor<TViewModel>), contract);
            return dependencyResolver;
        }
    }
}
