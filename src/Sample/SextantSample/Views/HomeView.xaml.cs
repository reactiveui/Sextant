// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Disposables;

using ReactiveUI;

using SextantSample.ViewModels;

namespace SextantSample.Views;

/// <summary>
/// HomeView.
/// </summary>
public partial class HomeView
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HomeView"/> class.
    /// </summary>
    public HomeView()
    {
        InitializeComponent();

        this.WhenActivated(disposables =>
    {
        this.BindCommand(ViewModel, x => x.OpenModal, x => x.FirstModalButton).DisposeWith(disposables);
        this.BindCommand(ViewModel, x => x.PushPage, x => x.PushPage).DisposeWith(disposables);
        this.BindCommand(ViewModel, x => x.PushGenericPage, x => x.PushGenericPage).DisposeWith(disposables);
    });

        Interactions
            .ErrorMessage
            .RegisterHandler(async x =>
            {
                await DisplayAlert("Error", x.Input.Message, "Done");
                x.SetOutput(true);
            });
    }
}
