// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using ReactiveUI;

namespace Sextant.Plugins.Popup
{
    /// <summary>
    /// Represents a popup navigation event.
    /// </summary>
    public class PopupNavigationEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopupNavigationEvent"/> class.
        /// </summary>
        /// <param name="page">The view model.</param>
        /// <param name="isAnimated">Is the page animated.</param>
        public PopupNavigationEvent(IViewFor page, bool isAnimated)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            if (page.ViewModel == null)
            {
                throw new InvalidOperationException($"{nameof(page.ViewModel)} cannot be null.");
            }

            ViewModel = (IViewModel)page.ViewModel;
            IsAnimated = isAnimated;
        }

        /// <summary>
        /// Gets the view model for the event.
        /// </summary>
        public IViewModel ViewModel { get; }

        /// <summary>
        /// Gets a value indicating whether the page is animated.
        /// </summary>
        public bool IsAnimated { get; }
    }
}
