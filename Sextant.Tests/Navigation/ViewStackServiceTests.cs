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
            public async Task Should_Throw_If_Stack_Empty()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await fixture.ViewStackService.PopModal());

                // Then
                result.Should().BeOfType<InvalidOperationException>();
            }

            [Fact]
            public async Task Should_Pop_Modal()
            {
                // Given
                var fixture = new ViewStackServiceFixture().WithModalStack();

                // When
                await fixture.ViewStackService.PopModal();
                var result = fixture.ViewStackService.ModalStack.FirstAsync().Wait();

                // Then
                result.Should().BeEmpty();
            }

            [Fact]
            public async Task Should_Receive_Pop_Modal()
            {
                // Given, When
                var fixture = new ViewStackServiceFixture().WithModalStack();

                // When
                var result = await fixture.ViewStackService.PopModal();

                // Then
                await fixture.View.Received().PopModal();
            }

            [Fact]
            public async Task Should_Return_Unit()
            {
                // Given, When
                var fixture = new ViewStackServiceFixture().WithModalStack();

                // When
                var result = await fixture.ViewStackService.PopModal();

                // Then
                result.Should().BeOfType<Unit>();
            }
        }

        public class ThePopPageMethod
        {
            [Fact]
            public async Task Should_Throw_If_Stack_Empty()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                var result = await Record.ExceptionAsync(async () => await fixture.ViewStackService.PopPage());

                // Then
                result.Should().BeOfType<InvalidOperationException>();
                result.Message.Should().Be("Stack is empty.");
            }

            [Fact]
            public async Task Should_Pop_Page()
            {
                // Given
                var fixture = new ViewStackServiceFixture().WithPageStack();

                // When
                await fixture.ViewStackService.PopPage();
                var result = fixture.ViewStackService.PageStack.FirstAsync().Wait();

                // Then
                result.Should().BeEmpty();
            }

            [Fact]
            public async Task Should_Return_Unit()
            {
                // Given, When
                var fixture = new ViewStackServiceFixture().WithPageStack();

                // When
                var result = await fixture.ViewStackService.PopPage();

                // Then
                result.Should().BeOfType<Unit>();
            }

            [Fact]
            public void Should_Receive_Pop_Page()
            {
                // Given
                var fixture = new ViewStackServiceFixture().WithPageStack();

                // When
                fixture.ViewStackService.PopPage();

                // Then
                fixture.View.Received().PopPage();
            }
        }

        public class ThePushModalMethod
        {
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

            [Fact]
            public async Task Should_Push_Modal()
            {
                // Given
                var fixture = new ViewStackServiceFixture();

                // When
                await fixture.ViewStackService.PushModal(new PageViewModelMock());
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
                await fixture.ViewStackService.PushModal(fixture.ModalViewModel, "modal");
                var result = fixture.ViewStackService.ModalStack.FirstAsync().Wait();

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
                fixture.ViewStackService.PushModal(fixture.ModalViewModel, "modal");

                // Then
                fixture.View.Received().PushModal(Arg.Any<IPageViewModel>(), "modal");
            }
        }

        public class ThePushPageMethod
        {
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
                await fixture.ViewStackService.PushPage(fixture.PageViewModel);
                var result = fixture.ViewStackService.PageStack.FirstAsync().Wait();

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
                fixture.ViewStackService.PushPage(fixture.PageViewModel);

                // Then
                fixture.View.Received().PushPage(Arg.Any<IPageViewModel>(), null, false, true);
            }
        }

        public class TheTopPageMethod
        {
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

            [Fact]
            public async Task Should_Return_Last_Element()
            {
                // Given
                var fixture = new ViewStackServiceFixture().WithPageStack();
                await fixture.ViewStackService.PushPage(new PageViewModelMock());

                // When
                var result = await fixture.ViewStackService.TopPage();

                // Then
                result.Should().BeOfType<PageViewModelMock>();
                result.Id.Should().Be(nameof(PageViewModelMock));
            }

            [Fact]
            public async Task Should_Not_Pop()
            {
                // Given
                var fixture = new ViewStackServiceFixture().WithPageStack();
                await fixture.ViewStackService.PushPage(new PageViewModelMock());

                // When
                await fixture.ViewStackService.TopPage();

                // Then
                await fixture.View.DidNotReceive().PopPage(Arg.Any<bool>());
            }
        }

        public class TheTopModalMethod
        {
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

            [Fact]
            public async Task Should_Return_Last_Element()
            {
                // Given
                var fixture = new ViewStackServiceFixture().WithModalStack();
                await fixture.ViewStackService.PushModal(new PageViewModelMock());

                // When
                var result = await fixture.ViewStackService.TopModal();

                // Then
                result.Should().BeOfType<PageViewModelMock>();
                result.Id.Should().Be(nameof(PageViewModelMock));
            }

            [Fact]
            public async Task Should_Not_Pop()
            {
                // Given
                var fixture = new ViewStackServiceFixture().WithModalStack();
                await fixture.ViewStackService.PushModal(new PageViewModelMock());

                // When
                await fixture.ViewStackService.TopModal();

                // Then
                await fixture.View.DidNotReceive().PopModal();
            }
        }
    }
}