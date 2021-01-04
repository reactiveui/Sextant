// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Splat;

namespace Sextant.Avalonia
{
    /// <summary>
    /// Extension methods associated with the IMutableDependencyResolver interface.
    /// </summary>
    public static class DependencyResolverMixins
    {
        /// <summary>
        /// Registers a navigation view into the container.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <param name="navigationViewFactory">The navigation view factory.</param>
        /// <typeparam name="TView">The view type.</typeparam>
        /// <returns>The dependency resolver.</returns>
        public static IMutableDependencyResolver RegisterNavigationView<TView>(
            this IMutableDependencyResolver dependencyResolver,
            Func<TView> navigationViewFactory)
            where TView : IView
        {
            var navigationView = navigationViewFactory();
            var viewStackService = new ViewStackService(navigationView);
            dependencyResolver.RegisterLazySingleton<IViewStackService>(() => viewStackService);
            dependencyResolver.RegisterLazySingleton<IView>(() => navigationView, "NavigationView");
            return dependencyResolver;
        }

        /// <summary>
        /// Resolves navigation view from a dependency resolver.
        /// </summary>
        /// <param name="dependencyResolver"></param>
        /// <param name="contract"></param>
        /// <returns>The dependency resolver.</returns>
        public static IView GetNavigationView(
            this IReadonlyDependencyResolver dependencyResolver,
            string contract = null) =>
            dependencyResolver.GetService<IView>(contract ?? "NavigationView");
    }
}
