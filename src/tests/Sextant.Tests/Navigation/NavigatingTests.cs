// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Sextant.Mocks;

namespace Sextant.Tests;

/// <summary>Groups the NavigatingTests navigation checks.</summary>
public static class NavigatingTests
{
    /// <summary>Tests the WhenNavigatingTo method.</summary>
    public sealed class TheWhenNavigatingToMethod
    {
        /// <summary>The navigation text key.</summary>
        private const string GreetingKey = "hello";

        /// <summary>The expected navigation text.</summary>
        private const string GreetingValue = "world";

        /// <summary>The expected numeric navigation value.</summary>
        private const int ExpectedMeaning = 42;

        /// <summary>Should unwrap the parameters.</summary>
        /// <returns>A task that completes after the assertions.</returns>
        [Test]
        public async Task Should_Unwrap_Parameters()
        {
            // Given
            using ParameterViewModel sut = new();

            // When
            _ = sut.WhenNavigatingTo(new NavigationParameter { { GreetingKey, GreetingValue }, { "life", ExpectedMeaning } }).Subscribe();

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
            await Assert.That(() => sut.WhenNavigatingTo(new NavigationParameter { { GreetingKey, GreetingValue } }).Subscribe()).Throws<KeyNotFoundException>();
        }
    }
}
