﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Concurrency;
using System.Text;
using ReactiveUI;
using Sextant.Abstractions;
using Splat;

namespace Sextant
{
    /// <summary>
    /// Extension methods associated with the IMutableDependencyResolver interface.
    /// </summary>
#if WINDOWS_UWP
    public static partial class DependencyResolverMixins
#else
    public static class DependencyResolverMixins
#endif
    {
        /// <summary>
        /// Gets the navigation view key.
        /// </summary>
        [SuppressMessage("Design", "CA1721: Confusing name, should be method.", Justification = "Deliberate usage.")]
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
            if (dependencyResolver is null)
            {
                throw new ArgumentNullException(nameof(dependencyResolver));
            }

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
        /// <returns>The dependencyResolver.</returns>
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

            IView view = Locator.Current.GetService<IView>(NavigationView);
            dependencyResolver.RegisterLazySingleton(() => factory(view));
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

            dependencyResolver.RegisterLazySingleton(factory);
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
        public static IMutableDependencyResolver RegisterView<TView, TViewModel>(this IMutableDependencyResolver dependencyResolver, Func<IViewFor<TViewModel>> viewFactory, string? contract = null)
            where TView : IViewFor
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
    }
}
