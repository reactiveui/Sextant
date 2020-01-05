﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Splat;

namespace Sextant
{
    /// <summary>
    /// The main registration point for Sextant.
    /// </summary>
    public class Sextant
    {
        private static readonly Lazy<Sextant> _sextant = new Lazy<Sextant>();

        static Sextant()
        {
            Locator.RegisterResolverCallbackChanged(() =>
            {
                if (Locator.CurrentMutable == null)
                {
                    return;
                }

                Instance.Initialize();
            });
        }

        /// <summary>
        /// Gets the instance of <see cref="Sextant"/>.
        /// </summary>
        public static Sextant Instance => _sextant.Value;

        /// <summary>
        /// Gets the mutable dependency resolver.
        /// </summary>
        [SuppressMessage("Design", "CA1822: Implement statically", Justification = "Existing API.")]
        public IMutableDependencyResolver MutableLocator => Locator.CurrentMutable;
    }
}
