// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Xamarin.Forms;

namespace SextantSample.WPF;

/// <summary>
/// MainWindow.
/// </summary>
/// <seealso cref="Xamarin.Forms.Platform.WPF.FormsApplicationPage" />
/// <seealso cref="System.Windows.Markup.IComponentConnector" />
public partial class MainWindow
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        Forms.Init();
        LoadApplication(new SextantSample.App());
    }
}
