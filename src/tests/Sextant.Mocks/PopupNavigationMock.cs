// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using Mopups.Events;
using Mopups.Interfaces;
using Mopups.Pages;

namespace Sextant.Mocks;

/// <summary>Runs popup operations in memory while preserving their event order.</summary>
internal sealed class PopupNavigationMock : IPopupNavigation
{
    /// <summary>The currently displayed popups.</summary>
    private readonly List<PopupPage> _pages = [];

    /// <inheritdoc />
    public event EventHandler<PopupNavigationEventArgs>? Pushing;

    /// <inheritdoc />
    public event EventHandler<PopupNavigationEventArgs>? Pushed;

    /// <inheritdoc />
    public event EventHandler<PopupNavigationEventArgs>? Popping;

    /// <inheritdoc />
    public event EventHandler<PopupNavigationEventArgs>? Popped;

    /// <inheritdoc />
    public IReadOnlyList<PopupPage> PopupStack => _pages;

    /// <inheritdoc />
    public Task PushAsync(PopupPage page, bool animate)
    {
        var args = new PopupNavigationEventArgs(page, animate);
        Pushing?.Invoke(this, args);
        _pages.Add(page);
        Pushed?.Invoke(this, args);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task PopAsync(bool animate) => RemovePageAsync(_pages[^1], animate);

    /// <inheritdoc />
    public async Task PopAllAsync(bool animate)
    {
        while (_pages.Count > 0)
        {
            await PopAsync(animate);
        }
    }

    /// <inheritdoc />
    public Task RemovePageAsync(PopupPage page, bool animate)
    {
        var args = new PopupNavigationEventArgs(page, animate);
        Popping?.Invoke(this, args);
        _ = _pages.Remove(page);
        Popped?.Invoke(this, args);
        return Task.CompletedTask;
    }

    /// <summary>Adds a page without publishing navigation events.</summary>
    /// <param name="page">The page to add.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal void Seed(PopupPage page) => _pages.Add(page);
}
