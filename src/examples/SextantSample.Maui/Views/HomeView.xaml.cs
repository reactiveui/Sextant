// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Sextant.Plugins.Popup;

namespace SextantSample.Maui.Views;

/// <summary>Displays the sample view and its navigation commands.</summary>
[System.Diagnostics.DebuggerDisplay("HomeView: {ToString(),nq}")]
public partial class HomeView : ReactiveContentPage<HomeViewModel>
{
    /// <summary>Initializes a new instance of the <see cref="HomeView"/> class.</summary>
    public HomeView()
    {
        InitializeComponent();
        DeliveryNoteButton.Clicked += ShowDeliveryNote;

        _ = this.WhenActivated(disposables =>
        {
            _ = this.BindCommand(ViewModel, static x => x.OpenModal, static x => x.FirstModalButton).DisposeWith(disposables);
            _ = this.BindCommand(ViewModel, static x => x.PushPage, static x => x.PushPage).DisposeWith(disposables);
            _ = this.BindCommand(ViewModel, static x => x.PushGenericPage, static x => x.PushGenericPage).DisposeWith(disposables);
        });

        _ = Interactions
            .ErrorMessage
            .RegisterHandler(async x =>
            {
                await DisplayAlertAsync("Error", x.Input.Message, "Done");
                x.SetOutput(true);
            });
    }

    /// <summary>Shows the registered delivery note popup when the driver requests it.</summary>
    /// <param name="sender">The delivery note button.</param>
    /// <param name="e">The click event.</param>
    private async void ShowDeliveryNote(object? sender, EventArgs e)
    {
        var popups = AppLocator.Current.GetService<IPopupViewStackService>();
        if (popups is null)
        {
            await DisplayAlertAsync("Delivery note", "The popup service is unavailable.", "Close");
            return;
        }

        try
        {
            _ = await popups.PushPopup<DeliveryNote>().ToTask(CancellationToken.None);
        }
        catch (InvalidOperationException error)
        {
            await DisplayAlertAsync("Delivery note", error.Message, "Close");
        }
    }
}
