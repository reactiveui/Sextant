// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Sextant.Mocks;

/// <summary>A view model whose navigation callbacks complete immediately.</summary>
internal class NavigatedMock : INavigated
{
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) => Observable.Return(Unit.Default);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) => WhenNavigatedTo(parameter);
}
