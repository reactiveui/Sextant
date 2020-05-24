// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Sextant.Mocks;
using Shouldly;
using Xunit;

namespace Sextant.Tests
{
    /// <summary>
    /// Test a <see cref="IDestructible"/> implementation.
    /// </summary>
    public class DestructibleTests
    {
        /// <summary>
        /// Tests the destroy method.
        /// </summary>
        public class TheDestroyMethod
        {
            /// <summary>
            /// Should unwrap the parameters.
            /// </summary>
            [Fact]
            public void Should_Destroy()
            {
                // Given
                ParameterViewModel sut = new ParameterViewModel();

                // When
                sut.Disposable.IsDisposed.ShouldBeFalse();
                sut.Destroy();

                // Then
                sut.Disposable.IsDisposed.ShouldBeTrue();
            }
        }
    }
}
