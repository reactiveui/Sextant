// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;
using ReactiveUI.XamForms;

using SextantSample.ViewModels;
using Xamarin.Forms.Xaml;

namespace SextantSample.Views;

/// <summary>
/// GreenView.
/// </summary>
[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class GreenView : ReactiveContentPage<GreenViewModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GreenView"/> class.
    /// </summary>
    public GreenView()
    {
        InitializeComponent();

        this.BindCommand(ViewModel, x => x.OpenModal, x => x.Modal);
    }
}
