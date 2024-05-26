// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Maui.Controls;
using ReactiveUI;

namespace Sextant.Plugins.Popup;

/// <summary>
/// Base Popup page for that implements <see cref="IViewFor"/>.
/// </summary>
/// <typeparam name="TViewModel">The view model type.</typeparam>
public abstract class SextantPopupPage<TViewModel> : SextantPopupPage, IViewFor<TViewModel>
    where TViewModel : class, IViewModel
{
    /// <summary>
    /// The view model property.
    /// </summary>
    public static new readonly BindableProperty ViewModelProperty = BindableProperty.Create(
        nameof(ViewModel),
        typeof(TViewModel),
        typeof(IViewFor<TViewModel>),
        null,
        BindingMode.OneWay,
        null,
        OnViewModelChanged);

    /// <summary>
    /// Gets or sets the ViewModel to display.
    /// </summary>
    public new TViewModel? ViewModel
    {
        get => (TViewModel?)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    /// <summary>
    /// Gets or sets the ViewModel corresponding to this specific View.
    /// This should be a BindableProperty if you're using XAML.
    /// </summary>
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TViewModel?)value;
    }

    /// <inheritdoc/>
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        ViewModel = (BindingContext as TViewModel)!;
    }

    private static void OnViewModelChanged(BindableObject bindableObject, object oldValue, object newValue) => bindableObject.BindingContext = newValue;
}
