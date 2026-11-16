// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI.Avalonia.Reactive;
using SextantSample.ViewModels;

namespace SextantSample.Avalonia.Views;

/// <summary>Displays the red navigation page.</summary>
public partial class RedView : ReactiveUserControl<RedViewModel>
{
    /// <summary>Initializes a new instance of the <see cref="RedView"/> class.</summary>
    public RedView() => InitializeComponent();
}
