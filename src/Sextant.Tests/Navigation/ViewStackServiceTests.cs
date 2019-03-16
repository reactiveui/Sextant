// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Sextant.Abstraction;
using Xunit;

#pragma warning disable RCS1090, SA1615, CA1707
namespace Sextant.Tests.Navigation
{
    /// <summary>
    /// Tests for the <see cref="ViewStackService"/>.
    /// </summary>
    public sealed class ViewStackServiceTests
    {
        /// <summary>
        /// Tests for the pop modal method.
        /// </summary>
        public class ThePopModalMethod
        {
            /// <summary>
            /// Should pop the modal.
            /// </summary>
            [Fact]
            public void Should_Pop_Modal()
            {
                // Given
                var fixture = new ViewStackServiceFixture();
                fixture.PushModal(new PageViewModelMock()).Subscribe();

                // When
                fixture.ViewStackService.ModalStack.FirstAsync().Wait().Count.Should().Be(1);
                fixture.PopModal().Subscribe();

                // Then
                fixture.ViewStackService.ModalStack.FirstAsync().Wait().Should().BeEmpty();
            }

            /// <summary>
            /// Should receive pop modal.
            /// </summary>
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
            /// Should the return a unit.
            /// </summary>
            [Fact]
            public async Task Should_Return_Unit()
            {
                // Given, When
                var fixture = new ViewStackServiceFixture();
                fixture.PushModal(new PageViewModelMock()).Subscribe();

                // When
                var result = await fixture.PopModal();

                // Then
                result.Should().BeOfType<Unit>();
            }

            /// <summary>
            /// Should throw if the stack is empty.
            /// </summary>
            [Fact]
            public async Task Should_Throw_If_Stack_Empty()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await fixture.ViewStackService.PopModal());

                // Then
                result.Should().BeOfType<InvalidOperationException>();
            }
        }

        /// <summary>
        /// Tests for the pop page method.
        /// </summary>
        public class ThePopPageMethod
        {
            /// <summary>
            /// Should pop the page.
            /// </summary>
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
                result.Should().BeEmpty();
            }

            /// <summary>
            /// Should receive pop page.
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
            /// Should return the unit.
            /// </summary>
            [Fact]
            public async Task Should_Return_Unit()
            {
                // Given
                var fixture = new ViewStackServiceFixture();
                fixture.PushPage(new PageViewModelMock()).Subscribe();

                // When
                var result = await fixture.ViewStackService.PopPage();

                // Then
                result.Should().BeOfType<Unit>();
            }
        }

        /// <summary>
        /// Tests for the push modal method.
        /// </summary>
        public class ThePushModalMethod
        {
            /// <summary>
            /// Should push and pop.
            /// </summary>
            /// <param name="amount">The amount.</param>
            [Theory]
            [InlineData(1)]
            [InlineData(3)]
            [InlineData(5)]
            public async Task Should_Push_And_Pop(int amount)
            {
                // Given
                var fixture = new ViewStackServiceFixture();
                fixture.PushModal(new PageViewModelMock(), "modal", amount).Subscribe();
                fixture.ViewStackService.ModalStack.FirstAsync().Wait().Count.Should().Be(amount);
                fixture.PopModal(amount).Subscribe();

                // When
                var result = await fixture.ViewStackService.ModalStack.FirstAsync();

                // Then
                result.Should().BeEmpty();
            }

            /// <summary>
            /// Should push the modal.
            /// </summary>
            [Fact]
            public async Task Should_Push_Modal()
            {
                // Given
                var fixture = new ViewStackServiceFixture();
                fixture.PushModal(new PageViewModelMock()).Subscribe();

                // When
                var result = await fixture.ViewStackService.TopModal();

                // Then
                result.Should().NotBeNull();
                result.Should().BeOfType<PageViewModelMock>();
            }

            /// <summary>
            /// Should push the page on the stack.
            /// </summary>
            [Fact]
            public async Task Should_Push_Page_On_Stack()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                await fixture.ViewStackService.PushModal(new PageViewModelMock(), "modal");
                var result = await fixture.ViewStackService.ModalStack.FirstAsync();

                // Then
                result.Should().NotBeNullOrEmpty();
                result.Should().ContainSingle();
            }

            /// <summary>
            /// Should receive push modal.
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
            /// Should throw if the view model is null.
            /// </summary>
            [Fact]
            public async Task Should_Throw_If_View_Model_Null()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await fixture.ViewStackService.PushModal(null));

                // Then
                result.Should().BeOfType<ArgumentNullException>();
            }
        }

        /// <summary>
        /// Tests for the push page method.
        /// </summary>
        public class ThePushPageMethod
        {
            /// <summary>
            /// Should push the page.
            /// </summary>
            [Fact]
            public async Task Should_Push_Page()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                await fixture.ViewStackService.PushPage(new PageViewModelMock());
                var result = await fixture.ViewStackService.TopPage();

                // Then
                result.Should().NotBeNull();
                result.Should().BeOfType<PageViewModelMock>();
            }

            /// <summary>
            /// Should push the page on the stack.
            /// </summary>
            [Fact]
            public async Task Should_Push_Page_On_Stack()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                await fixture.ViewStackService.PushPage(new PageViewModelMock());
                var result = await fixture.ViewStackService.PageStack.FirstAsync();

                // Then
                result.Should().NotBeNullOrEmpty();
                result.Should().ContainSingle();
            }

            /// <summary>
            /// Should receive push page.
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
            /// Should throw if the view model is null.
            /// </summary>
            [Fact]
            public async Task Should_Throw_If_View_Model_Null()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await fixture.ViewStackService.PushPage(null));

                // Then
                result.Should().BeOfType<ArgumentNullException>();
            }
        }

        /// <summary>
        /// Tests for the top modal method.
        /// </summary>
        public class TheTopModalMethod
        {
            /// <summary>
            /// Should not pop the apge.
            /// </summary>
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
            /// Should return the last element.
            /// </summary>
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
                result.Should().BeOfType<PageViewModelMock>();
                result.Id.Should().Be("2");
            }

            /// <summary>
            /// Should throw if the stack is empty.
            /// </summary>
            [Fact]
            public async Task Should_Throw_If_Stack_Empty()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await fixture.ViewStackService.TopModal());

                result.Should().BeOfType<InvalidOperationException>();
                result.Message.Should().Be("Sequence contains no elements");
            }
        }

        /// <summary>
        /// Tests for the top page method.
        /// </summary>
        public class TheTopPageMethod
        {
            /// <summary>
            /// Shoulds the not pop.
            /// </summary>
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
            /// Shoulds return the last element.
            /// </summary>
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
                result.Should().BeOfType<PageViewModelMock>();
                result.Id.Should().Be("2");
            }

            /// <summary>
            /// Should throw if the stack is empty.
            /// </summary>
            [Fact]
            public async Task Should_Throw_If_Stack_Empty()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await fixture.ViewStackService.TopPage());

                result.Should().BeOfType<InvalidOperationException>();
                result.Message.Should().Be("Sequence contains no elements");
            }
        }
    }
}
#pragma warning restore RCS1090, SA1615, CA1707
