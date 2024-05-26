// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

using ReactiveUI;

using Sextant.XamForms;
using Xamarin.Forms;

namespace SextantSample.Views;

/// <summary>
/// BlueNavigationView.
/// </summary>
/// <seealso cref="Sextant.XamForms.NavigationView" />
/// <seealso cref="ReactiveUI.IViewFor" />
public class BlueNavigationView : NavigationView, IViewFor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BlueNavigationView"/> class.
    /// </summary>
    public BlueNavigationView()
        : base(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, ViewLocator.Current)
    {
        BarBackgroundColor = Color.Blue;
        BarTextColor = Color.White;
    }

    /// <summary>
    /// Gets or sets the View Model associated with the View.
    /// </summary>
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
    public object ViewModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations
}
