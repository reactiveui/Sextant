// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Immutable;
using System.Reactive;

namespace Sextant;

/// <summary>
/// Interface that defines a methods to interact with the navigation stack.
/// </summary>
public interface IViewStackService
{
    /// <summary>
    /// Gets the modal navigation stack.
    /// </summary>
    IObservable<IImmutableList<IViewModel>> ModalStack { get; }

    /// <summary>
    /// Gets the page navigation stack.
    /// </summary>
    IObservable<IImmutableList<IViewModel>> PageStack { get; }

    /// <summary>
    /// Gets the current view on the stack.
    /// </summary>
    IView View { get; }

    /// <summary>
    /// Pops the <see cref="INavigable"/> off the stack.
    /// </summary>
    /// <param name="animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the pop has been completed.</returns>
    IObservable<Unit> PopModal(bool animate = true);

    /// <summary>
    /// Pops the <see cref="INavigable"/> off the stack.
    /// </summary>
    /// <param name="animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the pop has been completed.</returns>
    IObservable<Unit> PopPage(bool animate = true);

    /// <summary>
    /// Pops to root page.
    /// </summary>
    /// <param name="animate">If set to <c>true</c> animate.</param>
    /// <returns>An observable that signals when the pop has been completed.</returns>
    IObservable<Unit> PopToRootPage(bool animate = true);

    /// <summary>
    /// Pushes the <see cref="IViewModel"/> onto the stack.
    /// </summary>
    /// <param name="modal">The modal.</param>
    /// <param name="contract">The contract.</param>
    /// <param name="withNavigationPage">Value indicating whether to wrap the modal in a navigation page.</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PushModal(IViewModel modal, string? contract = null, bool withNavigationPage = true);

    /// <summary>
    /// Pushes the <see cref="INavigable" /> onto the stack.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <param name="contract">The contract.</param>
    /// <param name="withNavigationPage">Value indicating whether to wrap the modal in a navigation page.</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PushModal<TViewModel>(string? contract = null, bool withNavigationPage = true)
        where TViewModel : IViewModel;

    /// <summary>
    /// Pushes the <see cref="INavigable" /> onto the stack.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <param name="contract">The contract.</param>
    /// <param name="resetStack">if set to <c>true</c> [reset stack].</param>
    /// <param name="animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PushPage<TViewModel>(string? contract = null, bool resetStack = false, bool animate = true)
        where TViewModel : IViewModel;

    /// <summary>
    /// Pushes the <see cref="IViewModel"/> onto the stack.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="contract">The contract.</param>
    /// <param name="resetStack">if set to <c>true</c> [reset stack].</param>
    /// <param name="animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PushPage(IViewModel page, string? contract = null, bool resetStack = false, bool animate = true);

    /// <summary>
    /// Pushes the <see cref="INavigable"/> onto the stack.
    /// </summary>
    /// <param name="page">The page.</param>
    /// <param name="contract">The contract.</param>
    /// <param name="resetStack">if set to <c>true</c> [reset stack].</param>
    /// <param name="animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PushPage(INavigable page, string? contract = null, bool resetStack = false, bool animate = true);

    /// <summary>
    /// Returns the top page from the current navigation stack.
    /// </summary>
    /// <returns>An observable that signals the top page of the stack.</returns>
    IObservable<IViewModel> TopPage();

    /// <summary>
    /// Returns the top modal from the current modal stack.
    /// </summary>
    /// <returns>An observable that signals the top modal of the stack.</returns>
    IObservable<IViewModel> TopModal();
}
