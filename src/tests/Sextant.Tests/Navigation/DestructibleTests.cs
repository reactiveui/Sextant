// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Linq;
using System.Threading.Tasks;
using Sextant.Mocks;

namespace Sextant.Tests;

/// <summary>Groups the DestructibleTests navigation checks.</summary>
public static class DestructibleTests
{
    /// <summary>Tests the destroy method.</summary>
    public class TheDestroyMethod
    {
        /// <summary>Should unwrap the parameters.</summary>
        /// <returns>A task that completes after the assertions.</returns>
        [Test]
        public async Task Should_Destroy()
        {
            // Given
            using ParameterViewModel sut = new();

            // When & Then
            using (Assert.Multiple())
            {
                await Assert.That(sut.Disposable.IsDisposed).IsFalse();
                sut.Destroy();
                await Assert.That(sut.Disposable.IsDisposed).IsTrue();
            }
        }

        /// <summary>Tests to make sure we receive a push page notification.</summary>
        /// <returns>A completion notification.</returns>
        [Test]
        public async Task Should_Call_When_Page_Popped()
        {
            // Given
            var viewModel1 = new DestructibleViewModel();
            var viewModel2 = new DestructibleViewModel();
            using var view = new NavigationViewMock();
            using var sut = new ParameterViewStackService(view, new ViewModelFactoryMock(), new Splat.WrappingFullLogger(new Splat.NullLogger()));

            // When
            await sut.PushPage(viewModel1);
            await sut.PushPage(viewModel2);
            await sut.PopPage();

            // Then
            await Assert.That(viewModel2.DestroyCount).IsEqualTo(1);
        }
    }

    /// <summary>Records destruction of a page view model.</summary>
    private sealed class DestructibleViewModel : IViewModel, IDestructible
    {
        /// <inheritdoc />
        public string? Id { get; } = nameof(DestructibleViewModel);

        /// <summary>Gets the number of destruction calls.</summary>
        public int DestroyCount { get; private set; }

        /// <inheritdoc />
        public void Destroy() => DestroyCount++;
    }
}
