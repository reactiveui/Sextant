// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Sextant.Mocks;
using Xunit;

namespace Sextant.Tests
{
    /// <summary>
    /// Test a <see cref="INavigated"/> implementation.
    /// </summary>
    public sealed class NavigatedTests
    {
        /// <summary>
        /// Tests the WhenNavigatedTo method.
        /// </summary>
        public sealed class TheWhenNavigatedToMethod
        {
            /// <summary>
            /// Should unwrap the parameters.
            /// </summary>
            [Fact]
            public void Should_Unwrap_Parameters()
            {
                // Given
                ParameterViewModel sut = new ParameterViewModel();

                // When
                sut.WhenNavigatedTo(new NavigationParameter { { "hello", "world" }, { "life", 42 } }).Subscribe();

                // Then
                sut.Text.Should().Be("world");
                sut.Meaning.Should().Be(42);
            }

            /// <summary>
            /// Should return null if no values are provided for the parameter.
            /// </summary>
            [Fact]
            public void Should_Return_Null_If_No_Values_Provided()
            {
                // Given
                ParameterViewModel sut = new ParameterViewModel();

                // When
                sut.WhenNavigatedTo(new NavigationParameter());

                // Then
                sut.Text.Should().BeNull();
            }

            /// <summary>
            /// Should not throw if key not found.
            /// </summary>
            [Fact]
            public void Should_Throw_If_Key_Not_Found()
            {
                // Given
                ParameterViewModel sut = new ParameterViewModel();

                // When
                var result = Record.Exception(() => sut.WhenNavigatedTo(new NavigationParameter { { "hello", "world" } }).Subscribe());

                // Then
                result.Should().BeOfType<KeyNotFoundException>();
            }
        }

        /// <summary>
        /// Tests the WhenNavigatedTo method.
        /// </summary>
        public sealed class TheWhenNavigatedFromMethod
        {
            /// <summary>
            /// Should unwrap the parameters.
            /// </summary>
            [Fact]
            public void Should_Unwrap_Parameters()
            {
                // Given
                ParameterViewModel sut = new ParameterViewModel();

                // When
                sut.WhenNavigatedFrom(new NavigationParameter { { "hello", "world" }, { "life", 42 } }).Subscribe();

                // Then
                sut.Text.Should().Be("world");
                sut.Meaning.Should().Be(42);
            }

            /// <summary>
            /// Should return null if key not found.
            /// </summary>
            [Fact]
            public void Should_Return_Null_If_No_Values_Provided()
            {
                // Given
                ParameterViewModel sut = new ParameterViewModel();

                // When
                sut.WhenNavigatedFrom(new NavigationParameter());

                // Then
                sut.Text.Should().BeNull();
            }

            /// <summary>
            /// Should not throw if key not found.
            /// </summary>
            [Fact]
            public void Should_Throw_If_Key_Not_Found()
            {
                // Given
                ParameterViewModel sut = new ParameterViewModel();

                // When
                var result = Record.Exception(() => sut.WhenNavigatedFrom(new NavigationParameter { { "hello", "world" } }).Subscribe());

                // Then
                result.Should().BeOfType<KeyNotFoundException>();
            }
        }
    }
}
