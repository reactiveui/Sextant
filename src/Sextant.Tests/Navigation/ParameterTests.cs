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
    /// Test the <see cref="NavigationParameter"/> class.
    /// </summary>
    public sealed class ParameterTests
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
                sut.WhenNavigatingTo(new NavigationParameter { { "hello", "world" } });

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
                sut.WhenNavigatingTo(new NavigationParameter { });

                // Then
                sut.Text.ShouldBeNull();
            }
        }

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
                sut.WhenNavigatedTo(new NavigationParameter { { "hello", "world" }, { "life", 42 } });

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
                sut.WhenNavigatedTo(new NavigationParameter { });

                // Then
                sut.Text.ShouldBeNull();
            }
        }
    }
}
