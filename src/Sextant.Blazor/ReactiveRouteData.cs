// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Sextant.Blazor
{
    /// <summary>
    /// Describes information determined during routing that specifies
    /// the page to be displayed.
    /// </summary>
    public sealed class ReactiveRouteData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveRouteData"/> class.
        /// </summary>
        /// <param name="pageType">The type of the page matching the route, which must implement <see cref="IComponent"/>.</param>
        /// <param name="routeValues">The route parameter values extracted from the matched route.</param>
        public ReactiveRouteData(Type pageType, IReadOnlyDictionary<string, object> routeValues)
        {
            if (pageType == null)
            {
                throw new ArgumentNullException(nameof(pageType));
            }

            if (!typeof(IComponent).IsAssignableFrom(pageType))
            {
                throw new ArgumentException($"The value must implement {nameof(IComponent)}.", nameof(pageType));
            }

            PageType = pageType;
            RouteValues = routeValues ?? throw new ArgumentNullException(nameof(routeValues));
        }

        /// <summary>
        /// Gets the type of the page matching the route.
        /// </summary>
        public Type PageType { get; }

        /// <summary>
        /// Gets route parameter values extracted from the matched route.
        /// </summary>
        public IReadOnlyDictionary<string, object> RouteValues { get; }
    }
}
