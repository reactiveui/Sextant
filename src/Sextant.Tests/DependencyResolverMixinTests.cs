// Copyright (c) 2025 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using NUnit.Framework;
using ReactiveUI;
using Sextant.Mocks;
using Splat;

namespace Sextant.Tests;

/// <summary>
/// Tests the IMutableDependencyResolver extension class.
/// </summary>
[TestFixture]
[NonParallelizable]
public sealed class DependencyResolverMixinTests
{
    /// <summary>
    /// Tests the register view model factory method.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class TheRegisterViewModelFactoryMethod
    {
        /// <summary>
        /// Sets up the test by clearing the dependency resolver.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Locator.CurrentMutable.UnregisterAll<IViewModelFactory>();
        }

        /// <summary>
        /// Should register the view model factory.
        /// </summary>
        [Test]
        public void Should_Register_View_Model_Factory()
        {
            // Given
            Locator.CurrentMutable.RegisterViewModelFactory();

            // When
            var result = ViewModelFactory.Current;

            // Then
            Assert.That(result, Is.TypeOf<DefaultViewModelFactory>());
        }

        /// <summary>
        /// Should register the view model factory.
        /// </summary>
        [Test]
        public void Should_Register_View_Model_Factory_With_Factory()
        {
            // Given
            var viewModelFactory = new DefaultViewModelFactory();
            Locator.CurrentMutable.RegisterViewModelFactory(() => viewModelFactory);

            // When
            var result = ViewModelFactory.Current;

            // Then
            Assert.That(result, Is.EqualTo(viewModelFactory));
        }
    }

    /// <summary>
    /// Tests the register view method.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class TheRegisterViewMethod
    {
        /// <summary>
        /// Sets up the test by clearing the dependency resolver.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Locator.CurrentMutable.UnregisterAll<IViewFor<NavigableViewModelMock>>();
        }

        /// <summary>
        /// Should register the view stack service.
        /// </summary>
        [Test]
        public void Should_Register_View()
        {
            // Given
            Locator.CurrentMutable.RegisterViewForNavigation<PageView, NavigableViewModelMock>();

            // When
            var result = Locator.Current.GetService<IViewFor<NavigableViewModelMock>>();

            // Then
            Assert.That(result, Is.TypeOf<PageView>());
        }

        /// <summary>
        /// Should register the view stack service factory.
        /// </summary>
        [Test]
        public void Should_Register_View_Factory()
        {
            // Given
            Locator.CurrentMutable.RegisterViewForNavigation(() => new PageView(), () => new NavigableViewModelMock());
            ////Locator.CurrentMutable.RegisterView<PageView, NavigableViewModelMock>(() => new PageView());

            // When
            var result = Locator.Current.GetService<IViewFor<NavigableViewModelMock>>();

            // Then
            Assert.That(result, Is.TypeOf<PageView>());
        }
    }

    /// <summary>
    /// Tests the register view for navigation method.
    /// </summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class TheRegisterViewForNavigationMethod
    {
        /// <summary>
        /// Sets up the test by clearing the dependency resolver.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            Locator.CurrentMutable.UnregisterAll<IViewFor<NavigableViewModelMock>>();
            Locator.CurrentMutable.UnregisterAll<NavigableViewModelMock>();
        }

        /// <summary>
        /// Should register the view for navigation.
        /// </summary>
        [Test]
        public void Should_Register_View_Factory_For_Navigation()
        {
            // Given
            Locator.CurrentMutable.RegisterViewForNavigation(() => new PageView(), () => new NavigableViewModelMock());

            // When
            var result = Locator.Current.GetService<IViewFor<NavigableViewModelMock>>();

            // Then
            Assert.That(result, Is.TypeOf<PageView>());
        }

        /// <summary>
        /// Should register the view for navigation.
        /// </summary>
        [Test]
        public void Should_Register_ViewModel_Factory_For_Navigation()
        {
            // Given
            Locator.CurrentMutable.RegisterViewForNavigation(() => new PageView(), () => new NavigableViewModelMock());

            // When
            var result = Locator.Current.GetService<NavigableViewModelMock>();

            // Then
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Should register the view for navigation.
        /// </summary>
        [Test]
        public void Should_Register_View_For_Navigation()
        {
            // Given
            Locator.CurrentMutable.RegisterViewForNavigation(new PageView(), new NavigableViewModelMock());

            // When
            var result = Locator.Current.GetService<IViewFor<NavigableViewModelMock>>();

            // Then
            Assert.That(result, Is.TypeOf<PageView>());
        }

        /// <summary>
        /// Should register the view for navigation.
        /// </summary>
        [Test]
        public void Should_Register_ViewModel_For_Navigation()
        {
            // Given
            Locator.CurrentMutable.RegisterViewForNavigation(() => new PageView(), () => new NavigableViewModelMock());

            // When
            var result = Locator.Current.GetService<NavigableViewModelMock>();

            // Then
            Assert.That(result, Is.Not.Null);
        }
    }
}
