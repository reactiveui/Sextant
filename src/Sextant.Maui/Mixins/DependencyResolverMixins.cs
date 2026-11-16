// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Concurrency;
using System.Runtime.CompilerServices;

namespace Sextant.Maui;

/// <summary>Extension methods associated with the IMutableDependencyResolver interface.</summary>
public static class DependencyResolverMixins
{
    /// <summary>Gets the navigation view key.</summary>
    [SuppressMessage("Design", "CA1721: Confusing name, should be method.", Justification = "Deliberate usage.")]
    public static string NavigationView => nameof(NavigationView);

    /// <summary>Extension members for <c>IMutableDependencyResolver</c>.</summary>
    /// <param name="dependencyResolver">The resolver that receives navigation services.</param>
    extension(IMutableDependencyResolver dependencyResolver)
    {
        /// <summary>Initializes the sextant.</summary>
        /// <returns>The dependencyResolver.</returns>
        public IMutableDependencyResolver RegisterNavigationView()
        {
            dependencyResolver.RegisterLazySingleton(
                static () => new NavigationView(
                RxSchedulers.MainThreadScheduler,
                AppLocator.Current.GetService<IViewLocator>() ?? throw new InvalidOperationException("IViewLocator not registered.")),
                typeof(IView),
                NavigationView);
            return dependencyResolver;
        }

        /// <summary>Initializes sextant.</summary>
        /// <param name = "mainThreadScheduler">The main scheduler.</param>
        /// <returns>The dependencyResolver.</returns>
        public IMutableDependencyResolver RegisterNavigationView(IScheduler mainThreadScheduler)
        {
            dependencyResolver.RegisterLazySingleton(
                () => new NavigationView(
                mainThreadScheduler,
                AppLocator.Current.GetService<IViewLocator>() ?? throw new InvalidOperationException("IViewLocator not registered.")),
                typeof(IView),
                NavigationView);
            return dependencyResolver;
        }

        /// <summary>Registers a value for navigation.</summary>
        /// <typeparam name = "TView">The type of view to register.</typeparam>
        /// <param name = "navigationViewFactory">The navigation view factory.</param>
        /// <returns>The dependencyResolver.</returns>
        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Long term object.")]
        public IMutableDependencyResolver RegisterNavigationView<TView>(Func<TView> navigationViewFactory)
            where TView : IView
        {
            ArgumentNullException.ThrowIfNull(dependencyResolver);
            ArgumentNullException.ThrowIfNull(navigationViewFactory);
            var navigationView = navigationViewFactory();
            var viewStackService = new ViewStackService(navigationView);
            dependencyResolver.RegisterLazySingleton<IViewStackService>(() => viewStackService);
            dependencyResolver.RegisterLazySingleton<IView>(() => navigationView, NavigationView);
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
        public NavigationView? GetNavigationView() => dependencyResolver.GetNavigationView(null);

        /// <summary>Gets the navigation view.</summary>
        /// <param name = "contract">The contract.</param>
        /// <returns>The navigation view.</returns>
        public NavigationView? GetNavigationView(string? contract)
        {
            ArgumentNullException.ThrowIfNull(dependencyResolver);
            return dependencyResolver.GetService<IView>(contract ?? NavigationView) as NavigationView;
        }
    }
}
