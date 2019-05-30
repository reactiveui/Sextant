// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Splat;

namespace Sextant
{
    /// <summary>
    /// The main registration point for Sextant.
    /// </summary>
    public class Sextant
    {
        private static readonly Lazy<Sextant> _sextant;

        static Sextant()
        {
            Splat.Locator.RegisterResolverCallbackChanged(() =>
            {
                if (Splat.Locator.CurrentMutable == null)
                {
                    return;
                }

                Instance.Initialize();
            });

            _sextant = new Lazy<Sextant>();
        }

        /// <summary>
        /// Gets the instance of <see cref="Sextant"/>.
        /// </summary>
        public static Sextant Instance => _sextant.Value;

        /// <summary>
        /// Gets the mutable dependency resolver.
        /// </summary>
        public IMutableDependencyResolver MutableLocator => Splat.Locator.CurrentMutable;
    }
}
