// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant.Maui;

/// <summary>Extension methods interact with <see cref="Sextant"/>.</summary>
public static class SextantExtensions
{
    /// <summary>Extension members for <c>Sextant</c>.</summary>
    /// <param name="sextant">The Sextant instance to configure.</param>
    extension(Sextant sextant)
    {
        /// <summary>Initializes the sextant.</summary>
        public void InitializeMaui()
        {
            ArgumentNullException.ThrowIfNull(sextant);

            _ = sextant
                .MutableLocator
                .RegisterNavigationView()
                .RegisterViewStackService()
                .RegisterParameterViewStackService()
                .RegisterViewModelFactory(static () => new DefaultViewModelFactory());
        }
    }
}
