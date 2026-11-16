// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Sextant.Maui;

/// <summary>Adapts platform navigation events to lifecycle notifications.</summary>
internal static class NavigationEvents
{
    /// <summary>Observes popped pages and removes the event handler when disposed.</summary>
    /// <param name="subscribe">Attaches the handler.</param>
    /// <param name="unsubscribe">Removes the handler.</param>
    /// <returns>The popped pages.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static IObservable<NavigationEventArgs> ObservePops(Action<EventHandler<NavigationEventArgs>> subscribe, Action<EventHandler<NavigationEventArgs>> unsubscribe) =>
        Observable.FromEventPattern<EventHandler<NavigationEventArgs>, NavigationEventArgs>(subscribe, unsubscribe).Select(static value => value.EventArgs);

    /// <summary>Observes view models associated with popped pages.</summary>
    /// <param name="subscribe">Attaches the handler.</param>
    /// <param name="unsubscribe">Removes the handler.</param>
    /// <returns>The popped view models.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static IObservable<IViewModel> ObserveViewModelPops(Action<EventHandler<NavigationEventArgs>> subscribe, Action<EventHandler<NavigationEventArgs>> unsubscribe) =>
        ObservePops(subscribe, unsubscribe).Select(static value => value.Page.BindingContext).OfType<IViewModel>();

    /// <summary>Sends lifecycle callbacks for device pops while ignoring service pops.</summary>
    /// <param name="navigation">The navigation view.</param>
    /// <param name="popped">The platform pop notifications.</param>
    /// <param name="source">The current navigation source.</param>
    /// <param name="subscriptions">Owns event and lifecycle subscriptions.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void SubscribeDevicePops(NavigationPage navigation, IObservable<NavigationEventArgs> popped, IObservable<NavigationSource> source, CompositeDisposable subscriptions) =>
        popped.WithLatestFrom(source, static (page, navigationSource) => (page, navigationSource))
            .Where(static result => result.navigationSource == NavigationSource.Device)
            .Select(static result => result.page)
            .Subscribe(value => NavigationLifecycle.OnPagePopped(value.Page, navigation.CurrentPage, subscriptions))
            .DisposeWith(subscriptions);
}
