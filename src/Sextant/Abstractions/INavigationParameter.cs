// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Sextant;

/// <summary>
/// Interface representing a parameter passed during navigation.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "By Design.")]
public interface INavigationParameter : IDictionary<string, object>
{
    /// <summary>
    /// Gets the value from the navigation parameter.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <typeparam name="T">The type parameter.</typeparam>
    /// <returns>The value.</returns>
    T GetValue<T>(string key);

    /// <summary>
    /// Gets the value from the navigation parameter with the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    /// <returns>A value indicating whether the value exists.</returns>
    bool TryGetValue<T>(string key, out T value);
}
