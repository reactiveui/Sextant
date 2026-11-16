// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Collections.Immutable;
using System.Reactive;

namespace Sextant;

/// <summary>Interface that defines a methods to interact with the navigation stack.</summary>
public interface IViewStackService
{
    /// <summary>Gets the modal navigation stack.</summary>
    IObservable<IImmutableList<IViewModel>> ModalStack { get; }

    /// <summary>Gets the page navigation stack.</summary>
    IObservable<IImmutableList<IViewModel>> PageStack { get; }

    /// <summary>Gets the current view on the stack.</summary>
    IView View { get; }

    /// <summary>Removes the top modal from the navigation stack.</summary>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PopModal() => PopModal(true);

    /// <summary>Pops the <see cref = "INavigable"/> off the stack.</summary>
    /// <param name = "animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the pop has been completed.</returns>
    IObservable<Unit> PopModal(bool animate);

    /// <summary>Removes the top page and sends navigation lifecycle notifications.</summary>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PopPage() => PopPage(true);

    /// <summary>Pops the <see cref = "INavigable"/> off the stack.</summary>
    /// <param name = "animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the pop has been completed.</returns>
    IObservable<Unit> PopPage(bool animate);

    /// <summary>Returns to the root page and destroys removed view models.</summary>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PopToRootPage() => PopToRootPage(true);

    /// <summary>Pops to root page.</summary>
    /// <param name = "animate">If set to <c>true</c> animate.</param>
    /// <returns>An observable that signals when the pop has been completed.</returns>
    IObservable<Unit> PopToRootPage(bool animate);

    /// <summary>Presents a modal, resolving its view from the registered contract.</summary>
    /// <param name="modal">The view model associated with this navigation operation.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushModal(IViewModel modal, string? contract) => PushModal(modal, contract, true);

    /// <summary>Presents a modal, resolving its view from the registered contract.</summary>
    /// <param name="modal">The view model associated with this navigation operation.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushModal(IViewModel modal) => PushModal(modal, null, true);

    /// <summary>Pushes the <see cref = "IViewModel"/> onto the stack.</summary>
    /// <param name = "modal">The modal.</param>
    /// <param name = "contract">The contract.</param>
    /// <param name = "withNavigationPage">Value indicating whether to wrap the modal in a navigation page.</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PushModal(IViewModel modal, string? contract, bool withNavigationPage);

    /// <summary>Presents a modal, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushModal(Type viewModelType, string? contract) => PushModal(viewModelType, contract, true);

    /// <summary>Presents a modal, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushModal(Type viewModelType) => PushModal(viewModelType, null, true);

    /// <summary>Presents a modal, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <param name="withNavigationPage">True to wrap the modal in a navigation page.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushModal(Type viewModelType, string? contract, bool withNavigationPage);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushModal<TViewModel>(string? contract)
        where TViewModel : IViewModel => PushModal<TViewModel>(contract, true);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushModal<TViewModel>()
        where TViewModel : IViewModel => PushModal<TViewModel>(null, true);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <param name = "withNavigationPage">True to wrap the modal in a navigation page.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushModal<TViewModel>(string? contract, bool withNavigationPage)
        where TViewModel : IViewModel => PushModal(typeof(TViewModel), contract, withNavigationPage);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <param name="resetStack">True to replace the current page stack.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(Type viewModelType, string? contract, bool resetStack) => PushPage(viewModelType, contract, resetStack, true);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(Type viewModelType, string? contract) => PushPage(viewModelType, contract, false, true);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(Type viewModelType) => PushPage(viewModelType, null, false, true);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <param name="resetStack">True to replace the current page stack.</param>
    /// <param name="animate">True to animate the navigation transition.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(Type viewModelType, string? contract, bool resetStack, bool animate);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="page">The view model associated with this navigation operation.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <param name="resetStack">True to replace the current page stack.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(IViewModel page, string? contract, bool resetStack) => PushPage(page, contract, resetStack, true);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="page">The view model associated with this navigation operation.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(IViewModel page, string? contract) => PushPage(page, contract, false, true);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="page">The view model associated with this navigation operation.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(IViewModel page) => PushPage(page, null, false, true);

    /// <summary>Pushes the <see cref = "IViewModel"/> onto the stack.</summary>
    /// <param name = "page">The page.</param>
    /// <param name = "contract">The contract.</param>
    /// <param name = "resetStack">if set to <c>true</c> [reset stack].</param>
    /// <param name = "animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PushPage(IViewModel page, string? contract, bool resetStack, bool animate);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="page">The view model associated with this navigation operation.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <param name="resetStack">True to replace the current page stack.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(INavigable page, string? contract, bool resetStack) => PushPage(page, contract, resetStack, true);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="page">The view model associated with this navigation operation.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(INavigable page, string? contract) => PushPage(page, contract, false, true);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="page">The view model associated with this navigation operation.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(INavigable page) => PushPage(page, null, false, true);

    /// <summary>Pushes the <see cref = "INavigable"/> onto the stack.</summary>
    /// <param name = "page">The page.</param>
    /// <param name = "contract">The contract.</param>
    /// <param name = "resetStack">if set to <c>true</c> [reset stack].</param>
    /// <param name = "animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PushPage(INavigable page, string? contract, bool resetStack, bool animate);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <param name = "resetStack">True to replace the page stack.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushPage<TViewModel>(string? contract, bool resetStack)
        where TViewModel : IViewModel => PushPage<TViewModel>(contract, resetStack, true);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushPage<TViewModel>(string? contract)
        where TViewModel : IViewModel => PushPage<TViewModel>(contract, false, true);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushPage<TViewModel>()
        where TViewModel : IViewModel => PushPage<TViewModel>(null, false, true);

    /// <summary>Resolves and presents the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <param name = "resetStack">True to replace the page stack.</param>
    /// <param name = "animate">True to animate the navigation transition.</param>
    /// <returns>A sequence that signals when navigation completes.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    IObservable<Unit> PushPage<TViewModel>(string? contract, bool resetStack, bool animate)
        where TViewModel : IViewModel => PushPage(typeof(TViewModel), contract, resetStack, animate);

    /// <summary>Returns the top page from the current navigation stack.</summary>
    /// <returns>An observable that signals the top page of the stack.</returns>
    IObservable<IViewModel> TopPage();

    /// <summary>Returns the top modal from the current modal stack.</summary>
    /// <returns>An observable that signals the top modal of the stack.</returns>
    IObservable<IViewModel> TopModal();
}
