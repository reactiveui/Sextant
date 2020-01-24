// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Shouldly;
using Xunit;

namespace Sextant.Blazor.Tests
{
    /// <summary>
    /// Tests the <see cref="SextantNavigationManager"/>.
    /// </summary>
    public class SextantNavigationManagerTests
    {
        /// <summary>
        /// Tests the thing.
        /// </summary>
        [Fact]
        public void Test1()
        {
            // Given
            SextantNavigationManager manager = new SextantNavigationManagerFixture();

            // When
            var result = manager.AbsoluteUri;

            // Then
            result.ShouldNotBeNull();
        }
    }
}
