// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;

namespace Sextant.Mocks;

/// <summary>Null view model.</summary>
[System.Diagnostics.DebuggerDisplay("NullViewModelMock: {Id}")]
public class NullViewModelMock : INavigable, IDisposable
{
    /// <summary>Signals completed navigation to this view model.</summary>
    private readonly Subject<Unit> _navigatedTo;

    /// <summary>Signals navigation starting for this view model.</summary>
    private readonly Subject<Unit> _navigatingTo;

    /// <summary>Signals navigation away from this view model.</summary>
    private readonly Subject<Unit> _navigatedFrom;

    /// <summary>Ensures the owned subjects are released once.</summary>
    private int _disposedValue;

    /// <summary>Initializes a new instance of the <see cref="NullViewModelMock"/> class.</summary>
    public NullViewModelMock()
    {
        _navigatedTo = new();
        _navigatedFrom = new();
        _navigatingTo = new();
    }

    /// <summary>Gets the ID of the page.</summary>
    public string Id => nameof(NullViewModelMock);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) =>
        Observable.Return(Unit.Default).Do(_ => _navigatedTo.OnNext(Unit.Default));

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) =>
        Observable.Return(Unit.Default).Do(_ => _navigatedFrom.OnNext(Unit.Default));

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) =>
        Observable.Return(Unit.Default).Do(_ => _navigatingTo.OnNext(Unit.Default));

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || Interlocked.Exchange(ref _disposedValue, 1) != 0)
        {
            return;
        }

        _navigatedFrom.Dispose();
        _navigatedTo.Dispose();
        _navigatingTo.Dispose();
    }
}
