// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Sextant;

namespace NavigationLifecycle;

/// <summary>Receives an order number when opened and the save result when revisited.</summary>
internal sealed class OrderPage : INavigable, IDestructible
{
    /// <summary>The parameter carrying the selected order.</summary>
    internal const string Order = "order";

    /// <summary>The parameter returned by the editor.</summary>
    internal const string Saved = "saved";

    /// <inheritdoc/>
    public string Id => nameof(OrderPage);

    /// <summary>Gets the order selected for editing.</summary>
    internal int Number { get; private set; }

    /// <summary>Gets a value indicating whether the editor saved the order.</summary>
    internal bool WasSaved { get; private set; }

    /// <summary>Gets a value indicating whether the page was removed.</summary>
    internal bool WasDestroyed { get; private set; }

    /// <inheritdoc/>
    public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter)
    {
        Number = parameter.GetValue(Order, Number);
        return Observable.Return(Unit.Default);
    }

    /// <inheritdoc/>
    public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter)
    {
        if (parameter.TryGetValue<bool>(Saved, out var saved))
        {
            WasSaved = saved;
        }

        return Observable.Return(Unit.Default);
    }

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) => Observable.Return(Unit.Default);

    /// <inheritdoc/>
    public void Destroy() => WasDestroyed = true;
}
