// Copyright (c) 2025 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Maui.Controls;
using Mopups.Pages;
using ReactiveUI;

namespace Sextant.Plugins.Popup;

/// <summary>
/// Base Popup page for that implements <see cref="IViewFor"/>.
/// </summary>
public abstract class SextantPopupPage : PopupPage, IViewFor
{
    /// <summary>
    /// The view model property.
    /// </summary>
    public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(
        nameof(ViewModel),
        typeof(object),
        typeof(IViewFor<object>),
        null,
        BindingMode.OneWay,
        null,
        OnViewModelChanged);

    /// <summary>
    /// Initializes a new instance of the <see cref="SextantPopupPage"/> class.
    /// </summary>
    protected SextantPopupPage() =>
        BackgroundClick =
            Observable.FromEvent<EventHandler, Unit>(
                handler =>
                {
                    void EventHandler(object? sender, EventArgs args) => handler(Unit.Default);
                    return EventHandler;
                },
                x => BackgroundClicked += x,
                x => BackgroundClicked -= x);

    /// <summary>
    /// Gets the background click observable signal.
    /// </summary>
    /// <value>The background click.</value>
    public IObservable<Unit> BackgroundClick { get; }

    /// <summary>
    /// Gets or sets the ViewModel to display.
    /// </summary>
    public object? ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    /// <inheritdoc/>
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        ViewModel = BindingContext;
    }

    private static void OnViewModelChanged(BindableObject bindableObject, object oldValue, object newValue) => bindableObject.BindingContext = newValue;
}
