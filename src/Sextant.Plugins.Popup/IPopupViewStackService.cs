// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Reactive;
using Mopups.Interfaces;

namespace Sextant.Plugins.Popup;

/// <summary>Interface representing a Sextant decorator for <see cref = "IPopupNavigation"/>.</summary>
public interface IPopupViewStackService : IParameterViewStackService
{
    /// <summary>Gets an observable sequence of pushing events.</summary>
    IObservable<PopupNavigationEvent> Pushing { get; }

    /// <summary>Gets an observable sequence of pushed events.</summary>
    IObservable<PopupNavigationEvent> Pushed { get; }

    /// <summary>Gets an observable sequence of popping events.</summary>
    IObservable<PopupNavigationEvent> Popping { get; }

    /// <summary>Gets an observable sequence of popped events.</summary>
    IObservable<PopupNavigationEvent> Popped { get; }

    /// <summary>Gets the popup stack.</summary>
    IReadOnlyList<IViewModel> PopupStack { get; }

    /// <summary>Presents a popup, resolving its view from the registered contract.</summary>
    /// <param name="viewModel">The view model associated with this navigation operation.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPopup(IViewModel viewModel, string? contract) => PushPopup(viewModel, contract, true);

    /// <summary>Presents a popup, resolving its view from the registered contract.</summary>
    /// <param name="viewModel">The view model associated with this navigation operation.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPopup(IViewModel viewModel) => PushPopup(viewModel, null, true);

    /// <summary>Push a pop up page to the stack.</summary>
    /// <param name = "viewModel">The view model.</param>
    /// <param name = "contract">The contract.</param>
    /// <param name = "animate">Animate the page.</param>
    /// <returns>A completion notification.</returns>
    IObservable<Unit> PushPopup(IViewModel viewModel, string? contract, bool animate);

    /// <summary>Presents a popup, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPopup(Type viewModelType, string? contract) => PushPopup(viewModelType, contract, true);

    /// <summary>Presents a popup, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPopup(Type viewModelType) => PushPopup(viewModelType, null, true);

    /// <summary>Presents a popup, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <param name="animate">True to animate the navigation transition.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPopup(Type viewModelType, string? contract, bool animate);

    /// <summary>Presents a popup, resolving its view from the registered contract.</summary>
    /// <param name="viewModel">The view model associated with this navigation operation.</param>
    /// <param name="navigationParameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPopup(INavigable viewModel, INavigationParameter navigationParameter, string? contract) => PushPopup(viewModel, navigationParameter, contract, true);

    /// <summary>Presents a popup, resolving its view from the registered contract.</summary>
    /// <param name="viewModel">The view model associated with this navigation operation.</param>
    /// <param name="navigationParameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPopup(INavigable viewModel, INavigationParameter navigationParameter) => PushPopup(viewModel, navigationParameter, null, true);

    /// <summary>Push a pop up page to the stack.</summary>
    /// <param name = "viewModel">The view model.</param>
    /// <param name = "navigationParameter">The navigation parameter.</param>
    /// <param name = "contract">The contract.</param>
    /// <param name = "animate">Animate the page.</param>
    /// <returns>A completion notification.</returns>
    IObservable<Unit> PushPopup(INavigable viewModel, INavigationParameter navigationParameter, string? contract, bool animate);

    /// <summary>Presents a popup, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="navigationParameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPopup(Type viewModelType, INavigationParameter navigationParameter, string? contract) => PushPopup(viewModelType, navigationParameter, contract, true);

    /// <summary>Presents a popup, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="navigationParameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPopup(Type viewModelType, INavigationParameter navigationParameter) => PushPopup(viewModelType, navigationParameter, null, true);

    /// <summary>Presents a popup, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="navigationParameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <param name="animate">True to animate the navigation transition.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPopup(Type viewModelType, INavigationParameter navigationParameter, string? contract, bool animate);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushPopup<TViewModel>(string? contract)
        where TViewModel : IViewModel => PushPopup<TViewModel>(contract, true);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushPopup<TViewModel>()
        where TViewModel : IViewModel => PushPopup<TViewModel>(null, true);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <param name = "animate">True to animate the navigation transition.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushPopup<TViewModel>(string? contract, bool animate)
        where TViewModel : IViewModel => PushPopup(typeof(TViewModel), contract, animate);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "navigationParameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushPopup<TViewModel>(INavigationParameter navigationParameter, string? contract)
        where TViewModel : INavigable => PushPopup<TViewModel>(navigationParameter, contract, true);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "navigationParameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushPopup<TViewModel>(INavigationParameter navigationParameter)
        where TViewModel : INavigable => PushPopup<TViewModel>(navigationParameter, null, true);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "navigationParameter">The parameters passed to navigation lifecycle callbacks.</param>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <param name = "animate">True to animate the navigation transition.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushPopup<TViewModel>(INavigationParameter navigationParameter, string? contract, bool animate)
        where TViewModel : INavigable => PushPopup(typeof(TViewModel), navigationParameter, contract, animate);

    /// <summary>Closes the top popup.</summary>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PopPopup() => PopPopup(true);

    /// <summary>Pop a pop up page.</summary>
    /// <param name = "animate">Animate the page.</param>
    /// <returns>A completion notification.</returns>
    IObservable<Unit> PopPopup(bool animate);

    /// <summary>Closes the popup stack.</summary>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PopAllPopups() => PopAllPopups(true);

    /// <summary>Pop all popups from the stack.</summary>
    /// <param name = "animate">Animate the page.</param>
    /// <returns>A completion notification.</returns>
    IObservable<Unit> PopAllPopups(bool animate);

    /// <summary>Removes the popup associated with the supplied view model.</summary>
    /// <param name="viewModel">The view model associated with this navigation operation.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> RemovePopup(IViewModel viewModel, string? contract) => RemovePopup(viewModel, contract, true);

    /// <summary>Removes the popup associated with the supplied view model.</summary>
    /// <param name="viewModel">The view model associated with this navigation operation.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> RemovePopup(IViewModel viewModel) => RemovePopup(viewModel, null, true);

    /// <summary>Remove Popup.</summary>
    /// <param name = "viewModel">The view model.</param>
    /// <param name = "contract">The contract.</param>
    /// <param name = "animate">Animate the page.</param>
    /// <returns>A completion notification.</returns>
    IObservable<Unit> RemovePopup(IViewModel viewModel, string? contract, bool animate);
}
