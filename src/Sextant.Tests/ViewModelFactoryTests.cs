// Copyright (c) 2025 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using NUnit.Framework;
using Splat;

namespace Sextant.Tests;

/// <summary>
/// Tests the <see cref="ViewModelFactory"/>.
/// </summary>
[TestFixture]
public sealed class ViewModelFactoryTests
{
    /// <summary>
    /// Tests the currently registered view model factory parameter.
    /// </summary>
    [TestFixture]
    public class CurrentPropertyTests
    {
        /// <summary>
        /// Sets up the test by unregistering all IViewModelFactory instances.
        /// </summary>
        [SetUp]
        public void SetUp() => Locator.CurrentMutable.UnregisterAll<IViewModelFactory>();

        /// <summary>
        /// Should throw if the IViewFactory is not registered.
        /// </summary>
        [Test]
        public void Should_Throw_If_Not_Registered()
        {
            // Given, When, Then
            var exception = Assert.Throws<ViewModelFactoryNotFoundException>(() =>
            {
                var dummy = ViewModelFactory.Current;
            });
            Assert.That(exception!.Message, Is.EqualTo("Could not find a default ViewModelFactory. This should never happen, your dependency resolver is broken"));
        }

        /// <summary>
        /// Should return the default view model factory.
        /// </summary>
        [Test]
        public void Should_Return_View_Model_Factory()
        {
            // Given, When
            Locator.CurrentMutable.Register(() => new DefaultViewModelFactory(), typeof(IViewModelFactory));
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
