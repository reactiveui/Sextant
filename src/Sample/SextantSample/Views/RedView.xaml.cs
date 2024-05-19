// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;
using ReactiveUI.XamForms;
using SextantSample.ViewModels;

namespace SextantSample.Views;

/// <summary>
/// RedView.
/// </summary>
public partial class RedView
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RedView"/> class.
    /// </summary>
    public RedView()
    {
        InitializeComponent();
        this.BindCommand(ViewModel, x => x.PopModal, x => x.PopModal);
        this.BindCommand(ViewModel, x => x.PushPage, x => x.PushPage);
        this.BindCommand(ViewModel, x => x.PopPage, x => x.PopPage);
        this.BindCommand(ViewModel, x => x.PopToRoot, x => x.PopToRoot);

        Interactions
            .ErrorMessage
            .RegisterHandler(async x =>
            {
                await DisplayAlert("Error", x.Input.Message, "Done");
                x.SetOutput(true);
            });
    }
}
