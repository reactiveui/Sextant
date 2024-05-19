// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant.Maui;

/// <summary>
/// Extension methods interact with <see cref="Sextant"/>.
/// </summary>
public static class SextantExtensions
{
    /// <summary>
    /// Initializes the sextant.
    /// </summary>
    /// <param name="sextant">The sextant.</param>
    public static void InitializeMaui(this Sextant sextant)
    {
        if (sextant is null)
        {
            throw new ArgumentNullException(nameof(sextant));
        }

        sextant
            .MutableLocator
            .RegisterNavigationView()
            .RegisterViewStackService()
            .RegisterParameterViewStackService()
            .RegisterViewModelFactory(() => new DefaultViewModelFactory());
    }
}
