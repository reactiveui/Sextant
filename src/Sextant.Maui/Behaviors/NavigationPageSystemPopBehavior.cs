// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Microsoft.Maui.Controls;

namespace Sextant.Maui
{
    /// <summary>
    /// Represents a <see cref="BehaviorBase{T}"/> that intercepts the backwards navigation.
    /// </summary>
    public sealed class NavigationPageSystemPopBehavior : BehaviorBase<NavigationPage>
    {
        private readonly IObservable<NavigationSource> _navigationSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationPageSystemPopBehavior"/> class.
        /// </summary>
        /// <param name="navigationSource">A value indicating whether the back button was pressed.</param>
        public NavigationPageSystemPopBehavior(IObservable<NavigationSource> navigationSource) =>
            _navigationSource = navigationSource;

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
                .WithLatestFrom(_navigationSource, (navigated, navigationSource) => (navigated, navigationSource))
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
}
