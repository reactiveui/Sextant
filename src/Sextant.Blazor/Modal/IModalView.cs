// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Sextant.Blazor
{
    /// <summary>
    /// Interface for modal component that will work with Sextant.Blazor.
    /// </summary>
    public interface IModalView
    {
        /// <summary>
        /// Display the selected view type in a modal window.
        /// </summary>
        /// <param name="viewType">The view component type.</param>
        /// <param name="viewModel">The viewModel.</param>
        /// <returns>A Task.</returns>
        Task ShowViewAsync(Type viewType, IViewModel viewModel);

        /// <summary>
        /// Hide the modal window.
        /// </summary>
        /// <returns>A Task.</returns>
        Task HideAsync();
    }
}
