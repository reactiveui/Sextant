// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using Sextant;

namespace NavigationLifecycle;

/// <summary>Implements the platform side of a page stack without opening a window.</summary>
internal sealed class NavigationHost : IView, IDisposable
{
    /// <summary>Tracks the pages the host can report as popped.</summary>
    private readonly Stack<IViewModel> _pages = new();

    /// <summary>Reports back navigation to the service.</summary>
    private readonly Subject<IViewModel?> _popped = new();

    /// <inheritdoc/>
    public IScheduler MainThreadScheduler => ImmediateScheduler.Instance;

    /// <inheritdoc/>
    public IObservable<IViewModel?> PagePopped => _popped;

    /// <inheritdoc/>
    public IObservable<Unit> PushPage(IViewModel viewModel, string? contract, bool resetStack, bool animate)
    {
        if (resetStack)
        {
            _pages.Clear();
        }

        _pages.Push(viewModel);
        return Observable.Return(Unit.Default);
    }

    /// <inheritdoc/>
    public IObservable<Unit> PopPage(bool animate)
    {
        _popped.OnNext(_pages.Pop());
        return Observable.Return(Unit.Default);
    }

    /// <inheritdoc/>
    public IObservable<Unit> PopToRootPage(bool animate)
    {
        while (_pages.Count > 1)
        {
            _popped.OnNext(_pages.Pop());
        }

        return Observable.Return(Unit.Default);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract, bool withNavigationPage) => Observable.Return(Unit.Default);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopModal() => Observable.Return(Unit.Default);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose() => _popped.Dispose();
}
