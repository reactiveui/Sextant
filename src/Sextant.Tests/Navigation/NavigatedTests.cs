// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using NUnit.Framework;
using Sextant.Mocks;

namespace Sextant.Tests;

/// <summary>Groups the NavigatedTests navigation checks.</summary>
public static class NavigatedTests
{
    /// <summary>The navigation text key.</summary>
    private const string GreetingKey = "hello";

    /// <summary>The expected navigation text.</summary>
    private const string GreetingValue = "world";

    /// <summary>The expected numeric navigation value.</summary>
    private const int ExpectedMeaning = 42;

    /// <summary>Tests the WhenNavigatedTo method.</summary>
    [TestFixture]
    public sealed class TheWhenNavigatedToMethod
    {
        /// <summary>Should unwrap the parameters.</summary>
        [Test]
        public void Should_Unwrap_Parameters()
        {
            // Given
            ParameterViewModel sut = new();

            // When
            _ = sut.WhenNavigatedTo(new NavigationParameter { { GreetingKey, GreetingValue }, { "life", ExpectedMeaning } }).Subscribe();

            // Then
            Assert.Multiple(() =>
            {
                Assert.That(sut.Text, Is.EqualTo(GreetingValue));
                Assert.That(sut.Meaning, Is.EqualTo(ExpectedMeaning));
            });
        }

        /// <summary>Should return null if no values are provided for the parameter.</summary>
        [Test]
        public void Should_Return_Null_If_No_Values_Provided()
        {
            // Given
            using ParameterViewModel sut = new();

            // When
            _ = sut.WhenNavigatedTo(new NavigationParameter());

            // Then
            Assert.That(sut.Text, Is.Null);
        }

        /// <summary>Should not throw if key not found.</summary>
        [Test]
        public void Should_Throw_If_Key_Not_Found()
        {
            // Given
            ParameterViewModel sut = new();

            // When & Then
            _ = Assert.Throws<KeyNotFoundException>(() => sut.WhenNavigatedTo(new NavigationParameter { { GreetingKey, GreetingValue } }).Subscribe());
        }
    }

    /// <summary>Tests the WhenNavigatedTo method.</summary>
    [TestFixture]
    public sealed class TheWhenNavigatedFromMethod
    {
        /// <summary>Should unwrap the parameters.</summary>
        [Test]
        public void Should_Unwrap_Parameters()
        {
            // Given
            ParameterViewModel sut = new();

            // When
            _ = sut.WhenNavigatedFrom(new NavigationParameter { { GreetingKey, GreetingValue }, { "life", ExpectedMeaning } }).Subscribe();

            // Then
            Assert.Multiple(() =>
            {
                Assert.That(sut.Text, Is.EqualTo(GreetingValue));
                Assert.That(sut.Meaning, Is.EqualTo(ExpectedMeaning));
            });
        }

        /// <summary>Should not throw if key not found.</summary>
        [Test]
        public void Should_Throw_If_Key_Not_Found()
        {
            // Given
            ParameterViewModel sut = new();

            // When & Then
            _ = Assert.Throws<KeyNotFoundException>(() => sut.WhenNavigatedFrom(new NavigationParameter { { GreetingKey, GreetingValue } }).Subscribe());
        }
    }
}
