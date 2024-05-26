// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SextantSample.ViewModels;

namespace SextantSample.Avalonia.Views;

/// <summary>
/// FirstModalView.
/// </summary>
public class FirstModalView : ReactiveUserControl<FirstModalViewModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FirstModalView"/> class.
    /// </summary>
    public FirstModalView() => AvaloniaXamlLoader.Load(this);
}
