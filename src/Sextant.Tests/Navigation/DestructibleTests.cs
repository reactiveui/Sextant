// Copyright (c) 2025 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using ReactiveUI;
using Sextant.Mocks;
using Splat;
using Xunit;

namespace Sextant.Tests;

/// <summary>
/// Test a <see cref="IDestructible"/> implementation.
/// </summary>
public sealed class DestructibleTests
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
            ParameterViewModel sut = new();

            // When
            sut.Disposable.IsDisposed.Should().BeFalse();
            sut.Destroy();

            // Then
            sut.Disposable.IsDisposed.Should().BeTrue();
        }

        /// <summary>
        /// Tests to make sure we receive a push page notification.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Call_When_Page_Popped()
        {
            Locator.CurrentMutable.InitializeSplat();
            Locator.CurrentMutable.InitializeReactiveUI();

            // Given
            var viewModel1 = Substitute.For<IDestructibleMock>();
            var viewModel2 = Substitute.For<IDestructibleMock>();
            ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(new NavigationViewMock());

            // When
            await sut.PushPage(viewModel1);
            await sut.PushPage(viewModel2);
            await sut.PopPage();

            // Then
            viewModel2.Received(1).Destroy();
        }
    }
}
