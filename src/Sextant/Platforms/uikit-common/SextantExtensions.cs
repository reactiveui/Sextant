// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Text;
using Splat;

namespace Sextant
{
    /// <summary>
    /// Extensions methods to setup the <see cref="Sextant"/> instance.
    /// </summary>
    public static class SextantExtensions
    {
        /// <summary>
        /// Initializes the specified sextant.
        /// </summary>
        /// <param name="sextant">The sextant.</param>
        public static void Initialize(this Sextant sextant)
        {
            if (sextant is null)
            {
                throw new ArgumentNullException(nameof(sextant));
            }

            sextant.MutableLocator.RegisterNavigationController().RegisterViewStackService();
        }

        /// <summary>
        /// Registers the navigation controller.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <returns>The mutable dependency resolver.</returns>
        public static IMutableDependencyResolver RegisterNavigationController(
            this IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterLazySingleton(() => new NavigationViewController());
            return dependencyResolver;
        }

        /// <summary>
        /// Registers the navigation controller.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <param name="mainScheduler">The main scheduler.</param>
        /// <param name="backgroundScheduler">The background scheduler.</param>
        /// <returns>
        /// The mutable dependency resolver.
        /// </returns>
        public static IMutableDependencyResolver RegisterNavigationController(
            this IMutableDependencyResolver dependencyResolver,
            IScheduler mainScheduler,
            IScheduler backgroundScheduler)
        {
            dependencyResolver.RegisterLazySingleton(() => new NavigationViewController(mainScheduler, backgroundScheduler));
            return dependencyResolver;
        }

        /// <summary>
        /// Registers the navigation controller.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>The mutable dependency resolver.</returns>
        public static IMutableDependencyResolver RegisterNavigationController(
            this IMutableDependencyResolver dependencyResolver,
            Func<NavigationViewController> factory)
        {
            dependencyResolver.RegisterLazySingleton(() => factory);
            return dependencyResolver;
        }
    }
}
