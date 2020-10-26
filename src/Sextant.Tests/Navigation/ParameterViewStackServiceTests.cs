// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Sextant.Mocks;
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
        /// Tests the construction of the object.
        /// </summary>
        public class TheConstructor
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="TheConstructor"/> class.
            /// </summary>
            public TheConstructor()
            {
                Locator.GetLocator().UnregisterAll<IViewModelFactory>();
            }

            /// <summary>
            /// Test that the object constructed uses the static instance of ViewModelFactory.
            /// </summary>
            [Fact]
            public void Should_Throw_If_View_Model_Factory_Current_Null()
            {
                // Given, When
                var result = Record.Exception(() => new ParameterViewStackServiceFixture().WithFactory(null));

                // Then
                result.Should().BeOfType<ViewModelFactoryNotFoundException>();
            }

            /// <summary>
            /// Test that the object constructed uses the static instance of ViewModelFactory.
            /// </summary>
            [Fact]
            public void Should_Resolve_View_Model_Factory()
            {
                // Given
                Locator.CurrentMutable.RegisterViewModelFactory();

                // When
                var result = Record.Exception(() => new ParameterViewStackServiceFixture().WithFactory(null));

                // Then
                result.Should().BeNull();
            }
        }

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
                    await Record.ExceptionAsync(async () => await sut.PushPage(null, navigationParameter))
                        .ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("navigableViewModel");
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
                    await Record.ExceptionAsync(async () => await sut.PushPage(viewModel, (INavigationParameter)null))
                        .ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("parameter");
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
                await viewModel.Received().WhenNavigatingTo(navigationParameter);
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
                await viewModel.Received().WhenNavigatedTo(navigationParameter);
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
                await viewModel.DidNotReceive().WhenNavigatedFrom(navigationParameter);
            }

            /// <summary>
            /// Tests to make sure we receive a navigation methods in the correct order.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_In_Order()
            {
                // Given
                var view = Substitute.For<IView>();
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(view);

                // When
                await sut.PushPage(viewModel, navigationParameter);

                // Then
                Received.InOrder(() =>
                {
                    viewModel.WhenNavigatingTo(Arg.Any<INavigationParameter>());
                    view.PushPage(Arg.Any<IViewModel>(), null, Arg.Any<bool>(), Arg.Any<bool>());
                    viewModel.WhenNavigatedTo(Arg.Any<INavigationParameter>());
                });
            }

            /// <summary>Tests to make sure we receive a push page notification.</summary>
            /// <param name="contract">The contract.</param>
            /// <param name="reset">Reset the stack.</param>
            /// <param name="animate">Animate the navigation.</param>
            /// <returns>A completion notification.</returns>
            [Theory]
            [InlineData(null, false, true)]
            [InlineData(null, true, false)]
            [InlineData("hello", true, true)]
            [InlineData("hello", true, false)]
            [InlineData("hello", false, true)]
            [InlineData("hello", false, false)]
            public async Task Should_Call_View(string contract, bool reset, bool animate)
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var view = Substitute.For<IView>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(view);

                // When
                await sut.PushPage(viewModel, navigationParameter, contract, reset, animate);

                // Then
                await view.Received().PushPage(viewModel, contract, reset, animate);
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
                Locator.CurrentMutable.UnregisterAll<NavigableViewModelMock>();
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
                    await Record.ExceptionAsync(async () => await sut.PushPage<NullViewModelMock>(navigationParameter))
                        .ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("navigableViewModel");
            }

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
                    await Record.ExceptionAsync(async () => await sut.PushPage<NavigableViewModelMock>(null))
                        .ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("parameter");
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <param name="contract">The contract.</param>
            /// <param name="reset">Reset the stack.</param>
            /// <param name="animate">Animate the navigation.</param>
            /// <returns>A completion notification.</returns>
            [Theory]
            [InlineData(null, false, true)]
            [InlineData(null, true, false)]
            [InlineData("hello", true, true)]
            [InlineData("hello", true, false)]
            [InlineData("hello", false, true)]
            [InlineData("hello", false, false)]
            public async Task Should_Call_View(string contract, bool reset, bool animate)
            {
                // Given
                var view = Substitute.For<IView>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(view);

                // When
                await sut.PushPage<NavigableViewModelMock>(navigationParameter, contract, reset, animate);

                // Then
                await view.Received().PushPage(Arg.Any<NavigableViewModelMock>(), contract, reset, animate);
            }

            /// <summary>
            /// Tests the view model factory is called.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_ViewModel_Factory()
            {
                // Given
                var factory = Substitute.For<IViewModelFactory>();
                factory.Create<NavigableViewModelMock>().Returns(new NavigableViewModelMock());
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithFactory(factory);

                // When
                await sut.PushPage<NavigableViewModelMock>(navigationParameter);

                // Then
                factory.Received().Create<NavigableViewModelMock>();
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
                    await Record.ExceptionAsync(async () => await sut.PushModal(null, navigationParameter))
                        .ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("navigableModal");
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
                    await Record.ExceptionAsync(async () => await sut.PushModal(viewModel, null))
                        .ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("parameter");
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <param name="contract">The contract.</param>
            /// <param name="withNavigation">Wrap model in navigation page.</param>
            /// <returns>A completion notification.</returns>
            [Theory]
            [InlineData(null, false)]
            [InlineData(null, true)]
            [InlineData("hello", true)]
            [InlineData("hello", false)]
            public async Task Should_Call_View(string contract, bool withNavigation)
            {
                // Given
                var viewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                var view = Substitute.For<IView>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(view);

                // When
                await sut.PushModal(viewModel, navigationParameter, contract, withNavigation);

                // Then
                await view.Received().PushModal(viewModel, contract, withNavigation);
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
                await viewModel.Received().WhenNavigatingTo(navigationParameter);
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
                await viewModel.Received().WhenNavigatedTo(navigationParameter);
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
                await viewModel.DidNotReceive().WhenNavigatedFrom(navigationParameter);
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
                Locator.CurrentMutable.UnregisterAll<NavigableViewModelMock>();
                Locator.CurrentMutable.UnregisterAll<IViewModelFactory>();
                Locator.CurrentMutable.Register(() => new DefaultViewModelFactory(), typeof(IViewModelFactory));
                Locator.CurrentMutable.Register(() => new NavigableViewModelMock());
                Locator.CurrentMutable.UnregisterAll<NullViewModelMock>();
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
                    await Record.ExceptionAsync(async () => await sut.PushModal<NullViewModelMock>(navigationParameter))
                        .ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("navigableModal");
            }

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
                    await Record.ExceptionAsync(async () => await sut.PushModal<NavigableViewModelMock>(null))
                        .ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("parameter");
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <param name="contract">The contract.</param>
            /// <param name="withNavigation">Wrap model in navigation page.</param>
            /// <returns>A completion notification.</returns>
            [Theory]
            [InlineData(null, false)]
            [InlineData(null, true)]
            [InlineData("hello", true)]
            [InlineData("hello", false)]
            public async Task Should_Call_View(string contract, bool withNavigation)
            {
                // Given
                var view = Substitute.For<IView>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(view);

                // When
                await sut.PushModal<NavigableViewModelMock>(navigationParameter, contract, withNavigation);

                // Then
                await view.Received().PushModal(Arg.Any<NavigableViewModelMock>(), contract, withNavigation);
            }

            /// <summary>
            /// Tests the view model factory is called.
            /// </summary>
            /// <param name="contract">The contract.</param>
            /// <returns>A completion notification.</returns>
            [Theory]
            [InlineData("")]
            [InlineData(null)]
            [InlineData("contract")]
            public async Task Should_Call_ViewModel_Factory(string? contract)
            {
                // Given
                var factory = Substitute.For<IViewModelFactory>();
                factory.Create<NavigableViewModelMock>(Arg.Any<string?>()).Returns(new NavigableViewModelMock());
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithFactory(factory);

                // When
                await sut.PushModal<NavigableViewModelMock>(navigationParameter, contract);

                // Then
                factory.Received().Create<NavigableViewModelMock>(contract);
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
                    await Record.ExceptionAsync(async () => await sut.PopPage(null))
                    .ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("parameter");
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
                await view.Received().PopPage(Arg.Any<bool>());
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Destroy()
            {
                // Given
                var viewModel = Substitute.For<IEverything>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(new NavigationViewMock());
                await sut.PushPage(viewModel);
                await sut.PushPage(viewModel);

                // When
                await sut.PopPage(navigationParameter);

                // Then
                viewModel.Received(1).Destroy();
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Not_Call_When_Navigated_To()
            {
                // Given
                var firstViewModel = Substitute.For<INavigable>();
                var secondViewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(new NavigationViewMock());

                // When
                await sut.PushPage(firstViewModel);
                await sut.PushPage(secondViewModel);
                await sut.PopPage(navigationParameter);

                // Then
                await secondViewModel.DidNotReceive().WhenNavigatedTo(navigationParameter);
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_When_Navigated_To()
            {
                // Given
                var firstViewModel = Substitute.For<INavigable>();
                var secondViewModel = Substitute.For<INavigable>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(new NavigationViewMock());

                // When
                await sut.PushPage(firstViewModel, navigationParameter);
                await sut.PushPage(secondViewModel);
                await sut.PopPage(navigationParameter);

                // Then
                await firstViewModel.Received(2).WhenNavigatedTo(Arg.Any<INavigationParameter>());
                await secondViewModel.Received().WhenNavigatedFrom(navigationParameter);
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
                await viewModel.Received().WhenNavigatedFrom(navigationParameter);
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
                await viewModel.DidNotReceive().WhenNavigatingTo(navigationParameter);
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_In_Order()
            {
                // Given
                var viewModel1 = Substitute.For<IEverything>();
                var viewModel2 = Substitute.For<IEverything>();
                var subject = new Subject<IViewModel>();
                var navigationParameter = Substitute.For<INavigationParameter>();
                var view = Substitute.For<IView>();

                view.When(x => x.PopPage())
                    .Do(_ => subject.OnNext(viewModel2));
                view
                    .PagePopped
                    .Returns(subject.AsObservable());
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(view);
                await sut.PushPage(viewModel1);
                await sut.PushPage(viewModel2);

                // When
                await sut.PopPage(navigationParameter);

                // Then
                Received.InOrder(() =>
                {
                    view.PopPage(Arg.Any<bool>());
                    viewModel2.WhenNavigatedFrom(Arg.Any<INavigationParameter>());
                    viewModel2.Destroy();
                });
            }
        }
    }
}
