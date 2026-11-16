// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Sextant.Mocks;
using Splat;

namespace Sextant.Tests;

/// <summary>Groups the DestructibleTests navigation checks.</summary>
public static class DestructibleTests
{
    /// <summary>Tests the destroy method.</summary>
    [TestFixture]
    [NonParallelizable]
    public class TheDestroyMethod
    {
        /// <summary>Should unwrap the parameters.</summary>
        [Test]
        public void Should_Destroy()
        {
            // Given
            ParameterViewModel sut = new();

            // When & Then
            Assert.Multiple(() =>
            {
                Assert.That(sut.Disposable.IsDisposed, Is.False);
                sut.Destroy();
                Assert.That(sut.Disposable.IsDisposed, Is.True);
            });
        }

        /// <summary>Tests to make sure we receive a push page notification.</summary>
        /// <returns>A completion notification.</returns>
        [Test]
        public async Task Should_Call_When_Page_Popped()
        {
            AppLocator.CurrentMutable.InitializeSplat();
            _ = RxAppBuilder.CreateReactiveUIBuilder().WithCoreServices().BuildApp();

            // Given
            var viewModel1 = Substitute.For<IDestructibleMock>();
            var viewModel2 = Substitute.For<IDestructibleMock>();
            ParameterViewStackService sut = new ParameterViewStackServiceFixture().WithView(new NavigationViewMock());

            // When
            await sut.PushPage(viewModel1);
            await sut.PushPage(viewModel2);
            await sut.PopPage();

            // Then
            viewModel2.Received(1).Destroy();
        }
    }
}
