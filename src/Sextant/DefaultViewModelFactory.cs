// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Sextant.Abstractions;
using Splat;

namespace Sextant
{
    /// <summary>
    /// Default View Model Factory.
    /// </summary>
    public class DefaultViewModelFactory : IViewModelFactory
    {
        /// <inheritdoc />
        public TViewModel Create<TViewModel>(string? contract = null)
            where TViewModel : IViewModel => Locator.Current.GetService<TViewModel>(contract);
    }
}
