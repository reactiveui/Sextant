// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Reactive.Testing;
using ReactiveUI;
using Sextant;

namespace Sextant.IOS.Runner
{
    /// <summary>
    /// Navigation view controller fixture.
    /// </summary>
    internal class NavigationViewControllerFixture
    {
        /// <summary>
        /// Performs an implicit conversion from <see cref="NavigationViewControllerFixture"/> to <see cref="NavigationViewController"/>.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator NavigationViewController(NavigationViewControllerFixture fixture) =>
            fixture?.Build();

        private NavigationViewController Build() =>
            new NavigationViewController(new TestScheduler(), new TestScheduler(), new TestViewLocator());
    }
}
