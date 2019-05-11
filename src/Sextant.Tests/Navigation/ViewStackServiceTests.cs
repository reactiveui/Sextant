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
using NSubstitute;
using Shouldly;
using Xunit;

namespace Sextant.Tests.Navigation
{
    /// <summary>
    /// Tests that check to make sure that the view stack works correctly.
    /// </summary>
    public sealed class ViewStackServiceTests
    {
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
                sut.PageStack.ShouldNotBeOfType<BehaviorSubject<IImmutableList<IPageViewModel>>>();
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
                sut.PageStack.ShouldNotBeOfType<BehaviorSubject<IImmutableList<IPageViewModel>>>();
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
                sut.ModalStack.ShouldNotBeOfType<BehaviorSubject<IImmutableList<IPageViewModel>>>();
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
                sut.ModalStack.ShouldNotBeOfType<BehaviorSubject<IImmutableList<IPageViewModel>>>();
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
                await sut.PushModal(new PageViewModelMock());

                // When
                var item = await sut.ModalStack.FirstAsync();
                item.Count.ShouldBe(1);
                await sut.PopModal();

                // Then
                item = await sut.ModalStack.FirstAsync();
                item.ShouldBeEmpty();
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
                await sut.PushModal(new PageViewModelMock());

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
                await sut.PushModal(new PageViewModelMock());

                // When
                var result = await sut.PopModal();

                // Then
                result.ShouldBeOfType<Unit>();
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
                result.ShouldBeOfType<InvalidOperationException>();
                result?.Message.ShouldBe("Stack is empty.");
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
                ViewStackService sut = new ViewStackServiceFixture();
                await sut.PushModal(new PageViewModelMock());

                // When
                await sut.PopPage();
                var result = await sut.PageStack.FirstAsync();

                // Then
                result.ShouldBeEmpty();
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
                await sut.PushModal(new PageViewModelMock());

                // When
                await sut.PopPage();

                // Then
                sut.View.Received().PopPage();
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
                await sut.PushPage(new PageViewModelMock());

                // When
                var result = await sut.PopPage();

                // Then
                result.ShouldBeOfType<Unit>();
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
                var result = await Should.ThrowAsync<InvalidOperationException>(async () => await sut.PopToRootPage()).ConfigureAwait(false);

                // Then
                result.Message.ShouldBe("Stack is empty.");
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
                await sut.PushPage(new PageViewModelMock(), pages: 3);

                // When
                await sut.PopToRootPage();
                var result = await sut.PageStack.FirstOrDefaultAsync();

                // Then
                result.ShouldHaveSingleItem();
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
                await sut.PushPage(new PageViewModelMock(), pages: 3);

                sut.View.PagePopped.Subscribe(_ =>
                {
                    count++;
                });

                // When
                await sut.PopToRootPage();

                // Then
                count.ShouldBe(1);
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
                await sut.PushPage(new PageViewModelMock(), pages: 3);
                await sut.PopToRootPage();

                // When
                var result = await sut.PageStack.FirstOrDefaultAsync();

                // Then
                result.ShouldHaveSingleItem();
            }
        }

        /// <summary>
        /// Tests for the push model method.
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
                await sut.PushModal(new PageViewModelMock(), "modal", amount);
                sut.ModalStack.FirstAsync().Wait().Count.ShouldBe(amount);
                await sut.PopModal(amount);

                // When
                var result = await sut.ModalStack.FirstAsync();

                // Then
                result.ShouldBeEmpty();
            }

            /// <summary>
            /// Tests to make sure that the push model works.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Push_Modal()
            {
                // Given
                ViewStackService sut = new ViewStackServiceFixture();
                await sut.PushModal(new PageViewModelMock());

                // When
                var result = await sut.TopModal();

                // Then
                result.ShouldNotBeNull();
                result.ShouldBeOfType<PageViewModelMock>();
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
                await sut.PushModal(new PageViewModelMock(), "modal");
                var result = await sut.ModalStack.FirstAsync();

                // Then
                result.ShouldNotBeEmpty();
                result.Count.ShouldBe(1);
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
                await sut.PushModal(new PageViewModelMock(), "modal");

                // Then
                sut.View.Received().PushModal(Arg.Any<IPageViewModel>(), "modal");
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
                result.ShouldBeOfType<ArgumentNullException>();
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
                var result = await Should.ThrowAsync<ArgumentNullException>(async () => await sut.PushPage(null)).ConfigureAwait(false);

                // Then
                result.ParamName.ShouldBe("page");
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
                await sut.PushPage(new PageViewModelMock());
                var result = await sut.TopPage();

                // Then
                result.ShouldNotBeNull();
                result.ShouldBeOfType<PageViewModelMock>();
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
                await sut.PushPage(new PageViewModelMock());
                var result = await sut.PageStack.FirstAsync();

                // Then
                result.ShouldNotBeEmpty();
                result.Count.ShouldBe(1);
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
                await sut.PushPage(new PageViewModelMock());

                // Then
                sut.View.Received().PushPage(Arg.Any<IPageViewModel>(), null, false, true);
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
                await sut.PushPage(new PageViewModelMock(), pages: 3);
                await sut.PushPage(new PageViewModelMock(), resetStack: true);
                var result = await sut.PageStack.FirstOrDefaultAsync();

                // Then
                result.ShouldHaveSingleItem();
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
                await sut.PushModal(new PageViewModelMock());

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
                await sut.PushModal(new PageViewModelMock("1"));
                await sut.PushModal(new PageViewModelMock("2"));

                // When
                var result = await sut.TopModal();

                // Then
                result.ShouldBeOfType<PageViewModelMock>();
                result.Id.ShouldBe("2");
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
                var result = await Should.ThrowAsync<InvalidOperationException>(async () => await sut.TopModal()).ConfigureAwait(false);

                // Then
                result.Message.ShouldBe("Sequence contains no elements");
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
                await sut.PushPage(new PageViewModelMock());

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
                await sut.PushPage(new PageViewModelMock("1"));
                await sut.PushPage(new PageViewModelMock("2"));

                // When
                var result = await sut.TopPage();

                // Then
                result.ShouldBeOfType<PageViewModelMock>();
                result.Id.ShouldBe("2");
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
                var result = await Should.ThrowAsync<InvalidOperationException>(async () => await sut.TopPage()).ConfigureAwait(false);

                // Then
                result.Message.ShouldBe("Sequence contains no elements");
            }
        }
    }
}
