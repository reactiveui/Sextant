// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Splat;

namespace Sextant;

/// <summary>
/// Default View Model Factory.
/// </summary>
public class DefaultViewModelFactory : IViewModelFactory
{
    /// <inheritdoc />
    public TViewModel Create<TViewModel>(string? contract = null)
        where TViewModel : IViewModel
    {
        var viewModel = Locator.Current.GetService<TViewModel>(contract);
        return viewModel switch
        {
            null => throw new InvalidOperationException($"ViewModel of type {typeof(TViewModel).Name} {contract} not registered."),
            _ => viewModel
        };
    }
}
