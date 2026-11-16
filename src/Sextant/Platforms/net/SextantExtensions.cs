// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Sextant;

/// <summary>Extensions methods to setup the <see cref="Sextant"/> instance.</summary>
public static class SextantExtensions
{
    /// <summary>Extension members for <c>Sextant</c>.</summary>
    /// <param name="sextant">The Sextant instance to configure.</param>
    extension(Sextant sextant)
    {
        /// <summary>Initializes the specified sextant.</summary>
        public void Initialize()
        {
            ArgumentNullException.ThrowIfNull(sextant);

            NavigationRegistration.Initialize(sextant.MutableLocator);
        }
    }
}
