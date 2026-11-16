// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Sextant.Maui;

namespace Sextant.Tests.Navigation;

/// <summary>Tests for NavigationView resetStack behavior.</summary>
[System.Diagnostics.DebuggerDisplay("NavigationViewResetStackTests: {_viewLocator}")]
public sealed class NavigationViewResetStackTests
{
    /// <summary>The root page and two pushed pages.</summary>
    private const int FullStackCount = 3;

    /// <summary>The root page and one pushed page.</summary>
    private const int IntermediateStackCount = 2;

    /// <summary>Resolves the page used by each navigation operation.</summary>
    private TestViewLocator _viewLocator = null !;

    /// <summary>The view model shared by the initial page pushes.</summary>
    private TestViewModel _testViewModel = null !;

    /// <summary>Set up test fixtures.</summary>
    [Before(Test)]
    public void SetUp()
    {
        _viewLocator = new();
        _testViewModel = new();
    }

    /// <summary>Tests that resetStack=true completely clears the navigation stack when it's not empty.</summary>
    /// <returns>A completion notification.</returns>
    [Test]
    public async Task PushPage_WithResetStackTrue_WhenStackNotEmpty_ShouldClearEntireStack()
    {
        // Given
        var initialPage = new TestPage();
        using var navigationView = new NavigationView(CurrentThreadScheduler.Instance, _viewLocator, initialPage);

        // Add some pages to the stack
        await navigationView.PushPage(_testViewModel, null, false, false).FirstAsync();
        await navigationView.PushPage(_testViewModel, null, false, false).FirstAsync();

        // Verify stack has pages
        await Assert.That(navigationView.Navigation.NavigationStack.Count).IsEqualTo(FullStackCount); // root + 2 pushed pages

        // When - push new page with resetStack=true
        var newViewModel = new TestViewModel();
        await navigationView.PushPage(newViewModel, null, true, false).FirstAsync();

        // Then - navigation stack should only contain the new page
        using (Assert.Multiple())
        {
            await Assert.That(navigationView.Navigation.NavigationStack.Count).IsEqualTo(1);
            await Assert.That(navigationView.Navigation.NavigationStack[0]).IsTypeOf<TestPage>();
            await Assert.That(navigationView.Navigation.NavigationStack[0].BindingContext).IsEqualTo(newViewModel);
        }
    }

    /// <summary>Tests that resetStack=true works when navigation stack is empty.</summary>
    /// <returns>A completion notification.</returns>
    [Test]
    public async Task PushPage_WithResetStackTrue_WhenStackEmpty_ShouldPushPageNormally()
    {
        // Given
        using var navigationView = new NavigationView(CurrentThreadScheduler.Instance, _viewLocator);

        // Verify stack is empty
        await Assert.That(navigationView.Navigation.NavigationStack.Count).IsEqualTo(0);

        // When - push page with resetStack=true on empty stack
        await navigationView.PushPage(_testViewModel, null, true, false).FirstAsync();

        // Then - page should be added normally
        using (Assert.Multiple())
        {
            await Assert.That(navigationView.Navigation.NavigationStack.Count).IsEqualTo(1);
            await Assert.That(navigationView.Navigation.NavigationStack[0]).IsTypeOf<TestPage>();
            await Assert.That(navigationView.Navigation.NavigationStack[0].BindingContext).IsEqualTo(_testViewModel);
        }
    }

    /// <summary>Tests that resetStack=false preserves existing navigation behavior.</summary>
    /// <returns>A completion notification.</returns>
    [Test]
    public async Task PushPage_WithResetStackFalse_ShouldPreserveExistingBehavior()
    {
        // Given
        var initialPage = new TestPage();
        using var navigationView = new NavigationView(CurrentThreadScheduler.Instance, _viewLocator, initialPage);

        // Add a page to the stack
        await navigationView.PushPage(_testViewModel, null, false, false).FirstAsync();

        // Verify stack has pages
        await Assert.That(navigationView.Navigation.NavigationStack.Count).IsEqualTo(IntermediateStackCount); // root + 1 pushed page

        // When - push new page with resetStack=false
        var newViewModel = new TestViewModel();
        await navigationView.PushPage(newViewModel, null, false, false).FirstAsync();

        // Then - navigation stack should contain all pages
        using (Assert.Multiple())
        {
            await Assert.That(navigationView.Navigation.NavigationStack.Count).IsEqualTo(FullStackCount);
            await Assert.That(navigationView.Navigation.NavigationStack[2].BindingContext).IsEqualTo(newViewModel);
        }
    }

    /// <summary>Tests that page titles are set correctly when using resetStack.</summary>
    /// <returns>A completion notification.</returns>
    [Test]
    public async Task PushPage_WithResetStack_ShouldSetPageTitle()
    {
        // Given
        using var navigationView = new NavigationView(CurrentThreadScheduler.Instance, _viewLocator);
        _testViewModel.Id = "TestPageTitle";

        // When
        await navigationView.PushPage(_testViewModel, null, true, false).FirstAsync();

        // Then
        var pushedPage = navigationView.Navigation.NavigationStack[0];
        await Assert.That(pushedPage.Title).IsEqualTo("TestPageTitle");
    }

    /// <summary>Tests that view locator is called correctly when using resetStack.</summary>
    /// <returns>A completion notification.</returns>
    [Test]
    public async Task PushPage_WithResetStack_ShouldCallViewLocatorCorrectly()
    {
        // Given
        using var navigationView = new NavigationView(CurrentThreadScheduler.Instance, _viewLocator);
        const string contract = "TestContract";

        // When
        await navigationView.PushPage(_testViewModel, contract, true, false).FirstAsync();

        // Then
        await Assert.That(_viewLocator.ResolveCount).IsEqualTo(1);
        await Assert.That(_viewLocator.LastViewModel).IsSameReferenceAs(_testViewModel);
        await Assert.That(_viewLocator.LastContract).IsEqualTo(contract);
    }

    /// <summary>Tests that BindingContext is set correctly when using resetStack.</summary>
    /// <returns>A completion notification.</returns>
    [Test]
    public async Task PushPage_WithResetStack_ShouldSetBindingContext()
    {
        // Given
        using var navigationView = new NavigationView(CurrentThreadScheduler.Instance, _viewLocator);

        // When
        await navigationView.PushPage(_testViewModel, null, true, false).FirstAsync();

        // Then
        var pushedPage = navigationView.Navigation.NavigationStack[0];
        await Assert.That(pushedPage.BindingContext).IsEqualTo(_testViewModel);
    }

    /// <summary>Creates a fresh page and records navigation lookups.</summary>
    private sealed class TestViewLocator : IViewLocator
    {
        /// <summary>Gets the number of view lookups.</summary>
        public int ResolveCount { get; private set; }

        /// <summary>Gets the last view model requested.</summary>
        public object? LastViewModel { get; private set; }

        /// <summary>Gets the last contract requested.</summary>
        public string? LastContract { get; private set; }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IViewFor? ResolveView<TViewModel>(TViewModel viewModel, string? contract)
            where TViewModel : class => ResolveView((object?)viewModel, contract);

        /// <inheritdoc />
        public IViewFor? ResolveView(object? viewModel, string? contract)
        {
            ResolveCount++;
            LastViewModel = viewModel;
            LastContract = contract;

            // MAUI pages cannot belong to more than one navigation parent.
            return new TestPage();
        }

        /// <inheritdoc />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("Resolves a view from the view model's runtime type.")]
        public IViewFor? ResolveViewUnsafe(object? viewModel, string? contract) => ResolveView(viewModel, contract);
    }

    /// <summary>Test page for mocking.</summary>
    private sealed class TestPage : ContentPage, IViewFor<TestViewModel>
    {
        /// <inheritdoc/>
        public TestViewModel? ViewModel
        {
            get => field;
            set
            {
                field = value;
                BindingContext = value;
            }
        }

        /// <inheritdoc/>
        object? IViewFor.ViewModel { get => ViewModel; set => ViewModel = (TestViewModel?)value; }
    }

    /// <summary>Test view model for testing.</summary>
    private sealed class TestViewModel : ReactiveObject, IViewModel
    {
        /// <inheritdoc/>
        public string? Id { get; set; }
    }
}
