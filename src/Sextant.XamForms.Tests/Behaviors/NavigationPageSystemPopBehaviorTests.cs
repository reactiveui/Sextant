// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Subjects;
using System.Threading.Tasks;
using NSubstitute;
using Sextant.Mocks;
using Xamarin.Forms;
using Xunit;

namespace Sextant.XamForms.Tests
{
    /// <summary>
    /// Tests navigation page system pop behavior.
    /// </summary>
    public class NavigationPageSystemPopBehaviorTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationPageSystemPopBehaviorTests"/> class.
        /// </summary>
        public NavigationPageSystemPopBehaviorTests()
        {
            Xamarin.Forms.Mocks.MockForms.Init();
        }

        /// <summary>
        /// Tests that popping a page calls WhenNavigatedFrom.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Call_Navigated_From()
        {
            // Given
            var source = new BehaviorSubject<NavigationSource>(NavigationSource.Device);
            var viewModel = Substitute.For<INavigated>();
            var navigationPage = new NavigationPage();
            NavigationPageSystemPopBehavior sut = new(source);
            navigationPage.Behaviors.Add(sut);

            // When
            await navigationPage.PushAsync(new Page()).ConfigureAwait(true);
            await navigationPage.PushAsync(new Page { BindingContext = viewModel }).ConfigureAwait(true);
            await navigationPage.PopAsync().ConfigureAwait(true);

            // Then
            viewModel.Received(1).WhenNavigatedFrom(Arg.Any<INavigationParameter>());
        }

        /// <summary>
        /// Tests that popping a page calls Destroy.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Call_Destroy()
        {
            // Given
            var source = new BehaviorSubject<NavigationSource>(NavigationSource.Device);
            var viewModel = Substitute.For<IDestructible>();
            var navigationPage = new NavigationPage();
            NavigationPageSystemPopBehavior sut = new(source);
            navigationPage.Behaviors.Add(sut);

            // When
            await navigationPage.PushAsync(new Page()).ConfigureAwait(true);
            await navigationPage.PushAsync(new Page { BindingContext = viewModel }).ConfigureAwait(true);
            await navigationPage.PopAsync().ConfigureAwait(true);

            // Then
            viewModel.Received(1).Destroy();
        }

        /// <summary>
        /// Tests that popping a page calls WhenNavigatedTo.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Call_WhenNavigatedTo()
        {
            // Given
            var source = new BehaviorSubject<NavigationSource>(NavigationSource.Device);
            var viewModel = Substitute.For<INavigated>();
            var navigationPage = new NavigationPage();
            NavigationPageSystemPopBehavior sut = new(source);
            navigationPage.Behaviors.Add(sut);

            // When
            await navigationPage.PushAsync(new Page { BindingContext = viewModel }).ConfigureAwait(true);
            await navigationPage.PushAsync(new Page()).ConfigureAwait(true);
            await navigationPage.PopAsync().ConfigureAwait(true);

            // Then
            viewModel.Received(1).WhenNavigatedTo(Arg.Any<INavigationParameter>());
        }

        /// <summary>
        /// Tests that popping a page executes subscription when NavigationSource is Device.
        /// </summary>
        /// <returns>A completion notification.</returns>
        [Fact]
        public async Task Should_Not_Execute_When_Navigation_Service_Pop()
        {
            // Given
            var source = new BehaviorSubject<NavigationSource>(NavigationSource.NavigationService);
            var viewModel1 = Substitute.For<IEverything>();
            var viewModel2 = Substitute.For<INavigated>();
            var navigationPage = new NavigationPage();
            NavigationPageSystemPopBehavior sut = new(source);
            navigationPage.Behaviors.Add(sut);

            // When
            await navigationPage.PushAsync(new Page { BindingContext = viewModel1 }).ConfigureAwait(true);
            await navigationPage.PushAsync(new Page { BindingContext = viewModel2 }).ConfigureAwait(true);
            await navigationPage.PopAsync().ConfigureAwait(true);

            // Then
            viewModel1.DidNotReceive().WhenNavigatedTo(Arg.Any<INavigationParameter>());
            viewModel2.DidNotReceive().WhenNavigatedFrom(Arg.Any<INavigationParameter>());
            viewModel1.DidNotReceive().Destroy();
        }
    }
}
