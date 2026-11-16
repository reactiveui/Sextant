// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using Microsoft.Maui.Controls;
using Mopups.Pages;

namespace Sextant.Plugins.Popup;

/// <summary>Base Popup page for that implements <see cref="IViewFor"/>.</summary>
[System.Diagnostics.DebuggerDisplay("SextantPopupPage: {BackgroundClick}")]
public class SextantPopupPage : PopupPage, IViewFor
{
    /// <summary>The view model property.</summary>
    public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(
        nameof(ViewModel),
        typeof(object),
        typeof(IViewFor<object>),
        null,
        BindingMode.OneWay,
        null,
        static (bindable, _, value) => bindable.BindingContext = value);

    /// <summary>Initializes a new instance of the <see cref="SextantPopupPage"/> class.</summary>
    protected SextantPopupPage() =>
        BackgroundClick =
            Observable.FromEventPattern(
                x => BackgroundClicked += x,
                x => BackgroundClicked -= x).Select(static _ => Unit.Default);

    /// <summary>Gets the background click observable signal.</summary>
    /// <value>The background click.</value>
    public IObservable<Unit> BackgroundClick { get; }

    /// <summary>Gets or sets the ViewModel to display.</summary>
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
}
