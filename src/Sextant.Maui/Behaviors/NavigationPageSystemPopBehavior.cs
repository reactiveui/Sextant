// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Sextant.Maui;

/// <summary>
/// Represents a <see cref="BehaviorBase{T}"/> that intercepts the backwards navigation.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="NavigationPageSystemPopBehavior"/> class.
/// </remarks>
/// <param name="navigationSource">A value indicating whether the back button was pressed.</param>
public sealed class NavigationPageSystemPopBehavior(IObservable<NavigationSource> navigationSource) : BehaviorBase<NavigationPage>
{
    /// <inheritdoc/>
    protected override void OnAttachedTo(NavigationPage bindable)
    {
        Observable
            .FromEvent<EventHandler<NavigationEventArgs>, NavigationEventArgs>(
                handler =>
                {
                    void Handler(object sender, NavigationEventArgs args) => handler(args);
                    return Handler!;
                },
                x => bindable.Popped += x,
                x => bindable.Popped -= x)
            .WithLatestFrom(navigationSource, (navigated, navigationSource) => (navigated, navigationSource))
            .Where(result => result.navigationSource == NavigationSource.Device)
            .Select(x => x.navigated)
            .Subscribe(navigated =>
            {
                INavigationParameter navigationParameter = new NavigationParameter();

                navigated
                    .Page
                    .BindingContext
                    .InvokeViewModelAction<INavigated>(x =>
                        x.WhenNavigatedFrom(navigationParameter)
                            .Subscribe()
                            .DisposeWith(BehaviorDisposable));

                bindable
                    .CurrentPage
                    .BindingContext
                    .InvokeViewModelAction<INavigated>(x =>
                        x.WhenNavigatedTo(navigationParameter)
                            .Subscribe()
                            .DisposeWith(BehaviorDisposable));

                navigated
                    .Page
                    .BindingContext
                    .InvokeViewModelAction<IDestructible>(x => x.Destroy());
            })
            .DisposeWith(BehaviorDisposable);

        base.OnAttachedTo(bindable);
    }
}
