// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using FluentAssertions;
using Splat;
using Xunit;

namespace Sextant.XamForms.Tests
{
    /// <summary>
    /// Tests <see cref="Sextant"/> initialization methods.
    /// </summary>
    public sealed class SextantExtensionTests
    {
        /// <summary>
        /// Tests the Sextant Initalize Forms method.
        /// </summary>
        public sealed class TheInitializeFormsMethod
        {
            /// <summary>
            /// Tests the navigation view is registered.
            /// </summary>
            [Fact]
            public void Should_Register_Navigation_View()
            {
                // Given
                Sextant.Instance.InitializeForms();

                // When
                var result = Locator.Current.GetService<IView>(nameof(NavigationView));

                // Then
                result.Should().BeOfType<NavigationView>();
            }

            /// <summary>
            /// Tests the navigation view is registered.
            /// </summary>
            [Fact]
            public void Should_Register_View_Stack_Service()
            {
                // Given
                Sextant.Instance.InitializeForms();

                // When
                var result = Locator.Current.GetService<IViewStackService>();

                // Then
                result.Should().BeOfType<ParameterViewStackService>();
            }

            /// <summary>
            /// Tests the navigation view is registered.
            /// </summary>
            [Fact]
            public void Should_Register_Default_View_Model_Factory()
            {
                // Given
                Sextant.Instance.InitializeForms();

                // When
                var result = ViewModelFactory.Current;

                // Then
                result.Should().BeAssignableTo<IViewModelFactory>();
            }
        }
    }
}
