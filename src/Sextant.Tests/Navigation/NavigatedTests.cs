using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sextant.Mocks;
using Shouldly;
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
                sut.WhenNavigatedTo(new NavigationParameter());

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
                var result = Should.Throw<KeyNotFoundException>(() => sut.WhenNavigatedTo(new NavigationParameter { { "hello", "world" } }).Subscribe());

                // Then
                result.Message.ShouldBe("The given key 'life' was not present in the dictionary.");
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
                sut.WhenNavigatedFrom(new NavigationParameter());

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
                var result = Should.Throw<KeyNotFoundException>(() => sut.WhenNavigatedFrom(new NavigationParameter { { "hello", "world" } }).Subscribe());

                // Then
                result.Message.ShouldBe("The given key 'life' was not present in the dictionary.");
            }
        }
    }
}
