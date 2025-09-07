// Copyright (c) 2025 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sextant.Mocks;

namespace Sextant.Tests;

/// <summary>
/// Test a <see cref="INavigating"/> implementation.
/// </summary>
[TestFixture]
public sealed class NavigatingTests
{
    /// <summary>
    /// Tests the WhenNavigatingTo method.
    /// </summary>
    [TestFixture]
    public sealed class TheWhenNavigatingToMethod
    {
        /// <summary>
        /// Should unwrap the parameters.
        /// </summary>
        [Test]
        public void Should_Unwrap_Parameters()
        {
            // Given
            ParameterViewModel sut = new();

            // When
            sut.WhenNavigatingTo(new NavigationParameter { { "hello", "world" }, { "life", 42 } }).Subscribe();

            // Then
            Assert.Multiple(() =>
            {
                Assert.That(sut.Text, Is.EqualTo("world"));
                Assert.That(sut.Meaning, Is.EqualTo(42));
            });
        }

        /// <summary>
        /// Should not throw if key not found.
        /// </summary>
        [Test]
        public void Should_Throw_If_Key_Not_Found()
        {
            // Given
            ParameterViewModel sut = new();

            // When & Then
            Assert.Throws<KeyNotFoundException>(() => sut.WhenNavigatingTo(new NavigationParameter { { "hello", "world" } }).Subscribe());
        }
    }
}
