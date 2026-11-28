// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Sextant.Plugins.Popup;

namespace SextantSample.Maui.Views;

/// <summary>Shows a delivery note in a popup that closes when the driver taps the background.</summary>
internal sealed class DeliveryNotePopup : SextantPopupPage<DeliveryNote>
{
    /// <summary>Initializes a new instance of the <see cref="DeliveryNotePopup"/> class.</summary>
    public DeliveryNotePopup()
    {
        CloseWhenBackgroundIsClicked = true;
        Label label = new() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, BackgroundColor = Colors.White, TextColor = Colors.Black };
        label.SetBinding(Label.TextProperty, static (DeliveryNote note) => note.Instructions);
        Content = label;
    }
}
