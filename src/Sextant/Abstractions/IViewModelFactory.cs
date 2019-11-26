// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant.Abstractions
{
    /// <summary>
    /// Interface that represents a view model factory.
    /// </summary>
    public interface IViewModelFactory
    {
        /// <summary>
        /// Creates an instance of the specified view model.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <param name="contract">The contract of the view model.</param>
        /// <returns>A view model instance.</returns>
        TViewModel Create<TViewModel>(string contract = null)
            where TViewModel : IViewModel;
    }
}
