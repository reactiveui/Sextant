// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Sextant.Tests;

/// <summary>Groups the NavigationParameterTests navigation checks.</summary>
public static class NavigationParameterTests
{
    /// <summary>Tests the get value method.</summary>
    public sealed class TheGetValueMethod
    {
        /// <summary>Tests the method gets a value.</summary>
        /// <returns>A task that completes after the assertions.</returns>
        [Test]
        public async Task Should_Get_Value()
        {
            // Given
            NavigationParameter sut = new();
            sut.Add("key", TimeSpan.Zero);

            // When
            var result = sut.GetValue("key", default(TimeSpan));

            // Then
            await Assert.That(result).IsEqualTo(TimeSpan.Zero);
        }

        /// <summary>Tests the method gets a value.</summary>
        /// <returns>A task that completes after the assertions.</returns>
        [Test]
        public async Task Should_Get_Default_Value()
        {
            // Given
            NavigationParameter sut = new();

            // When
            var result = sut.GetValue("key", default(TimeSpan));

            // Then
            await Assert.That(result).IsEqualTo(TimeSpan.Zero);
        }
    }

    /// <summary>Test the try get value method.</summary>
    public sealed class TheTryGetValueMethod
    {
        /// <summary>Tests the method tries to gets a value.</summary>
        /// <returns>A task that completes after the assertions.</returns>
        [Test]
        public async Task Should_Try_Get_Value()
        {
            // Given
            NavigationParameter sut = new();
            sut.Add("key", TimeSpan.Zero);

            // When
            var result = sut.TryGetValue<TimeSpan>("key", out _);

            // Then
            await Assert.That(result).IsTrue();
        }

        /// <summary>Tests the method tries to gets a value.</summary>
        /// <returns>A task that completes after the assertions.</returns>
        [Test]
        public async Task Should_Not_Try_Get_Value()
        {
            // Given
            NavigationParameter sut = new();

            // When
            var result = sut.TryGetValue<TimeSpan>("key", out _);

            // Then
            await Assert.That(result).IsFalse();
        }
    }
}
