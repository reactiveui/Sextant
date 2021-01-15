// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace Sextant.XamForms
{
    /// <summary>
    /// Represents a <see cref="BehaviorBase{T}"/> that intercepts the backwards navigation.
    /// </summary>
    public class NavigationPageSystemPopBehavior : BehaviorBase<NavigationPage>
    {
        /// <inheritdoc/>
        protected override void OnAttachedTo(NavigationPage bindable)
        {
            Observable
                .FromEvent<EventHandler<NavigationEventArgs>, NavigationEventArgs>(
                    handler =>
                    {
                        void Handler(object sender, NavigationEventArgs args) => handler(args);
                        return Handler;
                    },
                    x => bindable.Popped += x,
                    x => bindable.Popped -= x)
                .Where(x => x.Page.BindingContext is INavigated)
                .Where(_ => true) // TODO: [rlittlesii: January 10, 2021] Verify this was done by the system and not the consumer of Sextant
                .Select(x => x.Page.BindingContext)
                .Cast<INavigated>()
                .Subscribe(navigated =>
                {
                    INavigationParameter navigationParameter = new NavigationParameter();
                    navigated
                        .WhenNavigatedFrom(navigationParameter)
                        .Subscribe()
                        .DisposeWith(BehaviorDisposable);

                    bindable
                        .CurrentPage
                        .BindingContext
                        .InvokeViewModelAction<INavigated>(x =>
                            x.WhenNavigatedTo(navigationParameter)
                                .Subscribe()
                                .DisposeWith(BehaviorDisposable));

                    navigated
                        .InvokeViewModelAction<IDestructible>(x => x.Destroy());
                })
                .DisposeWith(BehaviorDisposable);

            base.OnAttachedTo(bindable);
        }
    }
}
