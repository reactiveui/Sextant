using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Sextant.Abstraction;
using Xunit;

namespace Sextant.Tests.Navigation
{
    public sealed class ViewStackServiceTests
    {
        public class ThePopModalMethod
        {
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

        public class ThePopPageMethod
        {
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

        public class ThePushModalMethod
        {
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

        public class ThePushPageMethod
        {
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

            //[Theory]
            //[InlineData(1)]
            //[InlineData(3)]
            //[InlineData(5)]
            //public async Task Should_Push_And_Pop(int amount)
            //{
            //    // Given
            //    var fixture = new ViewStackServiceFixture();
            //    fixture.PushPage(new PageViewModelMock(),"page", amount).Subscribe();
            //    fixture.ViewStackService.PageStack.FirstAsync().Wait().Count.Should().Be(amount);
            //    fixture.PopPage(amount).Subscribe();

            //    // When
            //    var result = await fixture.ViewStackService.PageStack.FirstAsync();

            //    // Then
            //    fixture.ViewStackService.PageStack.FirstAsync().Wait().Should().BeEmpty();
            //}
        }

        public class TheTopModalMethod
        {
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

        public class TheTopPageMethod
        {
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