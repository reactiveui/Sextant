// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using NSubstitute.Extensions;
using ReactiveUI;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Events;
using Rg.Plugins.Popup.Pages;
using Sextant.Mocks;
using Xunit;

namespace Sextant.Plugins.Popup.Tests
{
    /// <summary>
    /// Tests the <see cref="PopupViewStackService"/> implementation.
    /// </summary>
    public sealed class PopupViewStackServiceTests
    {
        /// <summary>
        /// Tests that verify the Pushing property.
        /// </summary>
        public class ThePushingProperty
        {
            /// <summary>
            /// Tests the observer can respond to events.
            /// </summary>
            [Fact]
            public void Should_Observe_Pushing()
            {
                // Given
                PopupNavigationEvent pushing = null;
                var viewModel = new NavigableViewModelMock();
                var popup = new PopupMock
                {
                    ViewModel = viewModel
                };
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);
                sut.Pushing.Subscribe(x => pushing = x);

                // When
                navigation.Pushing += Raise.EventWith(new PopupNavigationEventArgs(popup, true));

                // Then
                pushing.ViewModel.Should().Be(viewModel);
            }
        }

        /// <summary>
        /// Tests that verify the Pushed property.
        /// </summary>
        public class ThePushedProperty
        {
            /// <summary>
            /// Tests the observer can respond to events.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Observe_Pushed()
            {
                // Given
                PopupNavigationEvent pushing = null;
                var viewModel = new NavigableViewModelMock();
                var popup = new PopupMock
                {
                    ViewModel = viewModel
                };
                var navigation = Substitute.For<IPopupNavigation>();
                var viewLocator = Substitute.For<IViewLocator>();
                viewLocator.ResolveView(Arg.Any<IViewModel>()).Returns(popup);
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation).WithViewLocator(viewLocator);
                sut.Pushed.Subscribe(x => pushing = x);

                // When
                await sut.PushPopup(viewModel);
                navigation.Pushed += Raise.EventWith(new PopupNavigationEventArgs(popup, true));

                // Then
                pushing.ViewModel.Should().Be(viewModel);
            }
        }

        /// <summary>
        /// Tests that verify the Popping property.
        /// </summary>
        public class ThePoppingProperty
        {
            /// <summary>
            /// Tests the observer can respond to events.
            /// </summary>
            [Fact]
            public void Should_Observe_Pushing()
            {
                // Given
                PopupNavigationEvent pushing = null;
                var viewModel = new NavigableViewModelMock();
                var popup = new PopupMock
                {
                    ViewModel = viewModel
                };
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);
                sut.Popping.Subscribe(x => pushing = x);

                // When
                navigation.Popping += Raise.EventWith(new PopupNavigationEventArgs(popup, true));

                // Then
                pushing.ViewModel.Should().Be(viewModel);
            }
        }

        /// <summary>
        /// Tests that verify the Popped property.
        /// </summary>
        public class ThePoppedProperty
        {
            /// <summary>
            /// Tests the observer can respond to events.
            /// </summary>
            [Fact]
            public void Should_Observe_Popped()
            {
                // Given
                PopupNavigationEvent pushing = null;
                var viewModel = new NavigableViewModelMock();
                var popup = new PopupMock
                {
                    ViewModel = viewModel
                };
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);
                sut.Popped.Subscribe(x => pushing = x);

                // When
                navigation.Popped += Raise.EventWith(new PopupNavigationEventArgs(popup, true));

                // Then
                pushing.ViewModel.Should().Be(viewModel);
            }

            /// <summary>
            /// Tests the observer can respond to events.
            /// </summary>
            [Fact]
            public void Should_Call_Destroy()
            {
                // Given
                PopupNavigationEvent pushing = null;
                var viewModel = Substitute.For<IEverything>();
                var popup = new PopupMock
                {
                    ViewModel = viewModel
                };
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);
                sut.Popped.Subscribe(x => pushing = x);

                // When
                navigation.Popped += Raise.EventWith(new PopupNavigationEventArgs(popup, true));

                // Then
                ((IDestructible)popup.ViewModel).Received(1).Destroy();
            }
        }

        /// <summary>
        /// Tests that verify the PushPopup method.
        /// </summary>
        public class ThePushPopupMethod
        {
            /// <summary>
            /// Tests the method calls the decorated method.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Popup_Navigation()
            {
                // Given
                var viewModel = new NavigableViewModelMock();
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);

                // When
                await sut.PushPopup(viewModel);

                // Then
                await navigation.Received(1).PushAsync(Arg.Any<PopupPage>()).ConfigureAwait(false);
            }

            /// <summary>
            /// Tests the method calls the view locator.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_View_Locator()
            {
                // Given
                var viewModel = new NavigableViewModelMock();
                var viewLocator = Substitute.For<IViewLocator>();
                viewLocator.ResolveView(Arg.Any<IViewModel>()).Returns(new PopupMock());
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithViewLocator(viewLocator);

                // When
                await sut.PushPopup(viewModel);

                // Then
                viewLocator.Received(1).ResolveView(Arg.Any<IViewModel>());
            }

            /// <summary>
            /// Tests the method emits a Pushing event.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Observe_Pushing()
            {
                // Given
                bool pushing = false;
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(new PopupNavigationMock());
                sut.Pushing.Select(_ => true).Subscribe(x => pushing = x);

                // When
                await sut.PushPopup(new NavigableViewModelMock());

                // Then
                pushing.Should().BeTrue();
            }

            /// <summary>
            /// Tests the method emits a Pushed event.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Observe_Pushed()
            {
                // Given
                bool pushed = false;
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(new PopupNavigationMock());
                sut.Pushed.Select(_ => true).Subscribe(x => pushed = x);

                // When
                await sut.PushPopup(new NavigableViewModelMock());

                // Then
                pushed.Should().BeTrue();
            }
        }

        /// <summary>
        /// Tests that verify the PushPopup method that takes parameters.
        /// </summary>
        public class ThePushPopupWithParametersMethod
        {
            /// <summary>
            /// Tests the method calls the decorated method.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Popup_Navigation()
            {
                // Given
                var parameter = new NavigationParameter();
                var viewModel = new NavigableViewModelMock();
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);

                // When
                await sut.PushPopup(viewModel, parameter);

                // Then
                await navigation.Received(1).PushAsync(Arg.Any<PopupPage>()).ConfigureAwait(false);
            }

            /// <summary>
            /// Tests the method calls the view locator.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_View_Locator()
            {
                // Given
                var parameter = new NavigationParameter();
                var viewModel = new NavigableViewModelMock();
                var viewLocator = Substitute.For<IViewLocator>();
                viewLocator.ResolveView(Arg.Any<IViewModel>()).Returns(new PopupMock());
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithViewLocator(viewLocator);

                // When
                await sut.PushPopup(viewModel, parameter);

                // Then
                viewLocator.Received(1).ResolveView(Arg.Any<IViewModel>());
            }

            /// <summary>
            /// Tests the method emits a Pushing event.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Observe_Pushing()
            {
                // Given
                bool pushing = false;
                var parameter = new NavigationParameter();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(new PopupNavigationMock());
                sut.Pushing.Select(_ => true).Subscribe(x => pushing = x);

                // When
                await sut.PushPopup(new NavigableViewModelMock(), parameter);

                // Then
                pushing.Should().BeTrue();
            }

            /// <summary>
            /// Tests the method emits a Pushed event.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Observe_Pushed()
            {
                // Given
                bool pushed = false;
                var parameter = new NavigationParameter();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(new PopupNavigationMock());
                sut.Pushed.Select(_ => true).Subscribe(x => pushed = x);

                // When
                await sut.PushPopup(new NavigableViewModelMock(), parameter);

                // Then
                pushed.Should().BeTrue();
            }

            /// <summary>
            /// Tests the method calls the navigating to method.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Navigating_To()
            {
                // Given
                var parameter = new NavigationParameter();
                var viewLocator = Substitute.For<IViewLocator>();
                var viewModel = Substitute.For<INavigable>();
                viewLocator.ResolveView(Arg.Any<NavigableViewModelMock>(), Arg.Any<string>()).Returns(new PopupMock());
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);

                // When
                await sut.PushPopup(viewModel, parameter);

                // Then
                await viewModel.Received(1).WhenNavigatingTo(parameter);
            }

            /// <summary>
            /// Tests the method calls the navigated to method.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Navigated_To()
            {
                // Given
                var parameter = new NavigationParameter();
                var viewModel = Substitute.For<AbstractViewModel>();
                var viewModelFactory = Substitute.For<IViewModelFactory>();
                viewModelFactory.Create<AbstractViewModel>().Returns(viewModel);
                var viewLocator = Substitute.For<IViewLocator>();
                viewLocator.ResolveView(Arg.Any<AbstractViewModel>(), Arg.Any<string>()).Returns(new PopupMock());
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation).WithViewLocator(viewLocator).WithViewModelFactory(viewModelFactory);

                // When
                await sut.PushPopup(viewModel, parameter);

                // Then
                await viewModel.Received(1).WhenNavigatedTo(parameter);
            }
        }

        /// <summary>
        /// Tests that verify the PushPopup generic method.
        /// </summary>
        public class ThePushPopupGenericMethod
        {
            /// <summary>
            /// Tests the method calls the decorated method.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Popup_Navigation()
            {
                // Given
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);

                // When
                await sut.PushPopup<NavigableViewModelMock>();

                // Then
                await navigation.Received(1).PushAsync(Arg.Any<PopupPage>()).ConfigureAwait(false);
            }

            /// <summary>
            /// Tests the method calls the view model factory.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_View_Model_Factory()
            {
                // Given
                var viewModelFactory = Substitute.For<IViewModelFactory>();
                viewModelFactory.Create<NavigableViewModelMock>(Arg.Any<string>()).Returns(new NavigableViewModelMock());
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithViewModelFactory(viewModelFactory);

                // When
                await sut.PushPopup<NavigableViewModelMock>();

                // Then
                viewModelFactory.Received(1).Create<NavigableViewModelMock>();
            }

            /// <summary>
            /// Tests the method calls the view locator.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_View_Locator()
            {
                // Given
                var viewLocator = Substitute.For<IViewLocator>();
                viewLocator.ResolveView(Arg.Any<IViewModel>()).Returns(new PopupMock());
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithViewLocator(viewLocator);

                // When
                await sut.PushPopup<NavigableViewModelMock>();

                // Then
                viewLocator.Received(1).ResolveView(Arg.Any<IViewModel>());
            }

            /// <summary>
            /// Tests the method emits a Pushing event.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Observe_Pushing()
            {
                // Given
                bool pushing = false;
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(new PopupNavigationMock());
                sut.Pushing.Select(_ => true).Subscribe(x => pushing = x);

                // When
                await sut.PushPopup<NavigableViewModelMock>();

                // Then
                pushing.Should().BeTrue();
            }

            /// <summary>
            /// Tests the method emits a Pushed event.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Observe_Pushed()
            {
                // Given
                bool pushed = false;
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(new PopupNavigationMock());
                sut.Pushed.Select(_ => true).Subscribe(x => pushed = x);

                // When
                await sut.PushPopup<NavigableViewModelMock>();

                // Then
                pushed.Should().BeTrue();
            }
        }

        /// <summary>
        /// Tests that verify the PushPopup generic method that takes parameters.
        /// </summary>
        public class ThePushPopupGenericWithParameterMethod
        {
            /// <summary>
            /// Tests the method calls the decorated method.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Popup_Navigation()
            {
                // Given
                var parameter = new NavigationParameter();
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);

                // When
                await sut.PushPopup<NavigableViewModelMock>(parameter);

                // Then
                await navigation.Received(1).PushAsync(Arg.Any<PopupPage>()).ConfigureAwait(false);
            }

            /// <summary>
            /// Tests the method calls the view model factory.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_View_Model_Factory()
            {
                // Given
                var parameter = new NavigationParameter();
                var viewModelFactory = Substitute.For<IViewModelFactory>();
                viewModelFactory.Create<NavigableViewModelMock>(Arg.Any<string>()).Returns(new NavigableViewModelMock());
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithViewModelFactory(viewModelFactory);

                // When
                await sut.PushPopup<NavigableViewModelMock>(parameter);

                // Then
                viewModelFactory.Received(1).Create<NavigableViewModelMock>();
            }

            /// <summary>
            /// Tests the method calls the view locator.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_View_Locator()
            {
                // Given
                var parameter = new NavigationParameter();
                var viewModel = new NavigableViewModelMock();
                var viewLocator = Substitute.For<IViewLocator>();
                viewLocator.ResolveView(Arg.Any<NavigableViewModelMock>(), Arg.Any<string>()).Returns(new PopupMock());
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithViewLocator(viewLocator);

                // When
                await sut.PushPopup<NavigableViewModelMock>(parameter);

                // Then
                viewLocator.Received(1).ResolveView(Arg.Any<IViewModel>());
            }

            /// <summary>
            /// Tests the method emits a Pushing event.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Observe_Pushing()
            {
                // Given
                bool pushing = false;
                var parameter = new NavigationParameter();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(new PopupNavigationMock());
                sut.Pushing.Select(_ => true).Subscribe(x => pushing = x);

                // When
                await sut.PushPopup<NavigableViewModelMock>(parameter);

                // Then
                pushing.Should().BeTrue();
            }

            /// <summary>
            /// Tests the method emits a Pushed event.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Observe_Pushed()
            {
                // Given
                bool pushed = false;
                var parameter = new NavigationParameter();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(new PopupNavigationMock());
                sut.Pushed.Select(_ => true).Subscribe(x => pushed = x);

                // When
                await sut.PushPopup<NavigableViewModelMock>(parameter);

                // Then
                pushed.Should().BeTrue();
            }

            /// <summary>
            /// Tests the method calls the navigating to method.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Navigating_To()
            {
                // Given
                var parameter = new NavigationParameter();
                var viewModel = Substitute.For<AbstractViewModel>();
                var viewLocator = Substitute.For<IViewLocator>();
                viewLocator.ResolveView(Arg.Any<AbstractViewModel>()).Returns(new PopupMock());
                var factory = Substitute.For<IViewModelFactory>();
                factory.Create<AbstractViewModel>(Arg.Any<string>()).Returns(viewModel);
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation).WithViewModelFactory(factory).WithViewLocator(viewLocator);

                // When
                await sut.PushPopup<AbstractViewModel>(parameter);

                // Then
                await viewModel.Received(1).WhenNavigatingTo(parameter);
            }

            /// <summary>
            /// Tests the method calls the navigated to method.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Navigated_To()
            {
                // Given
                var parameter = new NavigationParameter();
                var viewModel = Substitute.For<AbstractViewModel>();
                var viewLocator = Substitute.For<IViewLocator>();
                var viewModelFactory = Substitute.For<IViewModelFactory>();
                viewModelFactory.Create<AbstractViewModel>().Returns(viewModel);
                viewLocator
                    .ResolveView(Arg.Any<AbstractViewModel>(), Arg.Any<string>())
                    .Returns(new PopupMock());

                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation).WithViewModelFactory(viewModelFactory).WithViewLocator(viewLocator);

                // When
                await sut.PushPopup<AbstractViewModel>(parameter);

                // Then
                await viewModel.Received(1).WhenNavigatedTo(parameter);
            }
        }

        /// <summary>
        /// Tests that verify the PopPopup method.
        /// </summary>
        public class ThePopPopupMethod
        {
            /// <summary>
            /// Tests the method calls the decorated method.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Popup_Navigation()
            {
                // Given
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);

                // When
                await sut.PopPopup();

                // Then
                await navigation.Received(1).PopAsync(Arg.Any<bool>()).ConfigureAwait(false);
            }

            /// <summary>
            /// Tests the method emits a Popping event.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Observe_Popping()
            {
                // Given
                bool pushing = false;
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(new PopupNavigationMock());
                sut.Popping.Select(_ => true).Subscribe(x => pushing = x);

                // When
                await sut.PushPopup(new NavigableViewModelMock());
                await sut.PopPopup();

                // Then
                pushing.Should().BeTrue();
            }

            /// <summary>
            /// Tests the method emits a Popped event.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Observe_Popped()
            {
                // Given
                bool pushed = false;
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(new PopupNavigationMock());
                sut.Popped.Select(_ => true).Subscribe(x => pushed = x);

                // When
                await sut.PushPopup(new NavigableViewModelMock());
                await sut.PopPopup();

                // Then
                pushed.Should().BeTrue();
            }

            /// <summary>
            /// Tests the popped view model is destroyed.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Destroy()
            {
                // Given
                var viewModel = Substitute.For<IEverything>();
                var popup = new PopupMock
                {
                    ViewModel = viewModel
                };
                bool pushed = false;
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(new PopupNavigationMock());
                sut.Popped.Select(_ => true).Subscribe(x => pushed = x);

                // When
                await sut.PushPopup(viewModel);
                await sut.PopPopup();

                // Then
                ((IDestructible)popup.ViewModel).Received(1).Destroy();
            }
        }

        /// <summary>
        /// Tests that verify the PopAllPopups method.
        /// </summary>
        public class ThePopAllPopupsMethod
        {
            /// <summary>
            /// Tests the method calls the decorated method.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Popup_Navigation()
            {
                // Given
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);
                await sut.PushPopup(new NavigableViewModelMock());

                // When
                await sut.PopAllPopups();

                // Then
                await navigation.Received(1).PopAllAsync().ConfigureAwait(false);
            }

            /// <summary>
            /// Tests the popped view model is destroyed.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Destroy()
            {
                // Given
                var viewModel = Substitute.For<IEverything>();
                var popup = new PopupMock
                {
                    ViewModel = viewModel
                };
                bool pushed = false;
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(new PopupNavigationMock());
                sut.Popped.Select(_ => true).Subscribe(x => pushed = x);

                // When
                await sut.PushPopup(viewModel);
                await sut.PopAllPopups();

                // Then
                ((IDestructible)popup.ViewModel).Received(1).Destroy();
            }
        }

        /// <summary>
        /// Tests that verify the RemovePopup method.
        /// </summary>
        public class TheRemovePopupMethod
        {
            /// <summary>
            /// Tests the method calls the decorated method.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Popup_Navigation()
            {
                // Given
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);

                // When
                await sut.RemovePopup(new NavigableViewModelMock());

                // Then
                await navigation.Received(1).RemovePageAsync(Arg.Any<PopupPage>()).ConfigureAwait(false);
            }

            /// <summary>
            /// Tests the popped view model is destroyed.
            /// </summary>
            /// <returns>A completion notification.</returns>
            [Fact]
            public async Task Should_Call_Destroy()
            {
                // Given
                var viewModel = Substitute.For<IEverything>();
                var popup = new PopupMock
                {
                    ViewModel = viewModel
                };
                bool pushed = false;
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(new PopupNavigationMock());
                sut.Popped.Select(_ => true).Subscribe(x => pushed = x);

                // When
                await sut.PushPopup(viewModel);
                await sut.RemovePopup(viewModel);

                // Then
                ((IDestructible)popup.ViewModel).Received(1).Destroy();
            }
        }
    }
}
