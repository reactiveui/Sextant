// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
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
        /// Tests associated with the pop model methods.
        /// </summary>
        public class ThePopModalMethod
        {
            /// <summary>
            /// Checks to make sure that the pop modal method works correctly.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Pop_Modal()
            {
                // Given
                var fixture = new ViewStackServiceFixture();
                fixture.PushModal(new PageViewModelMock()).Subscribe();

                // When
                var item = await fixture.ViewStackService.ModalStack.FirstAsync();
                item.Count.ShouldBe(1);
                fixture.PopModal().Subscribe();

                // Then
                item = await fixture.ViewStackService.ModalStack.FirstAsync();
                item.ShouldBeEmpty();
            }

            /// <summary>
            /// Checks to make sure that the pop modal observables are received.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Receive_Pop_Modal()
            {
                // Given, When
                var fixture = new ViewStackServiceFixture();
                fixture.PushModal(new PageViewModelMock()).Subscribe();

                // When
                fixture.PopModal().Subscribe();

                // Then
                await fixture.View.Received().PopModal();
            }

            /// <summary>
            /// Checks to make sure that the pop returns a <see cref="Unit"/>.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Return_Unit()
            {
                // Given, When
                var fixture = new ViewStackServiceFixture();
                fixture.PushModal(new PageViewModelMock()).Subscribe();

                // When
                var result = await fixture.PopModal();

                // Then
                result.ShouldBeOfType<Unit>();
            }

            /// <summary>
            /// Checks to make sure that there is a exception thrown if the stack happens to be empty.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Throw_If_Stack_Empty()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await fixture.ViewStackService.PopModal()).ConfigureAwait(false);

                // Then
                result.ShouldBeOfType<InvalidOperationException>();
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
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Pop_Page()
            {
                // Given
                var fixture = new ViewStackServiceFixture();
                fixture.PushModal(new PageViewModelMock()).Subscribe();

                // When
                fixture.ViewStackService.PopPage().Subscribe();
                var result = await fixture.ViewStackService.PageStack.FirstAsync();

                // Then
                result.ShouldBeEmpty();
            }

            /// <summary>
            /// Checks to make sure that the pop page observables are received.
            /// </summary>
            [Fact]
            public void Should_Receive_Pop_Page()
            {
                // Given
                var fixture = new ViewStackServiceFixture();
                fixture.PushModal(new PageViewModelMock()).Subscribe();

                // When
                fixture.ViewStackService.PopPage().Subscribe();

                // Then
                fixture.View.Received().PopPage();
            }

            /// <summary>
            /// Checks to make sure that the pop returns a <see cref="Unit"/>.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Return_Unit()
            {
                // Given
                var fixture = new ViewStackServiceFixture();
                fixture.PushPage(new PageViewModelMock()).Subscribe();

                // When
                var result = await fixture.ViewStackService.PopPage();

                // Then
                result.ShouldBeOfType<Unit>();
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
            /// <returns>A task to monitor the progress.</returns>
            [Theory]
            [InlineData(1)]
            [InlineData(3)]
            [InlineData(5)]
            public async Task Should_Push_And_Pop(int amount)
            {
                // Given
                var fixture = new ViewStackServiceFixture();
                fixture.PushModal(new PageViewModelMock(), "modal", amount).Subscribe();
                fixture.ViewStackService.ModalStack.FirstAsync().Wait().Count.ShouldBe(amount);
                fixture.PopModal(amount).Subscribe();

                // When
                var result = await fixture.ViewStackService.ModalStack.FirstAsync();

                // Then
                result.ShouldBeEmpty();
            }

            /// <summary>
            /// Tests to make sure that the push model works.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Push_Modal()
            {
                // Given
                var fixture = new ViewStackServiceFixture();
                fixture.PushModal(new PageViewModelMock()).Subscribe();

                // When
                var result = await fixture.ViewStackService.TopModal();

                // Then
                result.ShouldNotBeNull();
                result.ShouldBeOfType<PageViewModelMock>();
            }

            /// <summary>
            /// Tests to make sure we can push a page onto the stack.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Push_Page_On_Stack()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                await fixture.ViewStackService.PushModal(new PageViewModelMock(), "modal");
                var result = await fixture.ViewStackService.ModalStack.FirstAsync();

                // Then
                result.ShouldNotBeEmpty();
                result.Count.ShouldBe(1);
            }

            /// <summary>
            /// Tests to make sure we receive an push modal notifications.
            /// </summary>
            [Fact]
            public void Should_Receive_Push_Modal()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                fixture.ViewStackService.PushModal(new PageViewModelMock(), "modal");

                // Then
                fixture.View.Received().PushModal(Arg.Any<IPageViewModel>(), "modal");
            }

            /// <summary>
            /// Tests to make sure that we get an exception throw if we pass in a null view model.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Throw_If_View_Model_Null()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await fixture.ViewStackService.PushModal(null)).ConfigureAwait(false);

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
            /// Tests to make sure that the push page works.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Push_Page()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                await fixture.ViewStackService.PushPage(new PageViewModelMock());
                var result = await fixture.ViewStackService.TopPage();

                // Then
                result.ShouldNotBeNull();
                result.ShouldBeOfType<PageViewModelMock>();
            }

            /// <summary>
            /// Tests to make sure we can push a page onto the stack.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Push_Page_On_Stack()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                await fixture.ViewStackService.PushPage(new PageViewModelMock());
                var result = await fixture.ViewStackService.PageStack.FirstAsync();

                // Then
                result.ShouldNotBeEmpty();
                result.Count.ShouldBe(1);
            }

            /// <summary>
            /// Tests to make sure we receive an push page notifications.
            /// </summary>
            [Fact]
            public void Should_Receive_Push_Page()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                fixture.PushPage(new PageViewModelMock());

                // Then
                fixture.View.Received().PushPage(Arg.Any<IPageViewModel>(), null, false, true);
            }

            /// <summary>
            /// Tests to make sure that we get an exception throw if we pass in a null view model.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Throw_If_View_Model_Null()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await fixture.ViewStackService.PushPage(null)).ConfigureAwait(false);

                // Then
                result.ShouldBeOfType<ArgumentNullException>();
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
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Not_Pop()
            {
                // Given
                var fixture = new ViewStackServiceFixture();
                fixture.PushModal(new PageViewModelMock()).Subscribe();

                // When
                fixture.ViewStackService.TopModal().Subscribe();

                // Then
                await fixture.View.DidNotReceive().PopModal();
            }

            /// <summary>
            /// Tests to make sure that it returns the last element only.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Return_Last_Element()
            {
                // Given
                var fixture = new ViewStackServiceFixture();
                fixture.PushModal(new PageViewModelMock("1")).Subscribe();
                fixture.PushModal(new PageViewModelMock("2")).Subscribe();

                // When
                var result = await fixture.ViewStackService.TopModal();

                // Then
                result.ShouldBeOfType<PageViewModelMock>();
                result.Id.ShouldBe("2");
            }

            /// <summary>
            /// Tests to make sure it throws an exception if the stack is empty.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Throw_If_Stack_Empty()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await fixture.ViewStackService.TopModal()).ConfigureAwait(false);

                result.ShouldBeOfType<InvalidOperationException>();
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
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Not_Pop()
            {
                // Given
                var fixture = new ViewStackServiceFixture();
                fixture.PushPage(new PageViewModelMock()).Subscribe();

                // When
                fixture.ViewStackService.TopPage().Subscribe();

                // Then
                await fixture.View.DidNotReceive().PopPage();
            }

            /// <summary>
            /// Tests to make sure that it returns the last element only.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Return_Last_Element()
            {
                // Given
                var fixture = new ViewStackServiceFixture();
                fixture.PushPage(new PageViewModelMock("1")).Subscribe();
                fixture.PushPage(new PageViewModelMock("2")).Subscribe();

                // When
                var result = await fixture.ViewStackService.TopPage();

                // Then
                result.ShouldBeOfType<PageViewModelMock>();
                result.Id.ShouldBe("2");
            }

            /// <summary>
            /// Tests to make sure it throws an exception if the stack is empty.
            /// </summary>
            /// <returns>A task to monitor the progress.</returns>
            [Fact]
            public async Task Should_Throw_If_Stack_Empty()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await fixture.ViewStackService.TopPage()).ConfigureAwait(false);

                result.ShouldBeOfType<InvalidOperationException>();
                result.Message.ShouldBe("Sequence contains no elements");
            }
        }
    }
}
