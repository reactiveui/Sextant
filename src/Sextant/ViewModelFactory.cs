// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using Splat;

namespace Sextant;

/// <summary>
/// Helper class which obtains the currently registered IViewModelFactory interface in the Splat dependency service.
/// </summary>
public static class ViewModelFactory
{
    /// <summary>
    /// Gets the current registered IViewModelFactory interface.
    /// </summary>
    /// <value>
    /// The current.
    /// </value>
    /// <exception cref="ViewModelFactoryNotFoundException">Could not find a default ViewModelFactory. This should never happen, your dependency resolver is broken.</exception>
    [SuppressMessage("Microsoft.Reliability", "CA1065", Justification = "Exception required to keep interface same.")]
    public static IViewModelFactory Current
    {
        get
        {
            var locator = Locator.Current.GetService<IViewModelFactory>();
            return locator switch
            {
                null => throw new ViewModelFactoryNotFoundException("Could not find a default ViewModelFactory. This should never happen, your dependency resolver is broken"),
                _ => locator
            };
        }
    }
}
