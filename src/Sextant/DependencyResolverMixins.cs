// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using ReactiveUI;
using Splat;

namespace Sextant;

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
    /// <returns>The dependencyResolver.</returns>
    public static IMutableDependencyResolver RegisterViewStackService(this IMutableDependencyResolver dependencyResolver)
    {
        if (dependencyResolver is null)
        {
            throw new ArgumentNullException(nameof(dependencyResolver));
        }

        dependencyResolver.RegisterLazySingleton<IViewStackService>(
            () => new ParameterViewStackService(
                Locator.Current.GetService<IView>(NavigationView) ?? throw new InvalidOperationException("IView not registered."),
                Locator.Current.GetService<IViewModelFactory>() ?? new DefaultViewModelFactory()));
        return dependencyResolver;
    }

    /// <summary>
    /// Registers the view stack service.
    /// </summary>
    /// <param name="dependencyResolver">The dependency resolver.</param>
    /// <returns>The dependencyResolver.</returns>
    public static IMutableDependencyResolver RegisterParameterViewStackService(this IMutableDependencyResolver dependencyResolver)
    {
        if (dependencyResolver is null)
        {
            throw new ArgumentNullException(nameof(dependencyResolver));
        }

        dependencyResolver.RegisterLazySingleton<IParameterViewStackService>(
            () => new ParameterViewStackService(
                Locator.Current.GetService<IView>(NavigationView) ?? throw new InvalidOperationException("IView not registered."),
                Locator.Current.GetService<IViewModelFactory>() ?? new DefaultViewModelFactory()));
        return dependencyResolver;
    }

    /// <summary>
    /// Registers the view stack service.
    /// </summary>
    /// <typeparam name="T">The view stack service type.</typeparam>
    /// <param name="dependencyResolver">The dependency resolver.</param>
    /// <param name="factory">The factory.</param>
    /// <returns>The dependencyResolver.</returns>
    [Obsolete("Use the Func<IView, IViewModelFactory, T> variant.")]
    public static IMutableDependencyResolver RegisterViewStackService<T>(this IMutableDependencyResolver dependencyResolver, Func<IView, T> factory)
        where T : IViewStackService
    {
        if (dependencyResolver is null)
        {
            throw new ArgumentNullException(nameof(dependencyResolver));
        }

        if (factory is null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        dependencyResolver.RegisterLazySingleton(() => factory(
            Locator.Current.GetService<IView>(NavigationView) ?? throw new InvalidOperationException("IView not registered.")));
        return dependencyResolver;
    }

    /// <summary>
    /// Registers the view stack service.
    /// </summary>
    /// <typeparam name="T">The view stack service type.</typeparam>
    /// <param name="dependencyResolver">The dependency resolver.</param>
    /// <param name="factory">The factory.</param>
    /// <returns>The dependencyResolver.</returns>
    public static IMutableDependencyResolver RegisterViewStackService<T>(this IMutableDependencyResolver dependencyResolver, Func<IView, IViewModelFactory, T> factory)
        where T : IViewStackService
    {
        if (dependencyResolver is null)
        {
            throw new ArgumentNullException(nameof(dependencyResolver));
        }

        if (factory is null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        dependencyResolver.RegisterLazySingleton(() => factory(
            Locator.Current.GetService<IView>(NavigationView) ?? throw new InvalidOperationException("IView not registered."),
            Locator.Current.GetService<IViewModelFactory>() ?? new DefaultViewModelFactory()));
        return dependencyResolver;
    }

    /// <summary>
    /// Registers the view model factory.
    /// </summary>
    /// <param name="dependencyResolver">The dependency resolver.</param>
    /// <returns>The dependencyResolver.</returns>
    public static IMutableDependencyResolver RegisterViewModelFactory(this IMutableDependencyResolver dependencyResolver)
    {
        if (dependencyResolver is null)
        {
            throw new ArgumentNullException(nameof(dependencyResolver));
        }

        dependencyResolver.RegisterLazySingleton<IViewModelFactory>(() => new DefaultViewModelFactory());
        return dependencyResolver;
    }

    /// <summary>
    /// Registers the view model factory.
    /// </summary>
    /// <param name="dependencyResolver">The dependency resolver.</param>
    /// <param name="factory">The factory.</param>
    /// <returns>The dependencyResolver.</returns>
    public static IMutableDependencyResolver RegisterViewModelFactory(this IMutableDependencyResolver dependencyResolver, Func<IViewModelFactory> factory)
    {
        if (dependencyResolver is null)
        {
            throw new ArgumentNullException(nameof(dependencyResolver));
        }

        if (factory is null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        dependencyResolver.RegisterLazySingleton(factory, typeof(IViewModelFactory));
        return dependencyResolver;
    }

    /// <summary>
    /// Registers the specified view with the Splat locator.
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <param name="dependencyResolver">The dependency resolver.</param>
    /// <param name="contract">The contract.</param>
    /// <returns>The dependency resolver to use.</returns>
    [Obsolete("Use RegisterViewForNavigation")]
    public static IMutableDependencyResolver RegisterView<TView, TViewModel>(this IMutableDependencyResolver dependencyResolver, string? contract = null)
        where TView : IViewFor<TViewModel>, new()
        where TViewModel : class, IViewModel
    {
        if (dependencyResolver is null)
        {
            throw new ArgumentNullException(nameof(dependencyResolver));
        }

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
    /// <returns>The dependencyResolver.</returns>
    [Obsolete("Use RegisterViewForNavigation")]
    public static IMutableDependencyResolver RegisterView<TView, TViewModel>(this IMutableDependencyResolver dependencyResolver, Func<TView> viewFactory, string? contract = null)
        where TView : class, IViewFor
        where TViewModel : class, IViewModel
    {
        if (dependencyResolver is null)
        {
            throw new ArgumentNullException(nameof(dependencyResolver));
        }

        if (viewFactory is null)
        {
            throw new ArgumentNullException(nameof(viewFactory));
        }

        dependencyResolver.Register(viewFactory, typeof(IViewFor<TViewModel>), contract);
        return dependencyResolver;
    }

    /// <summary>
    /// Registers the specified viewmodel with the Splat locator.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <param name="dependencyResolver">The dependency resolver.</param>
    /// <param name="contract">The contract.</param>
    /// <returns>The dependencyResolver.</returns>
    [Obsolete("Use of new makes this method undesirable.")]
    public static IMutableDependencyResolver RegisterViewModel<TViewModel>(this IMutableDependencyResolver dependencyResolver, string? contract = null)
        where TViewModel : IViewModel, new()
    {
        if (dependencyResolver is null)
        {
            throw new ArgumentNullException(nameof(dependencyResolver));
        }

        dependencyResolver.Register(() => new TViewModel(), typeof(TViewModel), contract);
        return dependencyResolver;
    }

    /// <summary>
    /// Registers the specified viewmodel with the Splat locator.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <param name="dependencyResolver">The dependency resolver.</param>
    /// <param name="viewModelFactory">The viewmodel factory.</param>
    /// <param name="contract">The contract.</param>
    /// <returns>The dependencyResolver.</returns>
    public static IMutableDependencyResolver RegisterViewModel<TViewModel>(this IMutableDependencyResolver dependencyResolver, Func<TViewModel> viewModelFactory, string? contract = null)
        where TViewModel : class, IViewModel
    {
        if (dependencyResolver is null)
        {
            throw new ArgumentNullException(nameof(dependencyResolver));
        }

        if (viewModelFactory is null)
        {
            throw new ArgumentNullException(nameof(viewModelFactory));
        }

        dependencyResolver.Register(viewModelFactory, typeof(TViewModel), contract);
        return dependencyResolver;
    }

    /// <summary>
    /// Registers the specified view model with the Splat locator.
    /// </summary>
    /// <param name="dependencyResolver">The dependency resolver.</param>
    /// <param name="viewModel">The view model.</param>
    /// <param name="contract">The contract.</param>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <returns>The dependencyResolver.</returns>
    public static IMutableDependencyResolver RegisterViewModel<TViewModel>(this IMutableDependencyResolver dependencyResolver, TViewModel viewModel, string? contract = null)
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

        dependencyResolver.Register(() => viewModel, typeof(TViewModel), contract);
        return dependencyResolver;
    }

    /// <summary>
    /// Registers the provided <see cref="IViewFor{T}" /> to the TViewModel for navigation.
    /// </summary>
    /// <typeparam name="TView">The view.</typeparam>
    /// <typeparam name="TViewModel">The view model.</typeparam>
    /// <param name="resolver">The resolver.</param>
    /// <param name="viewFactory">The view factory.</param>
    /// <param name="viewModelFactory">The view model factory.</param>
    /// <returns>
    /// The dependency resolver.
    /// </returns>
    public static IMutableDependencyResolver RegisterViewForNavigation<TView, TViewModel>(this IMutableDependencyResolver resolver, Func<TView> viewFactory, Func<TViewModel> viewModelFactory)
        where TView : class, IViewFor<TViewModel>
        where TViewModel : class, IViewModel
    {
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        resolver.Register(viewFactory, typeof(IViewFor<TViewModel>));
        resolver.Register(viewModelFactory, typeof(TViewModel));
        return resolver;
    }

    /// <summary>
    /// Registers the provided <see cref="IViewFor{T}"/> to the TViewModel for navigation.
    /// </summary>
    /// <param name="resolver">The resolver.</param>
    /// <param name="view">The view factory.</param>
    /// <param name="viewModel">The view model factory.</param>
    /// <typeparam name="TView">The view.</typeparam>
    /// <typeparam name="TViewModel">The view model.</typeparam>
    /// <returns>The dependency resolver.</returns>
    public static IMutableDependencyResolver RegisterViewForNavigation<TView, TViewModel>(this IMutableDependencyResolver resolver, TView view, TViewModel viewModel)
        where TView : class, IViewFor<TViewModel>
        where TViewModel : class, IViewModel
    {
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        resolver.Register(() => view, typeof(IViewFor<TViewModel>));
        resolver.Register(() => viewModel, typeof(TViewModel));
        return resolver;
    }

    /// <summary>
    /// Registers the view for navigation.
    /// </summary>
    /// <typeparam name="TView">The type of the view.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <param name="resolver">The resolver.</param>
    /// <returns>The dependency resolver.</returns>
    /// <exception cref="ArgumentNullException">resolver.</exception>
    public static IMutableDependencyResolver RegisterViewForNavigation<TView, TViewModel>(this IMutableDependencyResolver resolver)
        where TView : class, IViewFor<TViewModel>, new()
        where TViewModel : class, IViewModel, new()
    {
        if (resolver is null)
        {
            throw new ArgumentNullException(nameof(resolver));
        }

        resolver.Register(() => new TView(), typeof(IViewFor<TViewModel>));
        resolver.Register(() => new TViewModel(), typeof(TViewModel));
        return resolver;
    }
}
