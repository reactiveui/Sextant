// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using Splat;

namespace Sextant.Tests;

/// <summary>Groups the ViewModelFactoryTests navigation checks.</summary>
public static class ViewModelFactoryTests
{
    /// <summary>Tests the currently registered view model factory parameter.</summary>
    public class CurrentPropertyTests
    {
        /// <summary>Sets up the test by unregistering all IViewModelFactory instances.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [Before(Test)]
        public void SetUp() => AppLocator.CurrentMutable.UnregisterAll<IViewModelFactory>();

        /// <summary>Should throw if the IViewFactory is not registered.</summary>
        /// <returns>A task that completes after the assertions.</returns>
        [Test]
        public async Task Should_Throw_If_Not_Registered()
        {
            // Given, When, Then
            var exception = await Assert.That(static () =>
            {
                _ = ViewModelFactory.Current;
            }).Throws<ViewModelFactoryNotFoundException>();
            await Assert.That(exception!.Message).IsEqualTo("Could not find a default ViewModelFactory. This should never happen, your dependency resolver is broken");
        }

        /// <summary>Should return the default view model factory.</summary>
        /// <returns>A task that completes after the assertions.</returns>
        [Test]
        public async Task Should_Return_View_Model_Factory()
        {
            AppLocator.CurrentMutable.Register(static () => new DefaultViewModelFactory(), typeof(IViewModelFactory));
            var viewModelFactory = ViewModelFactory.Current;

            // Then
            using (Assert.Multiple())
            {
                await Assert.That(viewModelFactory).IsAssignableTo<IViewModelFactory>();
                await Assert.That(viewModelFactory).IsTypeOf<DefaultViewModelFactory>();
            }
        }
    }
}
