// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Disposables.Fluent;
using System.Reactive.Linq;

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
        _ = Observable
            .FromEvent<EventHandler<NavigationEventArgs>, NavigationEventArgs>(
                handler =>
                {
                    void Handler(object sender, NavigationEventArgs args)
                    {
                        _ = sender;
                        handler(args);
                    }
                    return Handler!;
                },
                x => bindable.Popped += x,
                x => bindable.Popped -= x)
            .WithLatestFrom(navigationSource, static (navigated, navigationSource) => (navigated, navigationSource))
            .Where(static result => result.navigationSource == NavigationSource.Device)
            .Select(static x => x.navigated)
            .Subscribe(navigated =>
            {
                INavigationParameter navigationParameter = new NavigationParameter();

                if (navigated.Page.BindingContext is INavigated previous)
                {
                    _ = previous.WhenNavigatedFrom(navigationParameter).Subscribe().DisposeWith(BehaviorDisposable);
                }

                if (bindable.CurrentPage?.BindingContext is INavigated current)
                {
                    _ = current.WhenNavigatedTo(navigationParameter).Subscribe().DisposeWith(BehaviorDisposable);
                }

                if (navigated.Page.BindingContext is IDestructible destructible)
                {
                    destructible.Destroy();
                }
            })
            .DisposeWith(BehaviorDisposable);

        base.OnAttachedTo(bindable);
    }
}
