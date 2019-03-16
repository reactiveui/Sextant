// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Immutable;
using System.Reactive;

namespace Sextant.Abstraction
{
    /// <summary>
    /// Interface that defines a methods to interact with the navigation stack.
    /// </summary>
    public interface IViewStackService
    {
        /// <summary>
        /// Gets the modal navigation stack.
        /// </summary>
        IObservable<IImmutableList<IPageViewModel>> ModalStack { get; }

        /// <summary>
        /// Gets the page navigation stack.
        /// </summary>
        IObservable<IImmutableList<IPageViewModel>> PageStack { get; }

        /// <summary>
        /// Gets the current view on the stack.
        /// </summary>
        IView View { get; }

        /// <summary>
        /// Pops the <see cref="IPageViewModel"/> off the stack.
        /// </summary>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns>A signal.</returns>
        IObservable<Unit> PopModal(bool animate = true);

        /// <summary>
        /// Pops the <see cref="IPageViewModel"/> off the stack.
        /// </summary>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns>A signal.</returns>
        IObservable<Unit> PopPage(bool animate = true);

        /// <summary>
        /// Pops to root page.
        /// </summary>
        /// <returns>The to root page.</returns>
        /// <param name="animate">If set to <c>true</c> animate.</param>
        IObservable<Unit> PopToRootPage(bool animate = true);

        /// <summary>
        /// Pushes the <see cref="IPageViewModel"/> onto the stack.
        /// </summary>
        /// <param name="modal">The modal.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>A signal.</returns>
        IObservable<Unit> PushModal(IPageViewModel modal, string contract = null);

        /// <summary>
        /// Pushes the <see cref="IPageViewModel"/> onto the stack.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="resetStack">if set to <c>true</c> [reset stack].</param>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns>A signal.</returns>
        IObservable<Unit> PushPage(IPageViewModel page, string contract = null, bool resetStack = false, bool animate = true);

        /// <summary>
        /// Returns the top page from the current navigation stack.
        /// </summary>
        /// <returns>A view model.</returns>
        IObservable<IPageViewModel> TopPage();

        /// <summary>
        /// Returns the top modal from the current modal stack.
        /// </summary>
        /// <returns>A view model.</returns>
        IObservable<IPageViewModel> TopModal();
    }
}
