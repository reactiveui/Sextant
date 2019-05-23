using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using ReactiveUI;
using Sextant.Mocks;
using Shouldly;
using Splat;
using Xunit;

namespace Sextant.XamForms.Tests
{
    /// <summary>
    /// Tests the IMutableDependencyResolver extension class.
    /// </summary>
    public sealed class DependencyResolverMixinTests
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
                Locator.CurrentMutable.InitializeSextant();

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
                Locator.CurrentMutable.InitializeSextant();

                // When
                var result = Locator.Current.GetService<IViewStackService>();

                // Then
                result.ShouldBeOfType<ViewStackService>();
            }
        }

        /// <summary>
        /// Tests the register navigation view method.
        /// </summary>
        public sealed class TheRegisterNavigationViewMethod
        {
            /// <summary>
            /// Should register the navigation view.
            /// </summary>
            [Fact]
            public void Should_Register_Navigation_View()
            {
                // Given
                Locator.CurrentMutable.RegisterNavigationView();

                // When
                var result = Locator.Current.GetService<IView>(DependencyResolverMixins.NavigationView);

                // Then
                result.ShouldBeOfType<NavigationView>();
            }

            /// <summary>
            /// Should register the navigation view.
            /// </summary>
            [Fact]
            public void Should_Register_Navigation_View_With_Schedulers()
            {
                // Given
                Locator.CurrentMutable.RegisterNavigationView(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler);

                // When
                var result = Locator.Current.GetService<IView>(DependencyResolverMixins.NavigationView);

                // Then
                result.ShouldBeOfType<NavigationView>();
            }
        }

        /// <summary>
        /// Tests the register view stack service method.
        /// </summary>
        public sealed class TheViewStackServiceMethod
        {
            /// <summary>
            /// Should register the view stack service.
            /// </summary>
            [Fact]
            public void Should_Register_View_Stack_Service()
            {
                // Given
                Locator.CurrentMutable.RegisterViewStackService();

                // When
                var result = Locator.Current.GetService<IViewStackService>();

                // Then
                result.ShouldBeOfType<ViewStackService>();
            }

            /// <summary>
            /// Should register the view stack service factory.
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

        /// <summary>
        /// Tests the register view method.
        /// </summary>
        public sealed class TheRegisterViewMethod
        {
            /// <summary>
            /// Should register the view stack service.
            /// </summary>
            [Fact]
            public void Should_Register_View()
            {
                // Given
                Locator.CurrentMutable.RegisterViewStackService();

                // When
                var result = Locator.Current.GetService<IViewStackService>();

                // Then
                result.ShouldBeOfType<ViewStackService>();
            }

            /// <summary>
            /// Should register the view stack service factory.
            /// </summary>
            [Fact]
            public void Should_Register_View_Factory()
            {
                // Given
                Locator.CurrentMutable.RegisterView<PageView, PageViewModelMock>(() => new PageView());

                // When
                var result = Locator.Current.GetService<IViewStackService>();

                // Then
                result.ShouldBeOfType<ViewStackService>();
            }
        }
    }
}
