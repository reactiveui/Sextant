// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace SextantSample.Maui.Views;

/// <summary>Displays the sample view and its navigation commands.</summary>
[System.Diagnostics.DebuggerDisplay("HomeView: {ToString(),nq}")]
public partial class HomeView : ReactiveContentPage<HomeViewModel>
{
    /// <summary>Initializes a new instance of the <see cref="HomeView"/> class.</summary>
    public HomeView()
    {
        InitializeComponent();

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
}
