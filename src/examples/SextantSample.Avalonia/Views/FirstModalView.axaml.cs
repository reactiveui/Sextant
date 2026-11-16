// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI.Avalonia.Reactive;
using SextantSample.ViewModels;

namespace SextantSample.Avalonia.Views;

/// <summary>Displays the first modal sample page.</summary>
public partial class FirstModalView : ReactiveUserControl<FirstModalViewModel>
{
    /// <summary>Initializes a new instance of the <see cref="FirstModalView"/> class.</summary>
    public FirstModalView() => InitializeComponent();
}
