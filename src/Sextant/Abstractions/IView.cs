// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Reactive;
using System.Reactive.Concurrency;

namespace Sextant;

/// <summary>Defines a view that be add to a navigation or modal stack.</summary>
public interface IView
{
    /// <summary>Gets the main thread scheduler for the <see cref = "IView"/> instance.</summary>
    IScheduler MainThreadScheduler { get; }

    /// <summary>Gets an observable notifying that a page was popped from the navigation stack.</summary>
    IObservable<IViewModel?> PagePopped { get; }

    /// <summary>Pops the modal from the modal stack.</summary>
    /// <returns>A signal that signals when the pop has been completed.</returns>
    IObservable<Unit> PopModal();

    /// <summary>Removes the top page and sends navigation lifecycle notifications.</summary>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PopPage() => PopPage(true);

    /// <summary>Pops the page from the navigation stack.</summary>
    /// <param name = "animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the pop has been completed.</returns>
    IObservable<Unit> PopPage(bool animate);

    /// <summary>Returns to the root page and destroys removed view models.</summary>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PopToRootPage() => PopToRootPage(true);

    /// <summary>Pops the root page.</summary>
    /// <param name = "animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the pop has been completed.</returns>
    IObservable<Unit> PopToRootPage(bool animate);

    /// <summary>Presents a modal, resolving its view from the registered contract.</summary>
    /// <param name="modalViewModel">The view model associated with this navigation operation.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract) => PushModal(modalViewModel, contract, true);

    /// <summary>Pushes the modal onto the modal stack.</summary>
    /// <param name = "modalViewModel">The modal view model.</param>
    /// <param name = "contract">The contract.</param>
    /// <param name = "withNavigationPage">Value indicating whether to wrap the modal in a navigation page.</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract, bool withNavigationPage);

    /// <summary>Adds a page to the navigation stack, resolving its view from the registered contract.</summary>
    /// <param name="viewModel">The view model associated with this navigation operation.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <param name="resetStack">True to replace the current page stack.</param>
    /// <returns>A sequence that signals when the navigation operation completes.</returns>
    IObservable<Unit> PushPage(IViewModel viewModel, string? contract, bool resetStack) => PushPage(viewModel, contract, resetStack, true);

    /// <summary>Pushes the page onto the navigation stack.</summary>
    /// <param name = "viewModel">The view model.</param>
    /// <param name = "contract">The contract.</param>
    /// <param name = "resetStack">if set to <c>true</c> [reset stack].</param>
    /// <param name = "animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PushPage(IViewModel viewModel, string? contract, bool resetStack, bool animate);
}
