// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant.Blazor
{
    /// <summary>
    /// Interface representing a Sextant view model.
    /// </summary>
    public interface IRouteViewModel : IViewModel
    {
        /// <summary>
        /// Gets or sets the original route that was intended when this viewmodel was created.
        /// </summary>
        string Route { get; set; }
    }
}
