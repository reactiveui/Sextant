// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Sextant.Mocks;

/// <summary>Abstract view model.</summary>
[System.Diagnostics.DebuggerDisplay("AbstractViewModel: {Id}")]
public class AbstractViewModel : INavigable
{
    /// <inheritdoc/>
    public string Id { get; } = string.Empty;

    /// <inheritdoc/>
    public virtual IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) =>
        Observable.Return(Unit.Default);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) => WhenNavigatedTo(parameter);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) => WhenNavigatedFrom(parameter);
}
