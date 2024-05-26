// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;
using SextantSample.ViewModels;

namespace SextantSample.Views;

/// <summary>
/// SecondModalView.
/// </summary>
public partial class SecondModalView
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SecondModalView"/> class.
    /// </summary>
    public SecondModalView()
    {
        InitializeComponent();
        this.BindCommand(ViewModel, x => x.PushPage, x => x.PushPage);
        this.BindCommand(ViewModel, x => x.PopModal, x => x.PopModal);

        Interactions
            .ErrorMessage
            .RegisterHandler(async x =>
            {
                await DisplayAlert("Error", x.Input.Message, "Done");
                x.SetOutput(true);
            });
    }
}
