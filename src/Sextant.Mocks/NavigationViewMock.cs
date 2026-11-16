// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace Sextant.Mocks;

/// <summary>Mock <see cref = "IView"/> implementation.</summary>
[System.Diagnostics.DebuggerDisplay("NavigationViewMock: {MainThreadScheduler}")]
public class NavigationViewMock : IView, IDisposable
{
    /// <summary>Notifies the service when a page leaves the stack.</summary>
    private readonly Subject<IViewModel> _pagePoppedSubject = new();

    /// <summary>The page history used by mock navigation.</summary>
    private readonly Stack<IViewModel> _pageStack = new();

    /// <summary>The modal history used by mock navigation.</summary>
    private readonly Stack<IViewModel> _modalStack = new();

    /// <summary>Initializes a new instance of the <see cref = "NavigationViewMock"/> class.</summary>
    public NavigationViewMock() => PagePopped = _pagePoppedSubject.AsObservable();

    /// <inheritdoc/>
    public IScheduler MainThreadScheduler { get; } = CurrentThreadScheduler.Instance;

    /// <inheritdoc/>
    public IObservable<IViewModel?> PagePopped { get; }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopModal() => Observable.Return(Unit.Default).Do(unit => { _ = _modalStack.Pop(); });

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopPage() => PopPage(true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopPage(bool animate) => Observable.Return(Unit.Default).Do(_ => _pagePoppedSubject.OnNext(_pageStack.Pop()));

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopToRootPage() => PopToRootPage(true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopToRootPage(bool animate) => Observable.Return(Unit.Default).Do(unit =>
    {
        while (_pageStack.Count > 1)
        {
            _pagePoppedSubject.OnNext(_pageStack.Pop());
        }
    });

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract) => PushModal(modalViewModel, contract, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract, bool withNavigationPage) =>
        Observable.Return(Unit.Default).Do(unit => _modalStack.Push(modalViewModel));

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(IViewModel viewModel, string? contract, bool resetStack) => PushPage(viewModel, contract, resetStack, true);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(IViewModel viewModel, string? contract, bool resetStack, bool animate) => Observable.Return(Unit.Default).Do(unit =>
    {
        if (resetStack)
        {
            _pageStack.Clear();
        }

        _pageStack.Push(viewModel);
    });

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
    /// <param name = "disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _pagePoppedSubject.Dispose();
        }
    }
}
