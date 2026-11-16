// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using NUnit.Framework;

namespace Sextant.Tests;

/// <summary>Groups the NavigationParameterTests navigation checks.</summary>
public static class NavigationParameterTests
{
    /// <summary>Tests the get value method.</summary>
    [TestFixture]
    public sealed class TheGetValueMethod
    {
        /// <summary>Tests the method gets a value.</summary>
        [Test]
        public void Should_Get_Value()
        {
            // Given
            NavigationParameter sut = new();
            sut.Add("key", TimeSpan.Zero);

            // When
            var result = sut.GetValue("key", default(TimeSpan));

            // Then
            Assert.That(result, Is.EqualTo(TimeSpan.Zero));
        }

        /// <summary>Tests the method gets a value.</summary>
        [Test]
        public void Should_Get_Default_Value()
        {
            // Given
            NavigationParameter sut = new();

            // When
            var result = sut.GetValue("key", default(TimeSpan));

            // Then
            Assert.That(result, Is.EqualTo(TimeSpan.Zero));
        }
    }

    /// <summary>Test the try get value method.</summary>
    [TestFixture]
    public sealed class TheTryGetValueMethod
    {
        /// <summary>Tests the method tries to gets a value.</summary>
        [Test]
        public void Should_Try_Get_Value()
        {
            // Given
            NavigationParameter sut = new();
            sut.Add("key", TimeSpan.Zero);

            // When
            var result = sut.TryGetValue<TimeSpan>("key", out _);

            // Then
            Assert.That(result, Is.True);
        }

        /// <summary>Tests the method tries to gets a value.</summary>
        [Test]
        public void Should_Not_Try_Get_Value()
        {
            // Given
            NavigationParameter sut = new();

            // When
            var result = sut.TryGetValue<TimeSpan>("key", out _);

            // Then
            Assert.That(result, Is.False);
        }
    }
}
