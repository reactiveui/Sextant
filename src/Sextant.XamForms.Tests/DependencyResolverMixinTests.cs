// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using ReactiveUI;
using Sextant.Mocks;
using Sextant.XamForms;
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
        /// Tests the register navigation method.
        /// </summary>
        public sealed class RegisterNavigation
        {
            /// <summary>
            /// Should register the view stack service.
            /// </summary>
            [Fact]
            public void Should_Register_Navigation_View()
            {
                // Given
                Locator.CurrentMutable.RegisterNavigationView(() => new NavigationView(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, ViewLocator.Current));

                // When
                var result = Locator.Current.GetService<IView>(DependencyResolverMixins.NavigationView);

                // Then
                result.ShouldBeOfType<NavigationView>();
            }
        }

        /// <summary>
        /// Tests the register view stack service method.
        /// </summary>
        public sealed class TheRegisterViewStackServiceMethod
        {
            /// <summary>
            /// Should register the view stack service.
            /// </summary>
            [Fact]
            public void Should_Register_View_Stack_Service()
            {
                // Given
                Locator.CurrentMutable.RegisterNavigationView();
                Locator.CurrentMutable.RegisterViewStackService();

                // When
                var result = Locator.Current.GetService<IViewStackService>();

                // Then
                result.ShouldBeOfType<ParameterViewStackService>();
            }

            /// <summary>
            /// Should register the view stack service factory.
            /// </summary>
            [Fact]
            public void Should_Register_View_Stack_Service_Factory()
            {
                // Given
                Locator.CurrentMutable.RegisterNavigationView();
                Locator.CurrentMutable.RegisterViewStackService<IViewStackService>(view => new ParameterViewStackService(view));

                // When
                var result = Locator.Current.GetService<IViewStackService>();

                // Then
                result.ShouldBeOfType<ParameterViewStackService>();
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
                Locator.CurrentMutable.RegisterView<PageView, NavigableViewModelMock>();

                // When
                var result = Locator.Current.GetService<IViewFor<NavigableViewModelMock>>();

                // Then
                result.ShouldBeOfType<PageView>();
            }

            /// <summary>
            /// Should register the view stack service factory.
            /// </summary>
            [Fact]
            public void Should_Register_View_Factory()
            {
                // Given
                Locator.CurrentMutable.RegisterView<PageView, NavigableViewModelMock>(() => new PageView());

                // When
                var result = Locator.Current.GetService<IViewFor<NavigableViewModelMock>>();

                // Then
                result.ShouldBeOfType<PageView>();
            }
        }
    }
}
