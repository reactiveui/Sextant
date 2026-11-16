// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Maui.Controls;

namespace Sextant.Plugins.Popup;

/// <summary>Base Popup page for that implements <see cref="IViewFor"/>.</summary>
/// <typeparam name="TViewModel">The view model type.</typeparam>
[System.Diagnostics.DebuggerDisplay("SextantPopupPage: {ViewModel}")]
public class SextantPopupPage<TViewModel> : SextantPopupPage, IViewFor<TViewModel>
    where TViewModel : class, IViewModel
{
    /// <summary>The view model property.</summary>
    public static new readonly BindableProperty ViewModelProperty = BindableProperty.Create(
        nameof(ViewModel),
        typeof(TViewModel),
        typeof(IViewFor<TViewModel>),
        null,
        BindingMode.OneWay,
        null,
        static (bindable, _, value) => bindable.BindingContext = value);

    /// <summary>Gets or sets the ViewModel to display.</summary>
    public new TViewModel? ViewModel
    {
        get => (TViewModel?)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    /// <summary>Gets or sets the ViewModel corresponding to this specific View. This should be a BindableProperty if you're using XAML.</summary>
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => ViewModel = (TViewModel?)value;
    }

    /// <inheritdoc/>
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        ViewModel = BindingContext as TViewModel;
    }
}
