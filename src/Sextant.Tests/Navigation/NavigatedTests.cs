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
/// Test a <see cref="INavigated"/> implementation.
/// </summary>
[TestFixture]
public sealed class NavigatedTests
{
    /// <summary>
    /// Tests the WhenNavigatedTo method.
    /// </summary>
    [TestFixture]
    public sealed class TheWhenNavigatedToMethod
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
            sut.WhenNavigatedTo(new NavigationParameter { { "hello", "world" }, { "life", 42 } }).Subscribe();

            // Then
            Assert.Multiple(() =>
            {
                Assert.That(sut.Text, Is.EqualTo("world"));
                Assert.That(sut.Meaning, Is.EqualTo(42));
            });
        }

        /// <summary>
        /// Should return null if no values are provided for the parameter.
        /// </summary>
        [Test]
        public void Should_Return_Null_If_No_Values_Provided()
        {
            // Given
            ParameterViewModel sut = new();

            // When
            sut.WhenNavigatedTo(new NavigationParameter());

            // Then
            Assert.That(sut.Text, Is.Null);
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
            Assert.Throws<KeyNotFoundException>(() => sut.WhenNavigatedTo(new NavigationParameter { { "hello", "world" } }).Subscribe());
        }
    }

    /// <summary>
    /// Tests the WhenNavigatedTo method.
    /// </summary>
    [TestFixture]
    public sealed class TheWhenNavigatedFromMethod
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
            sut.WhenNavigatedFrom(new NavigationParameter { { "hello", "world" }, { "life", 42 } }).Subscribe();

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
            Assert.Throws<KeyNotFoundException>(() => sut.WhenNavigatedFrom(new NavigationParameter { { "hello", "world" } }).Subscribe());
        }
    }
}
