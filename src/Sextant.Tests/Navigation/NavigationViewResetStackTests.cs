// Copyright (c) 2025 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using NSubstitute;
using NUnit.Framework;
using ReactiveUI;
using Sextant.Maui;
using Sextant.Mocks;
using Splat;

namespace Sextant.Tests.Navigation;

/// <summary>
/// Tests for NavigationView resetStack behavior.
/// </summary>
[TestFixture]
public sealed class NavigationViewResetStackTests
{
    private IViewLocator _viewLocator = null!;
    private TestViewModel _testViewModel = null!;

    /// <summary>
    /// Set up test fixtures.
    /// </summary>
    [SetUp]
    public void SetUp()
    {
        Locator.CurrentMutable.InitializeSplat();
        Locator.CurrentMutable.InitializeReactiveUI();

        _viewLocator = Substitute.For<IViewLocator>();
        _testViewModel = new TestViewModel();

        // Return a new TestPage for each call to avoid "Page must not already have a parent" errors
        _viewLocator.ResolveView(Arg.Any<object>(), Arg.Any<string>()).Returns(callInfo => new TestPage());
    }

    /// <summary>
    /// Tests that resetStack=true completely clears the navigation stack when it's not empty.
    /// </summary>
    /// <returns>A completion notification.</returns>
    [Test]
    public async Task PushPage_WithResetStackTrue_WhenStackNotEmpty_ShouldClearEntireStack()
    {
        // Given
        var initialPage = new TestPage();
        var navigationView = new NavigationView(CurrentThreadScheduler.Instance, CurrentThreadScheduler.Instance, _viewLocator, initialPage);

        // Add some pages to the stack
        await navigationView.PushPage(_testViewModel, null, false, false).FirstAsync();
        await navigationView.PushPage(_testViewModel, null, false, false).FirstAsync();

        // Verify stack has pages
        Assert.That(navigationView.Navigation.NavigationStack.Count, Is.EqualTo(3)); // root + 2 pushed pages

        // When - push new page with resetStack=true
        var newViewModel = new TestViewModel();
        await navigationView.PushPage(newViewModel, null, true, false).FirstAsync();

        // Then - navigation stack should only contain the new page
        Assert.Multiple(() =>
        {
            Assert.That(navigationView.Navigation.NavigationStack.Count, Is.EqualTo(1));
            Assert.That(navigationView.Navigation.NavigationStack[0], Is.TypeOf<TestPage>());
            Assert.That(navigationView.Navigation.NavigationStack[0].BindingContext, Is.EqualTo(newViewModel));
        });
    }

    /// <summary>
    /// Tests that resetStack=true works when navigation stack is empty.
    /// </summary>
    /// <returns>A completion notification.</returns>
    [Test]
    public async Task PushPage_WithResetStackTrue_WhenStackEmpty_ShouldPushPageNormally()
    {
        // Given
        var navigationView = new NavigationView(CurrentThreadScheduler.Instance, CurrentThreadScheduler.Instance, _viewLocator);

        // Verify stack is empty
        Assert.That(navigationView.Navigation.NavigationStack.Count, Is.EqualTo(0));

        // When - push page with resetStack=true on empty stack
        await navigationView.PushPage(_testViewModel, null, true, false).FirstAsync();

        // Then - page should be added normally
        Assert.Multiple(() =>
        {
            Assert.That(navigationView.Navigation.NavigationStack.Count, Is.EqualTo(1));
            Assert.That(navigationView.Navigation.NavigationStack[0], Is.TypeOf<TestPage>());
            Assert.That(navigationView.Navigation.NavigationStack[0].BindingContext, Is.EqualTo(_testViewModel));
        });
    }

    /// <summary>
    /// Tests that resetStack=false preserves existing navigation behavior.
    /// </summary>
    /// <returns>A completion notification.</returns>
    [Test]
    public async Task PushPage_WithResetStackFalse_ShouldPreserveExistingBehavior()
    {
        // Given
        var initialPage = new TestPage();
        var navigationView = new NavigationView(CurrentThreadScheduler.Instance, CurrentThreadScheduler.Instance, _viewLocator, initialPage);

        // Add a page to the stack
        await navigationView.PushPage(_testViewModel, null, false, false).FirstAsync();

        // Verify stack has pages
        Assert.That(navigationView.Navigation.NavigationStack.Count, Is.EqualTo(2)); // root + 1 pushed page

        // When - push new page with resetStack=false
        var newViewModel = new TestViewModel();
        await navigationView.PushPage(newViewModel, null, false, false).FirstAsync();

        // Then - navigation stack should contain all pages
        Assert.Multiple(() =>
        {
            Assert.That(navigationView.Navigation.NavigationStack.Count, Is.EqualTo(3));
            Assert.That(navigationView.Navigation.NavigationStack[2].BindingContext, Is.EqualTo(newViewModel));
        });
    }

    /// <summary>
    /// Tests that page titles are set correctly when using resetStack.
    /// </summary>
    /// <returns>A completion notification.</returns>
    [Test]
    public async Task PushPage_WithResetStack_ShouldSetPageTitle()
    {
        // Given
        var navigationView = new NavigationView(CurrentThreadScheduler.Instance, CurrentThreadScheduler.Instance, _viewLocator);
        _testViewModel.Id = "TestPageTitle";

        // When
        await navigationView.PushPage(_testViewModel, null, true, false).FirstAsync();

        // Then
        var pushedPage = navigationView.Navigation.NavigationStack[0];
        Assert.That(pushedPage.Title, Is.EqualTo("TestPageTitle"));
    }

    /// <summary>
    /// Tests that view locator is called correctly when using resetStack.
    /// </summary>
    /// <returns>A completion notification.</returns>
    [Test]
    public async Task PushPage_WithResetStack_ShouldCallViewLocatorCorrectly()
    {
        // Given
        var navigationView = new NavigationView(CurrentThreadScheduler.Instance, CurrentThreadScheduler.Instance, _viewLocator);
        const string contract = "TestContract";

        // When
        await navigationView.PushPage(_testViewModel, contract, true, false).FirstAsync();

        // Then
        _viewLocator.Received(1).ResolveView(_testViewModel, contract);
    }

    /// <summary>
    /// Tests that BindingContext is set correctly when using resetStack.
    /// </summary>
    /// <returns>A completion notification.</returns>
    [Test]
    public async Task PushPage_WithResetStack_ShouldSetBindingContext()
    {
        // Given
        var navigationView = new NavigationView(CurrentThreadScheduler.Instance, CurrentThreadScheduler.Instance, _viewLocator);

        // When
        await navigationView.PushPage(_testViewModel, null, true, false).FirstAsync();

        // Then
        var pushedPage = navigationView.Navigation.NavigationStack[0];
        Assert.That(pushedPage.BindingContext, Is.EqualTo(_testViewModel));
    }

    /// <summary>
    /// Test page for mocking.
    /// </summary>
    private sealed class TestPage : ContentPage, IViewFor<TestViewModel>
    {
        private TestViewModel? _viewModel;

        /// <inheritdoc />
        public TestViewModel? ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                BindingContext = value;
            }
        }

        /// <inheritdoc />
        object? IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TestViewModel?)value;
        }
    }

    /// <summary>
    /// Test view model for testing.
    /// </summary>
    private sealed class TestViewModel : ReactiveObject, IViewModel
    {
        /// <inheritdoc />
        public string? Id { get; set; }
    }
}
