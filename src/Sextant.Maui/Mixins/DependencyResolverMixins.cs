// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Reactive.Concurrency;

namespace Sextant.Maui;

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
    public static IMutableDependencyResolver RegisterNavigationView(this IMutableDependencyResolver dependencyResolver)
    {
        dependencyResolver.RegisterLazySingleton(
            () => new NavigationView(
                RxApp.MainThreadScheduler,
                RxApp.TaskpoolScheduler,
                Locator.Current.GetService<IViewLocator>() ?? throw new InvalidOperationException("IViewLocator not registered.")),
            typeof(IView),
            NavigationView);
        return dependencyResolver;
    }

    /// <summary>
    /// Initializes sextant.
    /// </summary>
    /// <param name="dependencyResolver">The dependency resolver.</param>
    /// <param name="mainThreadScheduler">The main scheduler.</param>
    /// <param name="backgroundScheduler">The background scheduler.</param>
    /// <returns>The dependencyResolver.</returns>
    public static IMutableDependencyResolver RegisterNavigationView(this IMutableDependencyResolver dependencyResolver, IScheduler mainThreadScheduler, IScheduler backgroundScheduler)
    {
        dependencyResolver.RegisterLazySingleton(
            () => new NavigationView(
                mainThreadScheduler,
                backgroundScheduler,
                Locator.Current.GetService<IViewLocator>() ?? throw new InvalidOperationException("IViewLocator not registered.")),
            typeof(IView),
            NavigationView);
        return dependencyResolver;
    }

    /// <summary>
    /// Registers a value for navigation.
    /// </summary>
    /// <typeparam name="TView">The type of view to register.</typeparam>
    /// <param name="dependencyResolver">The dependency resolver.</param>
    /// <param name="navigationViewFactory">The navigation view factory.</param>
    /// <returns>The dependencyResolver.</returns>
    [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Long term object.")]
    public static IMutableDependencyResolver RegisterNavigationView<TView>(this IMutableDependencyResolver dependencyResolver, Func<TView> navigationViewFactory)
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

        var navigationView = navigationViewFactory();
        var viewStackService = new ViewStackService(navigationView);

        dependencyResolver.RegisterLazySingleton<IViewStackService>(() => viewStackService);
        dependencyResolver.RegisterLazySingleton<IView>(() => navigationView, NavigationView);
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
        string? contract = null)
    {
        if (dependencyResolver is null)
        {
            throw new ArgumentNullException(nameof(dependencyResolver));
        }

        return dependencyResolver.GetService<IView>(contract ?? NavigationView) as NavigationView;
    }
}
