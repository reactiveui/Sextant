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
            /// Initializes a new instance of the <see cref="TheInitializeMethod"/> class.
            /// </summary>
            public TheInitializeMethod()
            {
                Locator.CurrentMutable.InitializeSplat();
            }

            /// <summary>
            /// Tests the navigation view is registered.
            /// </summary>
            [Fact]
            public void Should_Register_Navigation_View()
            {
                // Given
                Sextant.Initialize();

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
                Sextant.Initialize();

                // When
                var result = Locator.Current.GetService<IViewStackService>();

                // Then
                result.ShouldBeOfType<ViewStackService>();
            }
        }

        /// <summary>
        /// Tests the navigation view is registered.
        /// </summary>
        public sealed class TheRegisterViewStackServiceFactoryMethod
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TheRegisterViewStackServiceFactoryMethod"/> class.
            /// </summary>
            public TheRegisterViewStackServiceFactoryMethod()
            {
                Locator.CurrentMutable.InitializeSplat();
                Locator.CurrentMutable.RegisterNavigationView();
            }

            /// <summary>
            /// Tests the navigation view is registered.
            /// </summary>
            [Fact]
            public void Should_Register_View_Stack_Service_Factory()
            {
                // Given
                Locator.CurrentMutable.RegisterViewStackService<IViewStackService>(view => new ViewStackService(view));

                // When
                var result = Locator.Current.GetService<IViewStackService>();

                // Then
                result.ShouldBeOfType<ViewStackService>();
            }
        }
    }
}
