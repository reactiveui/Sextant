// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using NUnit.Framework;
using Splat;

namespace Sextant.Tests;

/// <summary>Groups the ViewModelFactoryTests navigation checks.</summary>
public static class ViewModelFactoryTests
{
    /// <summary>Tests the currently registered view model factory parameter.</summary>
    [TestFixture]
    [NonParallelizable]
    public class CurrentPropertyTests
    {
        /// <summary>Sets up the test by unregistering all IViewModelFactory instances.</summary>
        [SetUp]
        public void SetUp() => AppLocator.CurrentMutable.UnregisterAll<IViewModelFactory>();

        /// <summary>Should throw if the IViewFactory is not registered.</summary>
        [Test]
        public void Should_Throw_If_Not_Registered()
        {
            // Given, When, Then
            var exception = Assert.Throws<ViewModelFactoryNotFoundException>(static () =>
            {
            });
            Assert.That(exception!.Message, Is.EqualTo("Could not find a default ViewModelFactory. This should never happen, your dependency resolver is broken"));
        }

        /// <summary>Should return the default view model factory.</summary>
        [Test]
        public void Should_Return_View_Model_Factory()
        {
            AppLocator.CurrentMutable.Register(static () => new DefaultViewModelFactory(), typeof(IViewModelFactory));
            var viewModelFactory = ViewModelFactory.Current;

            // Then
            Assert.Multiple(() =>
            {
                Assert.That(viewModelFactory, Is.AssignableTo<IViewModelFactory>());
                Assert.That(viewModelFactory, Is.TypeOf<DefaultViewModelFactory>());
            });
        }
    }
}
