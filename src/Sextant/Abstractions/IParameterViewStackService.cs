// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Reactive;

namespace Sextant;

/// <summary>Interface that defines methods for passing parameters on navigation.</summary>
public interface IParameterViewStackService : IViewStackService
{
    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="navigableViewModel">The view model associated with this navigation operation.</param>
    /// <param name="parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <param name="resetStack">True to replace the current page stack.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(INavigable navigableViewModel, INavigationParameter parameter, string? contract, bool resetStack) => PushPage(navigableViewModel, parameter, contract, resetStack, true);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="navigableViewModel">The view model associated with this navigation operation.</param>
    /// <param name="parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(INavigable navigableViewModel, INavigationParameter parameter, string? contract) => PushPage(navigableViewModel, parameter, contract, false, true);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="navigableViewModel">The view model associated with this navigation operation.</param>
    /// <param name="parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(INavigable navigableViewModel, INavigationParameter parameter) => PushPage(navigableViewModel, parameter, null, false, true);

    /// <summary>Pushes the <see cref = "INavigable"/> onto the stack.</summary>
    /// <param name = "navigableViewModel">The navigable view model.</param>
    /// <param name = "parameter">The parameter.</param>
    /// <param name = "contract">The contract.</param>
    /// <param name = "resetStack">if set to <c>true</c> [reset stack].</param>
    /// <param name = "animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PushPage(INavigable navigableViewModel, INavigationParameter parameter, string? contract, bool resetStack, bool animate);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <param name="resetStack">True to replace the current page stack.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(Type viewModelType, INavigationParameter parameter, string? contract, bool resetStack) => PushPage(viewModelType, parameter, contract, resetStack, true);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(Type viewModelType, INavigationParameter parameter, string? contract) => PushPage(viewModelType, parameter, contract, false, true);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(Type viewModelType, INavigationParameter parameter) => PushPage(viewModelType, parameter, null, false, true);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <param name="resetStack">True to replace the current page stack.</param>
    /// <param name="animate">True to animate the navigation transition.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(Type viewModelType, INavigationParameter parameter, string? contract, bool resetStack, bool animate);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <param name = "resetStack">True to replace the page stack.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushPage<TViewModel>(INavigationParameter parameter, string? contract, bool resetStack)
        where TViewModel : INavigable => PushPage<TViewModel>(parameter, contract, resetStack, true);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushPage<TViewModel>(INavigationParameter parameter, string? contract)
        where TViewModel : INavigable => PushPage<TViewModel>(parameter, contract, false, true);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushPage<TViewModel>(INavigationParameter parameter)
        where TViewModel : INavigable => PushPage<TViewModel>(parameter, null, false, true);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <param name = "resetStack">True to replace the page stack.</param>
    /// <param name = "animate">True to animate the navigation transition.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushPage<TViewModel>(INavigationParameter parameter, string? contract, bool resetStack, bool animate)
        where TViewModel : INavigable => PushPage(typeof(TViewModel), parameter, contract, resetStack, animate);

    /// <summary>Presents a modal, resolving its view from the registered contract.</summary>
    /// <param name="navigableModal">The view model associated with this navigation operation.</param>
    /// <param name="parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushModal(INavigable navigableModal, INavigationParameter parameter, string? contract) => PushModal(navigableModal, parameter, contract, true);

    /// <summary>Presents a modal, resolving its view from the registered contract.</summary>
    /// <param name="navigableModal">The view model associated with this navigation operation.</param>
    /// <param name="parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushModal(INavigable navigableModal, INavigationParameter parameter) => PushModal(navigableModal, parameter, null, true);

    /// <summary>Pushes the <see cref = "IViewModel"/> onto the stack.</summary>
    /// <param name = "navigableModal">The modal.</param>
    /// <param name = "parameter">The parameter.</param>
    /// <param name = "contract">The contract.</param>
    /// <param name = "withNavigationPage">Value indicating whether to wrap the modal in a navigation page.</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PushModal(INavigable navigableModal, INavigationParameter parameter, string? contract, bool withNavigationPage);

    /// <summary>Presents a modal, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushModal(Type viewModelType, INavigationParameter parameter, string? contract) => PushModal(viewModelType, parameter, contract, true);

    /// <summary>Presents a modal, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushModal(Type viewModelType, INavigationParameter parameter) => PushModal(viewModelType, parameter, null, true);

    /// <summary>Presents a modal, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <param name="withNavigationPage">True to wrap the modal in a navigation page.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushModal(Type viewModelType, INavigationParameter parameter, string? contract, bool withNavigationPage);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushModal<TViewModel>(INavigationParameter parameter, string? contract)
        where TViewModel : INavigable => PushModal<TViewModel>(parameter, contract, true);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushModal<TViewModel>(INavigationParameter parameter)
        where TViewModel : INavigable => PushModal<TViewModel>(parameter, null, true);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <param name = "withNavigationPage">True to wrap the modal in a navigation page.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushModal<TViewModel>(INavigationParameter parameter, string? contract, bool withNavigationPage)
        where TViewModel : INavigable => PushModal(typeof(TViewModel), parameter, contract, withNavigationPage);

    /// <summary>Removes the top page and sends navigation lifecycle notifications.</summary>
    /// <param name="parameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PopPage(INavigationParameter parameter) => PopPage(parameter, true);

    /// <summary>Pops the <see cref = "IViewModel"/> off of the stack.</summary>
    /// <param name = "parameter">The parameter.</param>
    /// <param name = "animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PopPage(INavigationParameter parameter, bool animate);
}
