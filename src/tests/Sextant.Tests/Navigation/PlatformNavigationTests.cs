// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;
using Sextant.Mocks;
using AvaloniaNavigation = Sextant.Avalonia.NavigationView;
using MauiNavigation = Sextant.Maui.NavigationView;

namespace Sextant.Tests.Navigation;

/// <summary>Checks the platform navigation adapters using managed controls.</summary>
public sealed class PlatformNavigationTests
{
    /// <summary>The number of pages remaining when a navigation root was supplied.</summary>
    private const int RootAndPushedPageCount = 2;

    /// <summary>Checks that a disposed count observer stops receiving stack changes.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task AvaloniaNavigationStopsCountNotificationsAfterDisposal()
    {
        var navigation = new AvaloniaNavigation.Navigation();
        var count = -1;
        var subscription = navigation.CountChanged.Subscribe(value => count = value);
        navigation.Push(new AvaloniaPage());
        await Assert.That(count).IsEqualTo(1);
        subscription.Dispose();
        navigation.Push(new AvaloniaPage());
        await Assert.That(count).IsEqualTo(1);
    }

    /// <summary>Checks MAUI page resolution, reset and device pop events.</summary>
    /// <param name="withRoot">Whether navigation starts with a root page.</param>
    /// <returns>The asynchronous test.</returns>
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task MauiPagesReportPopsAndReplaceTheRoot(bool withRoot)
    {
        using var model = new NavigableViewModelMock();
        var locator = new PlatformViewLocator { Factory = static () => new MauiPage() };
        using var navigation = withRoot
            ? new DisposableNavigation(locator, new MauiPage())
            : new DisposableNavigation(locator);
        using var subscription = navigation.PagePopped.Subscribe();
        await navigation.PushPage(model, null, false, false);
        await navigation.PushPage(model, null, false, false);
        await navigation.PopPage(false);
        await Assert.That(navigation.Navigation.NavigationStack.Count).IsEqualTo(withRoot ? RootAndPushedPageCount : 1);
        await navigation.Navigation.PushAsync(new ContentPage());
        _ = await navigation.Navigation.PopAsync(false);
        await navigation.PushPage(model, null, true, false);
        await Assert.That(navigation.Navigation.NavigationStack.Count).IsEqualTo(1);
        await navigation.PushPage(model, null, true, false);
        await Assert.That(navigation.Navigation.NavigationStack.Count).IsEqualTo(1);
        await navigation.PushPage(model, null, false, false);
        await navigation.PopToRootPage(false);
        await Assert.That(navigation.Navigation.NavigationStack.Count).IsEqualTo(1);
        navigation.Release(false);
        navigation.Release(true);
        navigation.Release(true);
        await Assert.That(navigation.Behaviors.Count).IsEqualTo(0);
    }

    /// <summary>Checks missing and invalid MAUI views.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task MauiRejectsMissingAndNonPageViews()
    {
        using var model = new NavigableViewModelMock();
        var locator = new PlatformViewLocator();
        using var navigation = new MauiNavigation(ImmediateScheduler.Instance, locator);
        await Assert.That(async () => await navigation.PushPage(model, null, false, false)).Throws<InvalidOperationException>();
        locator.Factory = static () => new PageView();
        await Assert.That(async () => await navigation.PushPage(model, null, false, false)).Throws<InvalidOperationException>();
    }

    /// <summary>Checks modal wrapping and removal.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task MauiModalsSupportWrappedAndUnwrappedPages()
    {
        using var model = new NavigableViewModelMock();
        var locator = new PlatformViewLocator { Factory = static () => new MauiPage() };
        using var navigation = new MauiNavigation(ImmediateScheduler.Instance, locator, new ContentPage());
        await navigation.PushModal(model, null);
        await Assert.That(navigation.Navigation.ModalStack[0]).IsTypeOf<NavigationPage>();
        await navigation.PopModal();
        await navigation.PushModal(model, null, false);
        await Assert.That(navigation.Navigation.ModalStack[0]).IsTypeOf<MauiPage>();
        await navigation.PopModal();
        await Assert.That(navigation.Navigation.ModalStack.Count).IsEqualTo(0);
    }

    /// <summary>Checks Avalonia navigation layers, animation branches and view validation.</summary>
    /// <returns>The asynchronous test.</returns>
    [Test]
    public async Task AvaloniaPagesAndModalsUseIndependentLayers()
    {
        using var model = new NavigableViewModelMock();
        var locator = new PlatformViewLocator { Factory = static () => new AvaloniaPage() };
        var navigation = new AvaloniaNavigation(ImmediateScheduler.Instance, locator);
        await Assert.That(navigation.StyleKey).IsEqualTo(typeof(global::Avalonia.Controls.ContentControl));
        using var popped = navigation.PagePopped.Subscribe();
        await navigation.PushPage(model, null, false);
        await navigation.PushPage(model, null, false, false);
        await navigation.PopPage();
        await navigation.PushModal(model, null);
        await navigation.PushModal(model, null, false);
        await navigation.PushPage(model, null, false, true);
        await navigation.PopPage(true);
        await navigation.PopToRootPage(true);
        await navigation.PopModal();
        await navigation.PopModal();
        await navigation.PushPage(model, null, true, false);
        await navigation.PopToRootPage();
        await navigation.PopToRootPage(false);
        await navigation.PopPage(false);
        await navigation.PushPage(model, null, true, true);
        await navigation.PopPage(true);
        locator.Factory = static () => null;
        await Assert.That(() => navigation.PushPage(model, null, false)).Throws<InvalidOperationException>();
        await Assert.That(() => navigation.PushPage(null!, null, false)).Throws<ArgumentNullException>();
        await Assert.That(() => navigation.PushModal(null!, null)).Throws<ArgumentNullException>();
        var layers = (global::Avalonia.Controls.Grid)navigation.Content!;
        layers.Children[1].ClearValue(global::Avalonia.Controls.Panel.BackgroundProperty);
    }

    /// <summary>Allows protected disposal paths to be exercised.</summary>
    private sealed class DisposableNavigation : MauiNavigation
    {
        /// <summary>Initializes a new instance of the <see cref="DisposableNavigation"/> class.</summary>
        /// <param name="locator">The page locator.</param>
        public DisposableNavigation(IViewLocator locator)
            : base(ImmediateScheduler.Instance, locator)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DisposableNavigation"/> class.</summary>
        /// <param name="locator">The page locator.</param>
        /// <param name="root">The root page.</param>
        public DisposableNavigation(IViewLocator locator, Page root)
            : base(ImmediateScheduler.Instance, locator, root)
        {
        }

        /// <summary>Runs the protected disposal path.</summary>
        /// <param name="disposing">Whether to release managed resources.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Release(bool disposing) => Dispose(disposing);
    }

    /// <summary>A MAUI page whose binding context follows its view model.</summary>
    private sealed class MauiPage : ContentPage, IViewFor
    {
        /// <inheritdoc />
        public object? ViewModel { get => BindingContext; set => BindingContext = value; }
    }

    /// <summary>An Avalonia control whose data context follows its view model.</summary>
    private sealed class AvaloniaPage : global::Avalonia.Controls.ContentControl, IViewFor
    {
        /// <inheritdoc />
        public object? ViewModel { get => DataContext; set => DataContext = value; }
    }
}
