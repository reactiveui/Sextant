// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Concurrency;

namespace Sextant;

/// <summary>
/// Defines a view that be add to a navigation or modal stack.
/// </summary>
public interface IView
{
    /// <summary>
    /// Gets the main thread scheduler for the <see cref="IView"/> instance.
    /// </summary>
    IScheduler MainThreadScheduler { get; }

    /// <summary>
    /// Gets an observable notifying that a page was popped from the navigation stack.
    /// </summary>
    IObservable<IViewModel?> PagePopped { get; }

    /// <summary>
    /// Pops the modal from the modal stack.
    /// </summary>
    /// <returns>A signal that signals when the pop has been completed.</returns>
    IObservable<Unit> PopModal();

    /// <summary>
    /// Pops the page from the navigation stack.
    /// </summary>
    /// <param name="animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the pop has been completed.</returns>
    IObservable<Unit> PopPage(bool animate = true);

    /// <summary>
    /// Pops the root page.
    /// </summary>
    /// <param name="animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the pop has been completed.</returns>
    IObservable<Unit> PopToRootPage(bool animate = true);

    /// <summary>
    /// Pushes the modal onto the modal stack.
    /// </summary>
    /// <param name="modalViewModel">The modal view model.</param>
    /// <param name="contract">The contract.</param>
    /// <param name="withNavigationPage">Value indicating whether to wrap the modal in a navigation page.</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract, bool withNavigationPage = true);

    /// <summary>
    /// Pushes the page onto the navigation stack.
    /// </summary>
    /// <param name="viewModel">The view model.</param>
    /// <param name="contract">The contract.</param>
    /// <param name="resetStack">if set to <c>true</c> [reset stack].</param>
    /// <param name="animate">if set to <c>true</c> [animate].</param>
    /// <returns>An observable that signals when the push has been completed.</returns>
    IObservable<Unit> PushPage(
        IViewModel viewModel,
        string? contract,
        bool resetStack,
        bool animate = true);
}
