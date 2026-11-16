// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Mopups.Interfaces;

namespace Sextant.Plugins.Popup;

/// <summary>Abstract base class for view stack services.</summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PopupViewStackService"/> class.
/// </remarks>
public sealed class PopupViewStackService : PopupViewStackServiceBase
{
    /// <summary>Initializes a new instance of the <see cref="PopupViewStackService"/> class.</summary>
    /// <param name="view">The navigation view.</param>
    /// <param name="popupNavigation">The popup provider.</param>
    /// <param name="viewLocator">The view locator.</param>
    /// <param name="viewModelFactory">The view model factory.</param>
    public PopupViewStackService(IView view, IPopupNavigation popupNavigation, IViewLocator viewLocator, IViewModelFactory viewModelFactory)
        : base(view, popupNavigation, viewLocator, viewModelFactory)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="PopupViewStackService"/> class.</summary>
    /// <param name="view">The navigation view.</param>
    /// <param name="popupNavigation">The popup provider.</param>
    /// <param name="viewLocator">The view locator.</param>
    /// <param name="viewModelFactory">The view model factory.</param>
    /// <param name="logger">The navigation logger.</param>
    internal PopupViewStackService(IView view, IPopupNavigation popupNavigation, IViewLocator viewLocator, IViewModelFactory viewModelFactory, Splat.IFullLogger logger)
        : base(view, popupNavigation, viewLocator, viewModelFactory, logger)
    {
    }
}
