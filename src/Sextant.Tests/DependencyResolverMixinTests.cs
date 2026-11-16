// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using NUnit.Framework;
using Sextant.Mocks;
using Splat;

namespace Sextant.Tests;

/// <summary>Groups the DependencyResolverMixinTests navigation checks.</summary>
public static class DependencyResolverMixinTests
{
    /// <summary>Tests the register view model factory method.</summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class TheRegisterViewModelFactoryMethod
    {
        /// <summary>Sets up the test by clearing the dependency resolver.</summary>
        [SetUp]
        public void SetUp() => AppLocator.CurrentMutable.UnregisterAll<IViewModelFactory>();

        /// <summary>Should register the view model factory.</summary>
        [Test]
        public void Should_Register_View_Model_Factory()
        {
            // Given
            _ = AppLocator.CurrentMutable.RegisterViewModelFactory();

            // Then
            Assert.That(ViewModelFactory.Current, Is.TypeOf<DefaultViewModelFactory>());
        }

        /// <summary>Should register the view model factory.</summary>
        [Test]
        public void Should_Register_View_Model_Factory_With_Factory()
        {
            // Given
            var viewModelFactory = new DefaultViewModelFactory();
            _ = AppLocator.CurrentMutable.RegisterViewModelFactory(() => viewModelFactory);

            // Then
            Assert.That(ViewModelFactory.Current, Is.EqualTo(viewModelFactory));
        }
    }

    /// <summary>Tests the register view method.</summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class TheRegisterViewMethod
    {
        /// <summary>Sets up the test by clearing the dependency resolver.</summary>
        [SetUp]
        public void SetUp() => AppLocator.CurrentMutable.UnregisterAll<IViewFor<NavigableViewModelMock>>();

        /// <summary>Should register the view stack service.</summary>
        [Test]
        public void Should_Register_View()
        {
            // Given
            _ = AppLocator.CurrentMutable.RegisterViewForNavigation(static () => new PageView(), static () => new NavigableViewModelMock());

            // When
            var result = AppLocator.Current.GetService<IViewFor<NavigableViewModelMock>>();

            // Then
            Assert.That(result, Is.TypeOf<PageView>());
        }

        /// <summary>Should register the view stack service factory.</summary>
        [Test]
        public void Should_Register_View_Factory()
        {
            // Given
            var view = new PageView();
            _ = AppLocator.CurrentMutable.RegisterViewForNavigation(() => view, static () => new NavigableViewModelMock());

            // When
            var result = AppLocator.Current.GetService<IViewFor<NavigableViewModelMock>>();

            // Then
            Assert.That(result, Is.SameAs(view));
        }
    }

    /// <summary>Tests the register view for navigation method.</summary>
    [TestFixture]
    [NonParallelizable]
    public sealed class TheRegisterViewForNavigationMethod
    {
        /// <summary>Sets up the test by clearing the dependency resolver.</summary>
        [SetUp]
        public void SetUp()
        {
            AppLocator.CurrentMutable.UnregisterAll<IViewFor<NavigableViewModelMock>>();
            AppLocator.CurrentMutable.UnregisterAll<NavigableViewModelMock>();
        }

        /// <summary>Should register the view for navigation.</summary>
        [Test]
        public void Should_Register_View_Factory_For_Navigation()
        {
            // Given
            _ = AppLocator.CurrentMutable.RegisterViewForNavigation(static () => new PageView(), static () => new NavigableViewModelMock());

            // When
            var result = AppLocator.Current.GetService<IViewFor<NavigableViewModelMock>>();

            // Then
            Assert.That(result, Is.TypeOf<PageView>());
        }

        /// <summary>Should register the view for navigation.</summary>
        [Test]
        public void Should_Register_ViewModel_Factory_For_Navigation()
        {
            // Given
            _ = AppLocator.CurrentMutable.RegisterViewForNavigation(static () => new PageView(), static () => new NavigableViewModelMock());

            // When
            var result = AppLocator.Current.GetService<NavigableViewModelMock>();

            // Then
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>Should register the view for navigation.</summary>
        [Test]
        public void Should_Register_View_For_Navigation()
        {
            // Given
            _ = AppLocator.CurrentMutable.RegisterViewForNavigation(new PageView(), new NavigableViewModelMock());

            // When
            var result = AppLocator.Current.GetService<IViewFor<NavigableViewModelMock>>();

            // Then
            Assert.That(result, Is.TypeOf<PageView>());
        }

        /// <summary>Should register the view for navigation.</summary>
        [Test]
        public void Should_Register_ViewModel_For_Navigation()
        {
            // Given
            var viewModel = new NavigableViewModelMock();
            _ = AppLocator.CurrentMutable.RegisterViewForNavigation(new PageView(), viewModel);

            // When
            var result = AppLocator.Current.GetService<NavigableViewModelMock>();

            // Then
            Assert.That(result, Is.SameAs(viewModel));
        }
    }
}
