// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Immutable;
using System.Reactive;
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
    /// Tests that check to make sure that the view stack works correctly.
    /// </summary>
    public sealed class ViewStackServiceTests
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
                var result = Record.Exception(() => (ViewStackService)new ViewStackServiceFixture().WithFactory(null));

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
                var result = Record.Exception(() => (ViewStackService)new ViewStackServiceFixture().WithFactory(null));

                // Then
                result.Should().BeNull();
            }
        }

        /// <summary>
        /// Tests associated with the page stack property.
        /// </summary>
        public class ThePageStackProperty
        {
            /// <summary>
            /// Tests to verify the return value is an observable type.
            /// </summary>
            [Fact]
            public void Should_Return_Observable()
            {
                // Given, When
                ViewStackService sut = new ViewStackServiceFixture();

                // Then
                sut.PageStack.Should().NotBeOfType<BehaviorSubject<IImmutableList<IViewModel>>>();
            }

            /// <summary>
            /// Tests to verify the return value is not a subject type.
            /// </summary>
            [Fact]
            public void Should_Not_Return_Behavior()
            {
                // Given, When
                ViewStackService sut = new ViewStackServiceFixture();

                // Then
                sut.PageStack.Should().NotBeOfType<BehaviorSubject<IImmutableList<IViewModel>>>();
            }
        }

        /// <summary>
        /// Tests associated with the modal stack property.
        /// </summary>
        public class TheModalStackProperty
        {
            /// <summary>
            /// Tests to verify the return value is an observable type.
            /// </summary>
            [Fact]
            public void Should_Return_Observable()
            {
                // Given, When
                ViewStackService sut = new ViewStackServiceFixture();

                // Then
                sut.ModalStack.Should().NotBeOfType<BehaviorSubject<IImmutableList<IViewModel>>>();
            }

            /// <summary>
            /// Tests to verify the return value is not a subject type.
            /// </summary>
            [Fact]
            public void Should_Not_Return_Behavior()
            {
                // Given, When
                ViewStackService sut = new ViewStackServiceFixture();

                // Then
                sut.ModalStack.Should().NotBeOfType<BehaviorSubject<IImmutableList<IViewModel>>>();
            }
        }

        /// <summary>
        /// Tests associated with the pop model methods.
        /// </summary>
        public class ThePopModalMethod
        {
            /// <summary>
            /// Checks to make sure that the pop modal method works correctly.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Pop_Modal()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();
                await sut.PushModal(new NavigableViewModelMock());

                // When
                var item = await sut.ModalStack.FirstAsync();
                item.Count.Should().Be(1);
                await sut.PopModal();

                // Then
                item = await sut.ModalStack.FirstAsync();
                item.Should().BeEmpty();
            }

            /// <summary>
            /// Checks to make sure that the pop modal observables are received.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Receive_Pop_Modal()
            {
                // Given, When
                ViewStackService sut = new ViewStackServiceFixture();
                await sut.PushModal(new NavigableViewModelMock());

                // When
                await sut.PopModal();

                // Then
                await sut.View.Received().PopModal();
            }

            /// <summary>
            /// Checks to make sure that the pop returns a <see cref="Unit"/>.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Return_Unit()
            {
                // Given, When
                ViewStackService sut = new ViewStackServiceFixture();
                await sut.PushModal(new NavigableViewModelMock());

                // When
                var result = await sut.PopModal();

                // Then
                result.Should().BeOfType<Unit>();
            }

            /// <summary>
            /// Checks to make sure that there is a exception thrown if the stack happens to be empty.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_Stack_Empty()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await sut.PopModal()).ConfigureAwait(false);

                // Then
                result.Should().BeOfType<InvalidOperationException>();
                result?.Message.Should().Be("Stack is empty.");
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
                var view = Substitute.For<IView>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(view).WithModal(viewModel);

                // When
                await sut.PopModal();

                // Then
                viewModel.Received(1).Destroy();
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
                var view = Substitute.For<IView>();

                view.When(x => x.PopPage())
                    .Do(_ => subject.OnNext(viewModel2));
                view
                    .PagePopped
                    .Returns(subject.AsObservable());
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(view);
                await sut.PushModal(viewModel1);
                await sut.PushModal(viewModel2);

                // When
                await sut.PopModal();

                // Then
                Received.InOrder(() =>
                {
                    view.PopModal();
                    viewModel2.Destroy();
                });
            }
        }

        /// <summary>
        /// Tests associated with the pop page methods.
        /// </summary>
        public class ThePopPageMethod
        {
            /// <summary>
            /// Checks to make sure that the pop page works.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Pop_Page()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture().WithView(new NavigationViewMock());
                await sut.PushPage(new NavigableViewModelMock());

                // When
                await sut.PopPage();
                var result = await sut.PageStack.FirstAsync();

                // Then
                result.Should().BeEmpty();
            }

            /// <summary>
            /// Checks to make sure that the pop page observables are received.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Receive_Pop_Page()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();
                await sut.PushPage(new NavigableViewModelMock());

                // When
                await sut.PopPage();

                // Then
                await sut.View.Received().PopPage();
            }

            /// <summary>
            /// Checks to make sure that the pop returns a <see cref="Unit"/>.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Return_Unit()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();
                await sut.PushPage(new NavigableViewModelMock());

                // When
                var result = await sut.PopPage();

                // Then
                result.Should().BeOfType<Unit>();
            }

            /// <summary>
            /// Tests to make sure we destroy the view model.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Destroy()
            {
                // Given
                var viewModel1 = Substitute.For<IEverything>();
                var viewModel2 = Substitute.For<IEverything>();
                var view = new NavigationViewMock();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(view);
                await sut.PushPage(viewModel1);
                await sut.PushPage(viewModel2);

                // When
                await sut.PopPage();

                // Then
                viewModel2.Received(1).Destroy();
            }

            /// <summary>
            /// Tests to make sure we receive a event notifications in order.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_In_Order()
            {
                // Given
                var viewModel1 = Substitute.For<IEverything>();
                var viewModel2 = Substitute.For<IEverything>();
                var subject = new Subject<IViewModel>();
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
                await sut.PopPage();

                // Then
                Received.InOrder(() =>
                {
                    view.PopPage();
                    viewModel2.Destroy();
                });
            }
        }

        /// <summary>
        /// Tests for the pop to root method.
        /// </summary>
        public class ThePopToRootPageMethod
        {
            /// <summary>
            /// Tests to verify that no exception is thrown if the stack happens to be empty.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_Stack_Empty()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await sut.PopToRootPage()).ConfigureAwait(false);

                // Then
                result.Should().BeOfType<InvalidOperationException>().Which.Message.Should().Be("Stack is empty.");
            }

            /// <summary>
            /// Tests to verify the navigatino stack is cleared.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Clear_Navigation_Stack()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();
                await sut.PushPage(new NavigableViewModelMock(), pages: 3);

                // When
                await sut.PopToRootPage();
                var result = await sut.PageStack.FirstOrDefaultAsync();

                // Then
                result.Should().ContainSingle();
            }

            /// <summary>
            /// Tests to verify the navigatino stack is cleared.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Return_Single_Notification()
            {
                // Given
                int count = 0;
                ViewStackService sut = new ViewStackServiceFixture();
                await sut.PushPage(new NavigableViewModelMock(), pages: 3);

                sut.View.PagePopped.Subscribe(_ =>
                {
                    count++;
                });

                // When
                await sut.PopToRootPage();

                // Then
                count.Should().Be(1);
            }

            /// <summary>
            /// Tests to verify the navigation stack has an element left.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Have_One_Item_On_Stack()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();
                await sut.PushPage(new NavigableViewModelMock(), pages: 3);
                await sut.PopToRootPage();

                // When
                var result = await sut.PageStack.FirstOrDefaultAsync();

                // Then
                result.Should().ContainSingle();
            }

            /// <summary>
            /// Tests to make sure we destroy the view model.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Destroy()
            {
                // Given
                var viewModel1 = Substitute.For<IEverything>();
                var viewModel2 = Substitute.For<IEverything>();
                var viewModel3 = Substitute.For<IEverything>();
                var view = Substitute.For<IView>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(view);
                await sut.PushPage(viewModel1);
                await sut.PushPage(viewModel2);
                await sut.PushPage(viewModel3);

                // When
                await sut.PopToRootPage();

                // Then
                Received.InOrder(() =>
                {
                    viewModel3.Destroy();
                    viewModel2.Destroy();
                });
            }
        }

        /// <summary>
        /// Tests for the push modal method.
        /// </summary>
        public class ThePushModalMethod
        {
            /// <summary>
            /// Makes sure that the push and pop methods work correctly.
            /// </summary>
            /// <param name="amount">The number of pages.</param>
            /// <returns>A completion notification.</returns>
            [Theory]
            [InlineData(1)]
            [InlineData(3)]
            [InlineData(5)]
            public async Task Should_Push_And_Pop(int amount)
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();
                await sut.PushModal(new NavigableViewModelMock(), "modal", amount);
                sut.ModalStack.FirstAsync().Wait().Count.Should().Be(amount);
                await sut.PopModal(amount);

                // When
                var result = await sut.ModalStack.FirstAsync();

                // Then
                result.Should().BeEmpty();
            }

            /// <summary>
            /// Tests to make sure that the push modal works.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Push_Modal()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();
                await sut.PushModal(new NavigableViewModelMock());

                // When
                var result = await sut.TopModal();

                // Then
                result.Should().NotBeNull();
                result.Should().BeOfType<NavigableViewModelMock>();
            }

            /// <summary>
            /// Tests to make sure that the push modal respects navigation.
            /// </summary>
            /// <param name="withNavigationPage">Whether to use a navigation page.</param>
            /// <returns>A completion notification.</returns>
            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public async Task Should_Push_Modal_Navigation_Page(bool withNavigationPage)
            {
                // Given
                var view = Substitute.For<IView>();
                ViewStackService sut = new ViewStackServiceFixture().WithView(view);

                // When
                await sut.PushModal(new NavigableViewModelMock(), withNavigationPage: withNavigationPage);

                // Then
                await view.Received().PushModal(Arg.Any<INavigable>(), Arg.Any<string>(), withNavigationPage);
            }

            /// <summary>
            /// Tests to make sure we can push a page onto the stack.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Push_Page_On_Stack()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();

                // When
                await sut.PushModal(new NavigableViewModelMock(), "modal");
                var result = await sut.ModalStack.FirstAsync();

                // Then
                result.Should().NotBeEmpty();
                result.Count.Should().Be(1);
            }

            /// <summary>
            /// Tests to make sure we receive an push modal notifications.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Receive_Push_Modal()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture().WithView(Substitute.For<IView>());

                // When
                await sut.PushModal(new NavigableViewModelMock(), "modal");

                // Then
                await sut.View.Received().PushModal(Arg.Any<IViewModel>(), "modal");
            }

            /// <summary>
            /// Tests to make sure that we get an exception throw if we pass in a null view model.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_View_Model_Null()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await sut.PushModal(null)).ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentNullException>();
            }
        }

        /// <summary>
        /// Tests the push modal page method that passes parameters.
        /// </summary>
        public class ThePushModalGenericMethod
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ThePushModalGenericMethod"/> class.
            /// </summary>
            public ThePushModalGenericMethod()
            {
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
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                var result =
                    await Record.ExceptionAsync(async () => await sut.PushModal<NullViewModelMock>())
                        .ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("modal");
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_View()
            {
                // Given
                var view = Substitute.For<IView>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(view);

                // When
                await sut.PushModal<NavigableViewModelMock>();

                // Then
                await view.Received().PushModal(Arg.Any<NavigableViewModelMock>(), Arg.Any<string>(), Arg.Any<bool>());
            }

            /// <summary>
            /// Tests that the ViewModelFactory is called.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_ViewModel_Factory()
            {
                // Given
                var factory = Substitute.For<IViewModelFactory>();
                factory.Create<NavigableViewModelMock>(Arg.Any<string?>()).Returns(new NavigableViewModelMock());
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithFactory(factory);

                // When
                await sut.PushModal<NavigableViewModelMock>(navigationParameter);

                // Then
                factory.Received().Create<NavigableViewModelMock>();
            }
        }

        /// <summary>
        /// Tests associated with the push page method.
        /// </summary>
        public class ThePushPageMethod
        {
            /// <summary>
            /// Tests to make sure that we get an exception throw if we pass in a null view model.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_View_Model_Null()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await sut.PushPage(null)).ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("viewModel");
            }

            /// <summary>
            /// Tests to make sure that the push page works.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Push_Page()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();

                // When
                await sut.PushPage(new NavigableViewModelMock());
                var result = await sut.TopPage();

                // Then
                result.Should().NotBeNull();
                result.Should().BeOfType<NavigableViewModelMock>();
            }

            /// <summary>
            /// Tests to make sure we can push a page onto the stack.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Push_Page_On_Stack()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();

                // When
                await sut.PushPage(new NavigableViewModelMock());
                var result = await sut.PageStack.FirstAsync();

                // Then
                result.Should().NotBeEmpty();
                result.Count.Should().Be(1);
            }

            /// <summary>
            /// Tests to make sure we receive an push page notifications.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Receive_Push_Page()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture().WithView(Substitute.For<IView>());

                // When
                await sut.PushPage(new NavigableViewModelMock());

                // Then
                await sut.View.Received().PushPage(Arg.Any<IViewModel>(), null, false, true);
            }

            /// <summary>
            /// Tests to make sure we receive an push page notifications.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Clear_Navigation_Stack_If_Reset()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture().WithView(Substitute.For<IView>());

                // When
                await sut.PushPage(new NavigableViewModelMock(), pages: 3);
                await sut.PushPage(new NavigableViewModelMock(), resetStack: true);
                var result = await sut.PageStack.FirstOrDefaultAsync();

                // Then
                result.Should().ContainSingle();
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
            /// Should the throw if view model is null.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_View_Model_Null()
            {
                // Given
                ParameterViewStackService sut = new ParameterViewStackServiceFixture();

                // When
                var result =
                    await Record.ExceptionAsync(async () => await sut.PushPage<NullViewModelMock>())
                        .ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentNullException>().Which.ParamName.Should().Be("viewModel");
            }

            /// <summary>
            /// Tests to make sure we receive a push page notification.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_View()
            {
                // Given
                var view = Substitute.For<IView>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(view);

                // When
                await sut.PushPage<NavigableViewModelMock>();

                // Then
                await view.Received().PushPage(Arg.Any<NavigableViewModelMock>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>());
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
                factory.Create<NavigableViewModelMock>(Arg.Any<string?>()).Returns(new NavigableViewModelMock());
                var navigationParameter = Substitute.For<INavigationParameter>();
                ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithFactory(factory);

                // When
                await sut.PushPage<NavigableViewModelMock>(navigationParameter);

                // Then
                factory.Received().Create<NavigableViewModelMock>();
            }
        }

        /// <summary>
        /// Tests for the TopModal method.
        /// </summary>
        public class TheTopModalMethod
        {
            /// <summary>
            /// Tests to make sure that it does not pop the stack.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Not_Pop()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture().WithView(Substitute.For<IView>());
                await sut.PushModal(new NavigableViewModelMock());

                // When
                await sut.TopModal();

                // Then
                await sut.View.DidNotReceive().PopModal();
            }

            /// <summary>
            /// Tests to make sure that it returns the last element only.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Return_Last_Element()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();
                await sut.PushModal(new NavigableViewModelMock("1"));
                await sut.PushModal(new NavigableViewModelMock("2"));

                // When
                var result = await sut.TopModal();

                // Then
                result.Should().BeOfType<NavigableViewModelMock>();
                result.Id.Should().Be("2");
            }

            /// <summary>
            /// Tests to make sure it throws an exception if the stack is empty.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_Stack_Empty()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await sut.TopModal()).ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("index");
            }
        }

        /// <summary>
        /// Tests for the TopPage method.
        /// </summary>
        public class TheTopPageMethod
        {
            /// <summary>
            /// Tests to make sure that it does not pop the stack.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Not_Pop()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture().WithView(Substitute.For<IView>());
                await sut.PushPage(new NavigableViewModelMock());

                // When
                await sut.TopPage();

                // Then
                await sut.View.DidNotReceive().PopPage();
            }

            /// <summary>
            /// Tests to make sure that it returns the last element only.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Return_Last_Element()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();
                await sut.PushPage(new NavigableViewModelMock("1"));
                await sut.PushPage(new NavigableViewModelMock("2"));

                // When
                var result = await sut.TopPage();

                // Then
                result.Should().BeOfType<NavigableViewModelMock>();
                result.Id.Should().Be("2");
            }

            /// <summary>
            /// Tests to make sure it throws an exception if the stack is empty.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Throw_If_Stack_Empty()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await sut.TopPage()).ConfigureAwait(false);

                // Then
                result.Should().BeOfType<ArgumentOutOfRangeException>().Which.ParamName.Should().Be("index");
            }
        }
    }
}
