// Copyright (c) 2025 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Sextant.Mocks;

/// <summary>
/// Null view model.
/// </summary>
public class NullViewModelMock : INavigable, IDisposable
{
    private readonly Subject<Unit> _navigatedTo;
    private readonly Subject<Unit> _navigatingTo;
    private readonly Subject<Unit> _navigatedFrom;
    private bool _disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="NullViewModelMock"/> class.
    /// </summary>
    public NullViewModelMock()
    {
        _navigatedTo = new Subject<Unit>();
        _navigatedFrom = new Subject<Unit>();
        _navigatingTo = new Subject<Unit>();
    }

    /// <summary>
    /// Gets the ID of the page.
    /// </summary>
    public string Id => nameof(NullViewModelMock);

    /// <inheritdoc/>
    public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) =>
        Observable.Return(Unit.Default).Do(_ => _navigatedTo.OnNext(Unit.Default));

    /// <inheritdoc/>
    public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) =>
        Observable.Return(Unit.Default).Do(_ => _navigatedFrom.OnNext(Unit.Default));

    /// <inheritdoc/>
    public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) =>
        Observable.Return(Unit.Default).Do(_ => _navigatingTo.OnNext(Unit.Default));

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _navigatedFrom?.Dispose();
                _navigatedTo?.Dispose();
                _navigatingTo?.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _disposedValue = true;
        }
    }
}
