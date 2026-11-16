// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Sextant.Mocks;

/// <summary>A mock of a page view model.</summary>
/// <param name="id">The id of the page.</param>
/// <remarks>
/// Initializes a new instance of the <see cref="NavigableViewModelMock"/> class.
/// </remarks>
[System.Diagnostics.DebuggerDisplay("NavigableViewModelMock: {Id}")]
public class NavigableViewModelMock(string? id = null) : INavigable, IDisposable
{
    /// <summary>Signals completed navigation to this view model.</summary>
    private readonly Subject<Unit> _navigatedTo = new();

    /// <summary>Signals navigation starting for this view model.</summary>
    private readonly Subject<Unit> _navigatingTo = new();

    /// <summary>Signals navigation away from this view model.</summary>
    private readonly Subject<Unit> _navigatedFrom = new();

    /// <summary>Ensures the owned subjects are released once.</summary>
    private int _disposed;

    /// <summary>Initializes a new instance of the <see cref="NavigableViewModelMock"/> class.</summary>
    public NavigableViewModelMock()
        : this(string.Empty)
    {
    }

    /// <summary>Gets the ID of the page.</summary>
    public string Id { get; } = id ?? string.Empty;

    /// <inheritdoc/>
    public virtual IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) =>
        Observable.Return(Unit.Default).Do(_ => _navigatedTo.OnNext(Unit.Default));

    /// <inheritdoc/>
    public virtual IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) =>
        Observable.Return(Unit.Default).Do(_ => _navigatedFrom.OnNext(Unit.Default));

    /// <inheritdoc/>
    public virtual IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) =>
        Observable.Return(Unit.Default).Do(_ => _navigatingTo.OnNext(Unit.Default));

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Releases the lifecycle notification subjects.</summary>
    /// <param name="disposing">True to release managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        _navigatedTo.Dispose();
        _navigatingTo.Dispose();
        _navigatedFrom.Dispose();
    }
}
