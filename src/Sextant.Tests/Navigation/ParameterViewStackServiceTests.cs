// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Sextant.Abstractions;
using Sextant.Mocks;
using Shouldly;
using Splat;
using Xunit;

namespace Sextant.Tests
{
    /// <summary>
    /// Tests the <see cref="ParameterViewStackService"/>.
    /// </summary>
    public sealed class ParameterViewStackServiceTests
    {
        /// <summary>
        /// Tests the push page method that passes parameters.
        /// </summary>
        public class ThePushPageWithParameterMethod
        {
            /// <summary>
            /// Should the throw if view model null.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_View_Model_Null()
            {
                // Given
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                var result =
                    await Should
                        .ThrowAsync<ArgumentNullException>(async () => await sut.PushPage(null, navigationParameter))
                        .ConfigureAwait(false);

                // Then
                result.ParamName.ShouldBe("navigableViewModel");
            }

            /// <summary>
            /// Should the throw if view model null.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_Parameter_Null()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                var result =
                    await Should
                        .ThrowAsync<ArgumentNullException>(async () => await sut.PushPage(viewModel, null))
                        .ConfigureAwait(false);

                // Then
                result.ParamName.ShouldBe("parameter");
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_When_Navigating_To()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                await sut.PushPage(viewModel, navigationParameter);

                // Then
                viewModel.Received().WhenNavigatingTo(Arg.Any<INavigationParameter>());
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_When_Navigated_To()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                await sut.PushPage(viewModel, navigationParameter);

                // Then
                viewModel.Received().WhenNavigatedTo(Arg.Any<INavigationParameter>());
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Not_Call_When_Navigated_From()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                await sut.PushPage(viewModel, navigationParameter);

                // Then
                viewModel.DidNotReceive().WhenNavigatedFrom(Arg.Any<INavigationParameter>());
            }
        }

        /// <summary>
        /// Tests the push page generic method that passes parameters.
        /// </summary>
        public class ThePushPageGenericWithParameterMethod
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ThePushPageGenericWithParameterMethod"/> class.
            /// </summary>
            public ThePushPageGenericWithParameterMethod()
            {
                Locator.CurrentMutable.Register(() => new DefaultViewModelFactory(), typeof(IViewModelFactory));
                Locator.CurrentMutable.Register(() => new NavigableViewModelMock());
            }

            /// <summary>
            /// Should the throw if view model null.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_View_Model_Null()
            {
                // Given
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                var result =
                    await Should
                        .ThrowAsync<ArgumentNullException>(async () => await sut.PushPage<NavigableViewModelMock>(navigationParameter))
                        .ConfigureAwait(false);

                // Then
                result.ParamName.ShouldBe("navigableViewModel");
            }

            /// <summary>
            /// Should the throw if view model null.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_Parameter_Null()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                var result =
                    await Should
                        .ThrowAsync<ArgumentNullException>(async () => await sut.PushPage<NavigableViewModelMock>(null))
                        .ConfigureAwait(false);

                // Then
                result.ParamName.ShouldBe("parameter");
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_When_Navigating_To()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                await sut.PushPage<NavigableViewModelMock>(navigationParameter);

                // Then
                viewModel.Received().WhenNavigatingTo(Arg.Any<INavigationParameter>());
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_When_Navigated_To()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                await sut.PushPage<NavigableViewModelMock>(navigationParameter);

                // Then
                viewModel.Received().WhenNavigatedTo(Arg.Any<INavigationParameter>());
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Not_Call_When_Navigated_From()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                await sut.PushPage<NavigableViewModelMock>(navigationParameter);

                // Then
                viewModel.DidNotReceive().WhenNavigatedFrom(Arg.Any<INavigationParameter>());
            }
        }

        /// <summary>
        /// Tests the push modal page method that passes parameters.
        /// </summary>
        public class ThePushModalGenericWithParameterMethod
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ThePushModalGenericWithParameterMethod"/> class.
            /// </summary>
            public ThePushModalGenericWithParameterMethod()
            {
                Locator.CurrentMutable.Register(() => new DefaultViewModelFactory(), typeof(IViewModelFactory));
                Locator.CurrentMutable.Register(() => new NavigableViewModelMock());
            }

            /// <summary>
            /// Should the throw if view model null.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_View_Model_Null()
            {
                // Given
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                var result =
                    await Should
                        .ThrowAsync<ArgumentNullException>(async () => await sut.PushModal<NavigableViewModelMock>(navigationParameter))
                        .ConfigureAwait(false);

                // Then
                result.ParamName.ShouldBe("navigableViewModel");
            }

            /// <summary>
            /// Should the throw if view model null.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_Parameter_Null()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                var result =
                    await Should
                        .ThrowAsync<ArgumentNullException>(async () => await sut.PushModal<NavigableViewModelMock>(null))
                        .ConfigureAwait(false);

                // Then
                result.ParamName.ShouldBe("parameter");
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_When_Navigating_To()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                await sut.PushModal<NavigableViewModelMock>(navigationParameter);

                // Then
                viewModel.Received().WhenNavigatingTo(Arg.Any<INavigationParameter>());
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_When_Navigated_To()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                await sut.PushModal<NavigableViewModelMock>(navigationParameter);

                // Then
                viewModel.Received().WhenNavigatedTo(Arg.Any<INavigationParameter>());
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Not_Call_When_Navigated_From()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                await sut.PushModal<NavigableViewModelMock>(navigationParameter);

                // Then
                viewModel.DidNotReceive().WhenNavigatedFrom(Arg.Any<INavigationParameter>());
            }
        }

        /// <summary>
        /// Tests the push modal method that passes parameters.
        /// </summary>
        public class ThePushModalWithParameterMethod
        {
            /// <summary>
            /// Should the throw if view model null.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_View_Model_Null()
            {
                // Given
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                var result =
                    await Should
                        .ThrowAsync<ArgumentNullException>(async () => await sut.PushModal(null, navigationParameter))
                        .ConfigureAwait(false);

                // Then
                result.ParamName.ShouldBe("modal");
            }

            /// <summary>
            /// Should the throw if view model null.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_Parameter_Null()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                var result =
                    await Should
                        .ThrowAsync<ArgumentNullException>(async () => await sut.PushModal(viewModel, null))
                        .ConfigureAwait(false);

                // Then
                result.ParamName.ShouldBe("parameter");
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_View()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                var view = Substitute.For<IView>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(view);

                // When
                await sut.PushModal(viewModel, navigationParameter);

                // Then
                view.Received().PushModal(viewModel, Arg.Any<string>());
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_When_Navigating_To()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                await sut.PushModal(viewModel, navigationParameter);

                // Then
                viewModel.Received().WhenNavigatingTo(Arg.Any<INavigationParameter>());
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_When_Navigated_To()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                await sut.PushModal(viewModel, navigationParameter);

                // Then
                viewModel.Received().WhenNavigatedTo(Arg.Any<INavigationParameter>());
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Not_Call_When_Navigated_From()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                await sut.PushModal(viewModel, navigationParameter);

                // Then
                viewModel.DidNotReceive().WhenNavigatedFrom(Arg.Any<INavigationParameter>());
            }
        }

        /// <summary>
        /// Tests the pop page method that passes parameters.
        /// </summary>
        public class ThePopPageWithParameterMethod
        {
            /// <summary>
            /// Should the throw if view model null.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_Parameter_Null()
            {
                // Given
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                var result =
                    await Should
                    .ThrowAsync<ArgumentNullException>(async () => await sut.PopPage(null))
                    .ConfigureAwait(false);

                // Then
                result.ParamName.ShouldBe("parameter");
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_View()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                var view = Substitute.For<IView>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(view).WithPushed(viewModel);

                // When
                await sut.PopPage(navigationParameter);

                // Then
                view.Received().PopPage(Arg.Any<bool>());
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Not_Call_When_Navigated_To()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithPushed(viewModel);

                // When
                await sut.PopPage(navigationParameter);

                // Then
                viewModel.DidNotReceive().WhenNavigatedTo(Arg.Any<INavigationParameter>());
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_When_Navigated_From()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithPushed(viewModel);

                // When
                await sut.PopPage(navigationParameter);

                // Then
                viewModel.Received().WhenNavigatedFrom(Arg.Any<INavigationParameter>());
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Not_Call_When_Navigating_To()
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithPushed(viewModel);

                // When
                await sut.PopPage(navigationParameter);

                // Then
                viewModel.DidNotReceive().WhenNavigatingTo(Arg.Any<INavigationParameter>());
            }
        }
    }
}
