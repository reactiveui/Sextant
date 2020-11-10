// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using FluentAssertions;
using Xunit;

namespace Sextant.IOS.Runner
{
    /// <summary>
    /// Navigation view controller tests.
    /// </summary>
    public class NavigationViewControllerTests
    {
        /// <summary>
        /// Test that verifies view controller pops.
        /// </summary>
        [Fact]
        public void Should_Pop_View_Controller()
        {
            // Given
            var actual = false;
            NavigationViewController sut = new NavigationViewControllerFixture();
            sut.PagePopped.Subscribe(_ => { actual = true; });

            // When
            sut.PopPage();

            // Then
            actual.Should().BeTrue();
        }
    }
}
