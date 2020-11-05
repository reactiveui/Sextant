// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;
using Rg.Plugins.Popup.Contracts;

namespace Sextant.Plugins.Popup
{
    /// <summary>
    /// Abstract base class for view stack services.
    /// </summary>
    public sealed class PopupViewStackService : PopupViewStackServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupViewStackService"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="popupNavigation">The popup navigation.</param>
        /// <param name="viewLocator">The view locator.</param>
        /// <param name="viewModelFactory">The view model factory.</param>
        public PopupViewStackService(IView view, IPopupNavigation popupNavigation, IViewLocator viewLocator, IViewModelFactory viewModelFactory)
            : base(view, popupNavigation, viewLocator, viewModelFactory)
        {
        }
    }
}
