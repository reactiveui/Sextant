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
        /// Tests the initialization method.
        /// </summary>
        public sealed class TheInitializeSextantMethod
        {
            /// <summary>
            /// Should register the navigation view.
            /// </summary>
            [Fact]
            public void Should_Register_Navigation_View()
            {
                // Given
                Sextant.Instance.Initialize();

                // When
                var result = Locator.Current.GetService<IView>(DependencyResolverMixins.NavigationView);

                // Then
                result.ShouldBeOfType<NavigationView>();
            }

            /// <summary>
            /// Should register the view stack service.
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
