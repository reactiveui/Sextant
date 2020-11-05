// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive;

namespace Sextant.Plugins.Popup
{
    /// <summary>
    /// Interface representing a Sextant decorator for <see cref="Rg.Plugins.Popup.Contracts.IPopupNavigation"/>.
    /// </summary>
    public interface IPopupViewStackService : IParameterViewStackService
    {
        /// <summary>
        /// Gets an observable sequence of pushing events.
        /// </summary>
        IObservable<PopupNavigationEvent> Pushing { get; }

        /// <summary>
        /// Gets an observable sequence of pushed events.
        /// </summary>
        IObservable<PopupNavigationEvent> Pushed { get; }

        /// <summary>
        /// Gets an observable sequence of popping events.
        /// </summary>
        IObservable<PopupNavigationEvent> Popping { get; }

        /// <summary>
        /// Gets an observable sequence of popped events.
        /// </summary>
        IObservable<PopupNavigationEvent> Popped { get; }

        /// <summary>
        /// Gets the popup stack.
        /// </summary>
        IReadOnlyList<IViewModel> PopupStack { get; }

        /// <summary>
        /// Push a pop up page to the stack.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="animate">Animate the page.</param>
        /// <returns>A completion notification.</returns>
        IObservable<Unit> PushPopup(IViewModel viewModel, string? contract = null, bool animate = true);

        /// <summary>
        /// Push a pop up page to the stack.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="animate">Animate the page.</param>
        /// <typeparam name="TViewModel">The view model type.</typeparam>
        /// <returns>A completion notification.</returns>
        IObservable<Unit> PushPopup<TViewModel>(string? contract = null, bool animate = true)
            where TViewModel : IViewModel;

        /// <summary>
        /// Push a pop up page to the stack.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="navigationParameter">The navigation parameter.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="animate">Animate the page.</param>
        /// <returns>A completion notification.</returns>
        IObservable<Unit> PushPopup(INavigable viewModel, INavigationParameter navigationParameter, string? contract = null, bool animate = true);

        /// <summary>
        /// Push a pop up page to the stack.
        /// </summary>
        /// <param name="navigationParameter">The navigation parameter.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="animate">Animate the page.</param>
        /// <typeparam name="TViewModel">The view model type.</typeparam>
        /// <returns>A completion notification.</returns>
        IObservable<Unit> PushPopup<TViewModel>(INavigationParameter navigationParameter, string? contract = null, bool animate = true)
            where TViewModel : INavigable;

        /// <summary>
        /// Pop a pop up page.
        /// </summary>
        /// <param name="animate">Animate the page.</param>
        /// <returns>A completion notification.</returns>
        IObservable<Unit> PopPopup(bool animate = true);

        /// <summary>
        /// Pop all popups from the stack.
        /// </summary>
        /// <param name="animate">Animate the page.</param>
        /// <returns>A completion notification.</returns>
        IObservable<Unit> PopAllPopups(bool animate = true);

        /// <summary>
        /// Remove Popup.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="animate">Animate the page.</param>
        /// <returns>A completion notification.</returns>
        IObservable<Unit> RemovePopup(IViewModel viewModel, string? contract = null, bool animate = true);
    }
}
