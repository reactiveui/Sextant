// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
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
    public sealed class TheWhenNavigatedToMethod
    {
        /// <summary>Should unwrap the parameters.</summary>
        /// <returns>A task that completes after the assertions.</returns>
        [Test]
        public async Task Should_Unwrap_Parameters()
        {
            // Given
            using ParameterViewModel sut = new();

            // When
            _ = sut.WhenNavigatedTo(new NavigationParameter { { GreetingKey, GreetingValue }, { "life", ExpectedMeaning } }).Subscribe();

            // Then
            using (Assert.Multiple())
            {
                await Assert.That(sut.Text).IsEqualTo(GreetingValue);
                await Assert.That(sut.Meaning).IsEqualTo(ExpectedMeaning);
            }
        }

        /// <summary>Should return null if no values are provided for the parameter.</summary>
        /// <returns>A task that completes after the assertions.</returns>
        [Test]
        public async Task Should_Return_Null_If_No_Values_Provided()
        {
            // Given
            using ParameterViewModel sut = new();

            // When
            _ = sut.WhenNavigatedTo(new NavigationParameter());

            // Then
            await Assert.That(sut.Text).IsNull();
        }

        /// <summary>Should not throw if key not found.</summary>
        /// <returns>A task that completes after the assertions.</returns>
        [Test]
        public async Task Should_Throw_If_Key_Not_Found()
        {
            // Given
            ParameterViewModel sut = new();

            // When & Then
            await Assert.That(() => sut.WhenNavigatedTo(new NavigationParameter { { GreetingKey, GreetingValue } }).Subscribe()).Throws<KeyNotFoundException>();
        }
    }

    /// <summary>Tests the WhenNavigatedTo method.</summary>
    public sealed class TheWhenNavigatedFromMethod
    {
        /// <summary>Should unwrap the parameters.</summary>
        /// <returns>A task that completes after the assertions.</returns>
        [Test]
        public async Task Should_Unwrap_Parameters()
        {
            // Given
            using ParameterViewModel sut = new();

            // When
            _ = sut.WhenNavigatedFrom(new NavigationParameter { { GreetingKey, GreetingValue }, { "life", ExpectedMeaning } }).Subscribe();

            // Then
            using (Assert.Multiple())
            {
                await Assert.That(sut.Text).IsEqualTo(GreetingValue);
                await Assert.That(sut.Meaning).IsEqualTo(ExpectedMeaning);
            }
        }

        /// <summary>Should not throw if key not found.</summary>
        /// <returns>A task that completes after the assertions.</returns>
        [Test]
        public async Task Should_Throw_If_Key_Not_Found()
        {
            // Given
            ParameterViewModel sut = new();

            // When & Then
            await Assert.That(() => sut.WhenNavigatedFrom(new NavigationParameter { { GreetingKey, GreetingValue } }).Subscribe()).Throws<KeyNotFoundException>();
        }
    }
}
