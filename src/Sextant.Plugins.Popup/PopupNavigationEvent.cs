// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Sextant.Plugins.Popup;

/// <summary>Represents a popup navigation event.</summary>
[System.Diagnostics.DebuggerDisplay("PopupNavigationEvent: {ViewModel}")]
public class PopupNavigationEvent
{
    /// <summary>Initializes a new instance of the <see cref="PopupNavigationEvent"/> class.</summary>
    /// <param name="page">The view model.</param>
    /// <param name="isAnimated">Is the page animated.</param>
    /// <exception cref="InvalidOperationException">Thrown when <c>page.ViewModel is null</c>.</exception>
    public PopupNavigationEvent(IViewFor page, bool isAnimated)
    {
        ArgumentNullException.ThrowIfNull(page);

        if (page.ViewModel is null)
        {
            throw new InvalidOperationException($"{nameof(page.ViewModel)} cannot be null.");
        }

        ViewModel = (IViewModel)page.ViewModel;
        IsAnimated = isAnimated;
    }

    /// <summary>Gets the view model for the event.</summary>
    public IViewModel ViewModel { get; }

    /// <summary>Gets a value indicating whether the page is animated.</summary>
    public bool IsAnimated { get; }
}
