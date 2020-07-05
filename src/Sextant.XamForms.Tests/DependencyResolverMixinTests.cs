// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;
using ReactiveUI;
using Sextant.Abstractions;
using Sextant.Mocks;
using Sextant.XamForms;
using Shouldly;
using Splat;
using Xamarin.Forms;
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
        public sealed class TheRegisterNavigationViewFunctionMethod
        {
            /// <summary>
            /// Should register the view stack service.
            /// </summary>
            [Fact]
            public void Should_Register_Navigation_View()
            {
                // Given
                Locator.CurrentMutable.RegisterViewModelFactory();
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
            /// Initializes a new instance of the <see cref="TheRegisterViewStackServiceMethod"/> class.
            /// </summary>
            public TheRegisterViewStackServiceMethod()
            {
                Locator.CurrentMutable.UnregisterAll<IView>();
                Locator.CurrentMutable.UnregisterAll<IViewStackService>();
                Locator.CurrentMutable.UnregisterAll<IViewModelFactory>();
                Locator.CurrentMutable.UnregisterAll<IParameterViewStackService>();
            }

            /// <summary>
            /// Should register the view stack service.
            /// </summary>
            [Fact]
            public void Should_Register_View_Stack_Service()
            {
                // Given
                Locator.CurrentMutable.RegisterNavigationView();
                Locator.CurrentMutable.RegisterViewModelFactory();
                Locator.CurrentMutable.RegisterViewStackService();

                // When
                var result = Locator.Current.GetService<IViewStackService>();

                // Then
                result.ShouldBeOfType<ParameterViewStackService>();
            }

            /// <summary>
            /// Tests the view stack service overload.
            /// </summary>
            [Fact]
            public void Should_Register_View_Stack_Service_With_View()
            {
                // Given
                Locator.CurrentMutable.RegisterNavigationView();
                Locator.CurrentMutable.RegisterViewModelFactory();
                Locator.CurrentMutable.RegisterViewStackService<IViewStackService>(view => new ParameterViewStackService(view));

                // When
                var result = Locator.Current.GetService<IViewStackService>();

                // Then
                result.ShouldBeOfType<ParameterViewStackService>();
            }

            /// <summary>
            /// Tests the view stack service overload.
            /// </summary>
            [Fact]
            public void Should_Register_View_Stack_Service_With_Factory()
            {
                // Given
                Locator.CurrentMutable.RegisterNavigationView();
                Locator.CurrentMutable.RegisterViewModelFactory();
                Locator.CurrentMutable.RegisterViewStackService<IViewStackService>((view, factory) => new ParameterViewStackService(view, factory));

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

        /// <summary>
        /// Tests the get navigation view method.
        /// </summary>
        public sealed class TheGetNavigationView
        {
            /// <summary>
            /// Should register the view stack service.
            /// </summary>
            [Fact]
            public void Should_Return_Navigation_View()
            {
                // Given
                Locator.CurrentMutable.RegisterView<PageView, NavigableViewModelMock>();

                // When
                var result = Locator.Current.GetNavigationView();

                // Then
                result.ShouldBeAssignableTo<IView>();
                result.ShouldBeAssignableTo<Page>();
                result.ShouldBeAssignableTo<NavigationPage>();
            }
        }
    }
}
