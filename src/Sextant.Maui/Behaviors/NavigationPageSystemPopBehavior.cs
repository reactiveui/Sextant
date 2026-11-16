// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant.Maui;

/// <summary>Represents a <see cref="BehaviorBase{T}"/> that intercepts the backwards navigation.</summary>
/// <param name="navigationSource">A value indicating whether the back button was pressed.</param>
/// <remarks>
/// Initializes a new instance of the <see cref="NavigationPageSystemPopBehavior"/> class.
/// </remarks>
public sealed class NavigationPageSystemPopBehavior(IObservable<NavigationSource> navigationSource) : BehaviorBase<NavigationPage>
{
    /// <inheritdoc/>
    protected override void OnAttachedTo(NavigationPage bindable)
    {
        NavigationEvents.SubscribeDevicePops(
            bindable,
            NavigationEvents.ObservePops(handler => bindable.Popped += handler, handler => bindable.Popped -= handler),
            navigationSource,
            BehaviorDisposable);

        base.OnAttachedTo(bindable);
    }
}
