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
/// A mock of a page view model.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="NavigableViewModelMock"/> class.
/// </remarks>
/// <param name="id">The id of the page.</param>
public class NavigableViewModelMock(string? id = null) : INavigable
{
    private readonly ISubject<Unit> _navigatedTo = new Subject<Unit>();
    private readonly ISubject<Unit> _navigatingTo = new Subject<Unit>();
    private readonly ISubject<Unit> _navigatedFrom = new Subject<Unit>();

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigableViewModelMock"/> class.
    /// </summary>
    public NavigableViewModelMock()
        : this(string.Empty)
    {
    }

    /// <summary>
    /// Gets the ID of the page.
    /// </summary>
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
}
