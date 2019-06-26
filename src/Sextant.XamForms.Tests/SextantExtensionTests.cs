using System;
using System.Collections.Generic;
using System.Text;
using Sextant.XamForms;
using Shouldly;
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
                result.ShouldBeOfType<NavigationView>();
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
                result.ShouldBeOfType<ParameterViewStackService>();
            }
        }
    }
}
