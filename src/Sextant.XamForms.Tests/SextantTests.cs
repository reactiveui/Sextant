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
        /// Tests the Sextant Initalize method.
        /// </summary>
        public sealed class TheInitializeMethod
        {
            /// <summary>
            /// Tests the navigation view is registered.
            /// </summary>
            [Fact]
            public void Should_Register_Navigation_View()
            {
                // Given
                Sextant.Instance.Initialize();

                // When
                var result = Locator.Current.GetService<IView>(nameof(NavigationView));

                // Then
                result.ShouldBeOfType<NavigationView>();
            }

            /// <summary>
            /// Tests the navigation view is registered.
            /// </summary>
            [Fact]
            public void Should_Register_View_Stack_Service()
            {
                // Given
                Sextant.Instance.Initialize();

                // When
                var result = Locator.Current.GetService<IViewStackService>();

                // Then
                result.ShouldBeOfType<ViewStackService>();
            }
        }
    }
}
