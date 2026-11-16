// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace SextantSample.Maui.Views;

/// <summary>Displays the sample view and its navigation commands.</summary>
[System.Diagnostics.DebuggerDisplay("FirstModalView: {ToString(),nq}")]
public partial class FirstModalView : ReactiveContentPage<FirstModalViewModel>
{
    /// <summary>Initializes a new instance of the <see cref="FirstModalView"/> class.</summary>
    public FirstModalView()
    {
        InitializeComponent();

        _ = this.WhenActivated(disposables =>
        {
            _ = this.BindCommand(ViewModel, static vm => vm.OpenModal, static v => v.OpenSecondModal)
                .DisposeWith(disposables);
            _ = this.BindCommand(ViewModel, static vm => vm.PopModal, static v => v.PopModal)
                .DisposeWith(disposables);
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
