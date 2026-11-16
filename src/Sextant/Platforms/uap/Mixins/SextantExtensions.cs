// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Sextant
{
    /// <summary>
    /// Extension methods interact with <see cref="Sextant"/>.
    /// </summary>
    public static class SextantExtensions
    {
        /// <summary>
        /// Initializes the sextant.
        /// </summary>
        /// <param name="sextant">The sextant.</param>
        public static void Initialize(this Sextant sextant)
        {
            if (sextant is null)
            {
                throw new ArgumentNullException(nameof(sextant));
            }

            sextant
                .MutableLocator
                .RegisterUWPViewLocator()
                .RegisterNavigationView()
                .RegisterViewStackService()
                .RegisterParameterViewStackService()
                .RegisterViewModelFactory(() => new DefaultViewModelFactory());
        }
    }
}
