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
    /// The main registration point for Sextant.
    /// </summary>
    public class Sextant
    {
        private static readonly Lazy<Sextant> _sextant;

        static Sextant()
        {
            Locator.RegisterResolverCallbackChanged(() =>
            {
                if (Locator.CurrentMutable == null)
                {
                    return;
                }

                Initialize();
            });

            _sextant = new Lazy<Sextant>();
        }

        /// <summary>
        /// Gets the instance of <see cref="Sextant"/>.
        /// </summary>
        public static Sextant Instance => _sextant.Value;

        /// <summary>
        /// Initializes Sextant.
        /// </summary>
        public static void Initialize() =>
            Locator
                .CurrentMutable
                .RegisterNavigationView()
                .RegisterViewStackService();
    }
}
