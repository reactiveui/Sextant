// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Sextant.Blazor
{
    /// <summary>
    /// Specialized viewmodel that is only used to notify the router that it should navigate to the url directly instead of loading a razor page.
    /// </summary>
    public class DirectRouteViewModel : IViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DirectRouteViewModel"/> class.
        /// </summary>
        /// <param name="route">The route.</param>
        public DirectRouteViewModel(string route)
        {
            Route = route;
        }

        /// <inheritdoc/>
        public string Id => "This ViewModel tells SextantRouter to let the page navigate to the actual url in this viewmodel.";

        /// <summary>
        /// Gets or sets the original route that was intended when this viewmodel was created.
        /// </summary>
        public string Route { get; set; }
    }
}
