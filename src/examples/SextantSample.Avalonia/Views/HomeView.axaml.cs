// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI.Avalonia.Reactive;
using SextantSample.ViewModels;

namespace SextantSample.Avalonia.Views;

/// <summary>Displays the home navigation commands.</summary>
public partial class HomeView : ReactiveUserControl<HomeViewModel>
{
    /// <summary>Initializes a new instance of the <see cref="HomeView"/> class.</summary>
    public HomeView() => InitializeComponent();
}
