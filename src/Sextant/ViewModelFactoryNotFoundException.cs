// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Sextant;

/// <summary>An exception that is thrown if we are unable to find the View Model Factory.</summary>
[Serializable]
public class ViewModelFactoryNotFoundException : Exception
{
    /// <summary>Initializes a new instance of the <see cref="ViewModelFactoryNotFoundException"/> class.</summary>
    public ViewModelFactoryNotFoundException()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="ViewModelFactoryNotFoundException"/> class.</summary>
    /// <param name="message">The message.</param>
    public ViewModelFactoryNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="ViewModelFactoryNotFoundException" /> class.</summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ViewModelFactoryNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
