// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System.Collections.Generic;

namespace Sextant;

/// <summary>Interface representing a parameter passed during navigation.</summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "By Design.")]
public interface INavigationParameter : IDictionary<string, object>
{
    /// <summary>Returns the parameter value, or the supplied fallback when the key is absent.</summary>
    /// <typeparam name="T">The view model or parameter value type.</typeparam>
    /// <param name="key">The navigation parameter key.</param>
    /// <param name="defaultValue">The fallback returned when the key is absent.</param>
    /// <returns>The typed parameter value or the fallback.</returns>
    T GetValue<T>(string key, T defaultValue);

    /// <summary>Gets the value from the navigation parameter with the specified key.</summary>
    /// <typeparam name = "T">The type of the parameter.</typeparam>
    /// <param name = "key">The key.</param>
    /// <param name = "value">The value.</param>
    /// <returns>A value indicating whether the value exists.</returns>
    bool TryGetValue<T>(string key, out T value);
}
