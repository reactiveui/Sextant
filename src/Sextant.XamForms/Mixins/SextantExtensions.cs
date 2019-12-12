// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Splat;

namespace Sextant.XamForms
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
        public static void InitializeForms(this Sextant sextant) =>
            sextant
                .MutableLocator
                .RegisterNavigationView()
                .RegisterViewStackService()
                .RegisterParameterViewStackService()
                .RegisterViewModelFactory(() => new DefaultViewModelFactory());
    }
}
