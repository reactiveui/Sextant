// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant
{
    /// <summary>
    /// Service implementation to handle navigation stack updates.
    /// Taken from https://kent-boogaart.com/blog/custom-routing-in-reactiveui and adjusted.
    /// </summary>
    /// <seealso cref="IViewStackService" />
    public sealed class ViewStackService : ViewStackServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStackService"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public ViewStackService(IView view)
            : base(view)
        {
        }
    }
}
