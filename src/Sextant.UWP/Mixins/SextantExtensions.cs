// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Text;
using Splat;

namespace Sextant.UWP
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
        public static void InitializeUWP(this Sextant sextant) =>
            sextant
                .MutableLocator
                .RegisterUWPViewLocator()
                .RegisterNavigationView()
                .RegisterViewStackService()
                .RegisterParameterViewStackService()
                .RegisterViewModelFactory(() => new DefaultViewModelFactory());
    }
}
