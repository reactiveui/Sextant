using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;
using Splat;
using Xunit;

namespace Sextant.XamForms.Tests
{
    /// <summary>
    /// Tests the Sextant static class.
    /// </summary>
    public sealed class SextantTests
    {
        /// <summary>
        /// Tests the Sextant Instance property.
        /// </summary>
        public sealed class TheInstanceProperty
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TheInstanceProperty"/> class.
            /// </summary>
            public TheInstanceProperty()
            {
            }

            /// <summary>
            /// Tests the navigation view is registered.
            /// </summary>
            [Fact]
            public void Should_Not_Register_Navigation_View()
            {
                // Given
                var instance = Sextant.Instance;

                // When
                var result = Locator.Current.GetService<IView>(nameof(NavigationView));

                // Then
                result.ShouldBeOfType<NavigationView>();
            }

            /// <summary>
            /// Tests the navigation view is registered.
            /// </summary>
            [Fact]
            public void Should_Not_Register_View_Stack_Service()
            {
                // Given
                var instance = Sextant.Instance;

                // When
                var result = Locator.Current.GetService<IViewStackService>();

                // Then
                result.ShouldBeOfType<ViewStackService>();
            }
        }
    }
}
