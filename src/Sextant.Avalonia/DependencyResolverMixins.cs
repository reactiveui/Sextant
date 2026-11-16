// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Runtime.CompilerServices;
using Splat;

namespace Sextant.Avalonia;

/// <summary>Extension methods associated with the IMutableDependencyResolver interface.</summary>
public static class DependencyResolverMixins
{
    /// <summary>Extension members for <c>IMutableDependencyResolver</c>.</summary>
    /// <param name="dependencyResolver">The resolver that receives navigation services.</param>
    extension(IMutableDependencyResolver dependencyResolver)
    {
        /// <summary>Registers a navigation view into the container.</summary>
        /// <typeparam name = "TView">The view type.</typeparam>
        /// <param name = "navigationViewFactory">The navigation view factory.</param>
        /// <returns>The dependency resolver for builder use.</returns>
        public IMutableDependencyResolver RegisterNavigationView<TView>(Func<TView> navigationViewFactory)
            where TView : IView
        {
            ArgumentNullException.ThrowIfNull(navigationViewFactory);
            var navigationView = navigationViewFactory();
            var viewStackService = new ViewStackService(navigationView);
            dependencyResolver.RegisterLazySingleton<IViewStackService>(() => viewStackService);
            dependencyResolver.RegisterLazySingleton<IView>(() => navigationView, "NavigationView");
            return dependencyResolver;
        }
    }

    /// <summary>Extension members for <c>IReadonlyDependencyResolver</c>.</summary>
    /// <param name="dependencyResolver">The resolver that owns the navigation view.</param>
    extension(IReadonlyDependencyResolver dependencyResolver)
    {

        /// <summary>Returns the registered navigation view.</summary>
        /// <returns>A sequence that signals when the navigation operation completes.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IView GetNavigationView() => dependencyResolver.GetNavigationView(null);

        /// <summary>Resolves navigation view from a dependency resolver.</summary>
        /// <param name = "contract">Optional contract.</param>
        /// <returns>The dependency resolver for builder use.</returns>
        /// <exception cref="InvalidOperationException">The navigation view has no registration.</exception>
        public IView GetNavigationView(string? contract)
        {
            var view = dependencyResolver.GetService<IView>(contract ?? "NavigationView");
            return view switch
            {
                null => throw new InvalidOperationException("NavigationView not registered."),
                _ => view
            };
        }
    }
}
