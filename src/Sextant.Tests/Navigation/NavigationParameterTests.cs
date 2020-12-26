// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using FluentAssertions;
using Xunit;

namespace Sextant.Tests
{
    /// <summary>
    /// Tests <see cref="NavigationParameter"/> and extensions.
    /// </summary>
    public class NavigationParameterTests
    {
        /// <summary>
        /// Tests the method gets a value.
        /// </summary>
        [Fact]
        public void Should_Get_Value()
        {
            // Given
            NavigationParameter sut = new();
            sut.Add("key", TimeSpan.Zero);

            // When
            var result = sut.GetValue<TimeSpan>("key");

            // Then
            result
                .Should()
                .Be(TimeSpan.Zero);
        }

        /// <summary>
        /// Tests the method tries to gets a value.
        /// </summary>
        [Fact]
        public void Should_Try_Get_Value()
        {
            // Given
            NavigationParameter sut = new();
            sut.Add("key", TimeSpan.Zero);

            // When
            var result = sut.TryGetValue<TimeSpan>("key", out var value);

            // Then
            result
                .Should()
                .BeTrue();
        }
    }
}
