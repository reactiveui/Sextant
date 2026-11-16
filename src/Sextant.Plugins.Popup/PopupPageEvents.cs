// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Sextant.Plugins.Popup;

/// <summary>Adapts popup background clicks to navigation notifications.</summary>
internal static class PopupPageEvents
{
    /// <summary>Observes background clicks and removes the handler when disposed.</summary>
    /// <param name="subscribe">Attaches the event handler.</param>
    /// <param name="unsubscribe">Removes the event handler.</param>
    /// <returns>The click notifications.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static IObservable<Unit> ObserveBackgroundClicks(Action<EventHandler> subscribe, Action<EventHandler> unsubscribe) =>
        Observable.FromEventPattern(subscribe, unsubscribe).Select(static _ => Unit.Default);
}
