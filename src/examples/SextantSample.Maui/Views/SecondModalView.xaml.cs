// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
namespace SextantSample.Maui.Views;

/// <summary>Displays the sample view and its navigation commands.</summary>
[System.Diagnostics.DebuggerDisplay("SecondModalView: {ToString(),nq}")]
public partial class SecondModalView : ReactiveContentPage<SecondModalViewModel>
{
    /// <summary>Initializes a new instance of the <see cref="SecondModalView"/> class.</summary>
    public SecondModalView()
    {
        InitializeComponent();
        _ = this.BindCommand(ViewModel, static x => x.PushPage, static x => x.PushPage);
        _ = this.BindCommand(ViewModel, static x => x.PopModal, static x => x.PopModal);

        _ = Interactions
            .ErrorMessage
            .RegisterHandler(async x =>
            {
                await DisplayAlertAsync("Error", x.Input.Message, "Done");
                x.SetOutput(true);
            });
    }
}
