// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using FluentAssertions;
using ReactiveUI;
using Sextant.Mocks;
using Splat;
using Xunit;

namespace Sextant.Tests
{
    /// <summary>
    /// Tests the IMutableDependencyResolver extension class.
    /// </summary>
    public sealed class DependencyResolverMixinTests
    {
        /// <summary>
        /// Tests the register view model factory method.
        /// </summary>
        public sealed class TheRegisterViewModelFactoryMethod
        {
            /// <summary>
            /// Should register the view model factory.
            /// </summary>
            [Fact]
            public void Should_Register_View_Model_Factory()
            {
                // Given
                Locator.CurrentMutable.RegisterViewModelFactory();

                // When
                var result = ViewModelFactory.Current;

                // Then
                result.Should().BeOfType<DefaultViewModelFactory>();
            }

            /// <summary>
            /// Should register the view model factory.
            /// </summary>
            [Fact]
            public void Should_Register_View_Model_Factory_With_Factory()
            {
                // Given
                var viewModelFactory = new DefaultViewModelFactory();
                Locator.CurrentMutable.RegisterViewModelFactory(() => viewModelFactory);

                // When
                var result = ViewModelFactory.Current;

                // Then
                result.Should().Be(viewModelFactory);
            }
        }

        /// <summary>
        /// Tests the register view method.
        /// </summary>
        public sealed class TheRegisterViewMethod
        {
            /// <summary>
            /// Should register the view stack service.
            /// </summary>
            [Fact]
            public void Should_Register_View()
            {
                // Given
                Locator.CurrentMutable.RegisterView<PageView, NavigableViewModelMock>();

                // When
                var result = Locator.Current.GetService<IViewFor<NavigableViewModelMock>>();

                // Then
                result.Should().BeOfType<PageView>();
            }

            /// <summary>
            /// Should register the view stack service factory.
            /// </summary>
            [Fact]
            public void Should_Register_View_Factory()
            {
                // Given
                Locator.CurrentMutable.RegisterView<PageView, NavigableViewModelMock>(() => new PageView());

                // When
                var result = Locator.Current.GetService<IViewFor<NavigableViewModelMock>>();

                // Then
                result.Should().BeOfType<PageView>();
            }
        }
    }
}
