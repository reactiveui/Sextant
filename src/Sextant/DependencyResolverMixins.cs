// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Runtime.CompilerServices;
using Splat;

namespace Sextant;

/// <summary>Extension methods associated with the IMutableDependencyResolver interface.</summary>
public static class DependencyResolverMixins
{
    /// <summary>The error reported when the navigation view has no registration.</summary>
    private const string MissingNavigationView = "IView not registered.";

    /// <summary>Gets the navigation view key.</summary>
    public static string NavigationView => nameof(NavigationView);

    /// <summary>Extension members for <c>IMutableDependencyResolver</c>.</summary>
    /// <param name="dependencyResolver">The resolver that receives navigation registrations.</param>
    extension(IMutableDependencyResolver dependencyResolver)
    {
        /// <summary>Registers the view stack service.</summary>
        /// <returns>The dependencyResolver.</returns>
        public IMutableDependencyResolver RegisterViewStackService()
        {
            ArgumentNullException.ThrowIfNull(dependencyResolver);
            dependencyResolver.RegisterLazySingleton<IViewStackService>(static () => new ParameterViewStackService(
                AppLocator.Current.GetService<IView>(NavigationView) ?? throw new InvalidOperationException(MissingNavigationView),
                AppLocator.Current.GetService<IViewModelFactory>() ?? new DefaultViewModelFactory()));
            return dependencyResolver;
        }

        /// <summary>Registers the view stack service.</summary>
        /// <returns>The dependencyResolver.</returns>
        public IMutableDependencyResolver RegisterParameterViewStackService()
        {
            ArgumentNullException.ThrowIfNull(dependencyResolver);
            dependencyResolver.RegisterLazySingleton<IParameterViewStackService>(static () => new ParameterViewStackService(
                AppLocator.Current.GetService<IView>(NavigationView) ?? throw new InvalidOperationException(MissingNavigationView),
                AppLocator.Current.GetService<IViewModelFactory>() ?? new DefaultViewModelFactory()));
            return dependencyResolver;
        }

        /// <summary>Registers the view stack service.</summary>
        /// <typeparam name = "T">The view stack service type.</typeparam>
        /// <param name = "factory">The factory.</param>
        /// <returns>The dependencyResolver.</returns>
        public IMutableDependencyResolver RegisterViewStackService<T>(Func<IView, IViewModelFactory, T> factory)
            where T : class, IViewStackService
        {
            ArgumentNullException.ThrowIfNull(dependencyResolver);
            ArgumentNullException.ThrowIfNull(factory);
            dependencyResolver.RegisterLazySingleton(() => factory(
                AppLocator.Current.GetService<IView>(NavigationView) ?? throw new InvalidOperationException(MissingNavigationView),
                AppLocator.Current.GetService<IViewModelFactory>() ?? new DefaultViewModelFactory()));
            return dependencyResolver;
        }

        /// <summary>Registers the view model factory.</summary>
        /// <returns>The dependencyResolver.</returns>
        public IMutableDependencyResolver RegisterViewModelFactory()
        {
            ArgumentNullException.ThrowIfNull(dependencyResolver);
            dependencyResolver.RegisterLazySingleton<IViewModelFactory>(static () => new DefaultViewModelFactory());
            return dependencyResolver;
        }

        /// <summary>Registers the view model factory.</summary>
        /// <param name = "factory">The factory.</param>
        /// <returns>The dependencyResolver.</returns>
        public IMutableDependencyResolver RegisterViewModelFactory(Func<IViewModelFactory> factory)
        {
            ArgumentNullException.ThrowIfNull(dependencyResolver);
            ArgumentNullException.ThrowIfNull(factory);
            dependencyResolver.RegisterLazySingleton(factory, typeof(IViewModelFactory));
            return dependencyResolver;
        }

        /// <summary>Registers a view model instance or factory with the dependency resolver.</summary>
        /// <typeparam name="TViewModel">The view model or parameter value type.</typeparam>
        /// <param name="viewModelFactory">The factory invoked to create a view model.</param>
        /// <returns>The resolver for further registrations.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IMutableDependencyResolver RegisterViewModel<TViewModel>(Func<TViewModel> viewModelFactory)
                    where TViewModel : class, IViewModel => dependencyResolver.RegisterViewModel(viewModelFactory, null);

        /// <summary>Registers the specified viewmodel with the Splat locator.</summary>
        /// <typeparam name = "TViewModel">The type of the view model.</typeparam>
        /// <param name = "viewModelFactory">The viewmodel factory.</param>
        /// <param name = "contract">The contract.</param>
        /// <returns>The dependencyResolver.</returns>
        public IMutableDependencyResolver RegisterViewModel<TViewModel>(Func<TViewModel> viewModelFactory, string? contract)
            where TViewModel : class, IViewModel
        {
            ArgumentNullException.ThrowIfNull(dependencyResolver);
            ArgumentNullException.ThrowIfNull(viewModelFactory);
            dependencyResolver.Register(viewModelFactory, contract);
            return dependencyResolver;
        }

        /// <summary>Registers a view model instance or factory with the dependency resolver.</summary>
        /// <typeparam name="TViewModel">The view model or parameter value type.</typeparam>
        /// <param name="viewModel">The view model associated with this navigation operation.</param>
        /// <returns>The resolver for further registrations.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IMutableDependencyResolver RegisterViewModel<TViewModel>(TViewModel viewModel)
                    where TViewModel : class, IViewModel => dependencyResolver.RegisterViewModel(viewModel, null);

        /// <summary>Registers the specified view model with the Splat locator.</summary>
        /// <typeparam name = "TViewModel">The type of the view model.</typeparam>
        /// <param name = "viewModel">The view model.</param>
        /// <param name = "contract">The contract.</param>
        /// <returns>The dependencyResolver.</returns>
        public IMutableDependencyResolver RegisterViewModel<TViewModel>(TViewModel viewModel, string? contract)
            where TViewModel : class, IViewModel
        {
            ArgumentNullException.ThrowIfNull(dependencyResolver);
            ArgumentNullException.ThrowIfNull(viewModel);
            dependencyResolver.Register(() => viewModel, typeof(TViewModel), contract);
            return dependencyResolver;
        }

        /// <summary>Registers the provided <see cref = "IViewFor{T}"/> to the TViewModel for navigation.</summary>
        /// <typeparam name = "TView">The view.</typeparam>
        /// <typeparam name = "TViewModel">The view model.</typeparam>
        /// <param name = "viewFactory">The view factory.</param>
        /// <param name = "viewModelFactory">The view model factory.</param>
        /// <returns>
        /// The dependency resolver.
        /// </returns>
        public IMutableDependencyResolver RegisterViewForNavigation<TView, TViewModel>(Func<TView> viewFactory, Func<TViewModel> viewModelFactory)
            where TView : class, IViewFor<TViewModel>
            where TViewModel : class, IViewModel
        {
            ArgumentNullException.ThrowIfNull(dependencyResolver);
            ArgumentNullException.ThrowIfNull(viewFactory);
            ArgumentNullException.ThrowIfNull(viewModelFactory);
            dependencyResolver.Register<IViewFor<TViewModel>>(viewFactory);
            dependencyResolver.Register(viewModelFactory);
            return dependencyResolver;
        }

        /// <summary>Registers the provided <see cref = "IViewFor{T}"/> to the TViewModel for navigation.</summary>
        /// <typeparam name = "TView">The view.</typeparam>
        /// <typeparam name = "TViewModel">The view model.</typeparam>
        /// <param name = "view">The view factory.</param>
        /// <param name = "viewModel">The view model factory.</param>
        /// <returns>The dependency resolver.</returns>
        public IMutableDependencyResolver RegisterViewForNavigation<TView, TViewModel>(TView view, TViewModel viewModel)
            where TView : class, IViewFor<TViewModel>
            where TViewModel : class, IViewModel
        {
            ArgumentNullException.ThrowIfNull(dependencyResolver);
            ArgumentNullException.ThrowIfNull(view);
            ArgumentNullException.ThrowIfNull(viewModel);
            dependencyResolver.Register(() => view, typeof(IViewFor<TViewModel>));
            dependencyResolver.Register(() => viewModel, typeof(TViewModel));
            return dependencyResolver;
        }
    }
}
