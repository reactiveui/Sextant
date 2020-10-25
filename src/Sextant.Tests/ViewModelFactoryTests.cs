// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using FluentAssertions;
using Splat;
using Xunit;

namespace Sextant.Tests
{
    /// <summary>
    /// Tests the <see cref="ViewModelFactory"/>.
    /// </summary>
    public sealed class ViewModelFactoryTests
    {
        /// <summary>
        /// Tests the currently registered view model factory parameter.
        /// </summary>
        public class CurrentPropertyTests
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CurrentPropertyTests"/> class.
            /// </summary>
            public CurrentPropertyTests()
            {
                Locator.CurrentMutable.UnregisterAll<IViewModelFactory>();
            }

            /// <summary>
            /// Should throw if the IViewFactory is not registered.
            /// </summary>
            [Fact]
            public void Should_Throw_If_Not_Registered()
            {
                // Given, When
                var result = Record.Exception(() => ViewModelFactory.Current);

                // Then
                result.Should().BeOfType<ViewModelFactoryNotFoundException>().Which.Message.Should().Be("Could not find a default ViewModelFactory. This should never happen, your dependency resolver is broken");
            }

            /// <summary>
            /// Should return the default view model factory.
            /// </summary>
            [Fact]
            public void Should_Return_View_Model_Factory()
            {
                // Given, When
                Splat.Locator.CurrentMutable.Register(() => new DefaultViewModelFactory(), typeof(IViewModelFactory));
                var viewModelFactory = ViewModelFactory.Current;

                // Then
                viewModelFactory.Should().BeAssignableTo<IViewModelFactory>();
                viewModelFactory.Should().BeOfType<DefaultViewModelFactory>();
            }
        }
    }
}
