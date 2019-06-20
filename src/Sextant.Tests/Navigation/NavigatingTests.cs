using System;
using System.Collections.Generic;
using Sextant.Mocks;
using Shouldly;
using Xunit;

namespace Sextant.Tests
{
    /// <summary>
    /// Test a <see cref="INavigating"/> implementation.
    /// </summary>
    public sealed class NavigatingTests
    {
        /// <summary>
        /// Tests the WhenNavigatingTo method.
        /// </summary>
        public sealed class TheWhenNavigatingToMethod
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
                sut.WhenNavigatingTo(new NavigationParameter { { "hello", "world" }, { "life", 42 } }).Subscribe();

                // Then
                sut.Text.ShouldBe("world");
            }

            /// <summary>
            /// Should return null if key not found.
            /// </summary>
            [Fact]
            public void Should_Return_Null_If_Key_Not_Found()
            {
                // Given
                ParameterViewModel sut = new ParameterViewModel();

                // When
                sut.WhenNavigatingTo(new NavigationParameter());

                // Then
                sut.Text.ShouldBeNull();
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
                var result = Should.Throw<KeyNotFoundException>(() => sut.WhenNavigatingTo(new NavigationParameter { { "hello", "world" } }).Subscribe());

                // Then
                result.Message.ShouldBe("The given key 'life' was not present in the dictionary.");
            }
        }
    }
}
