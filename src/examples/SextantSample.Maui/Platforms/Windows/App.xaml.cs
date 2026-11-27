// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
namespace SextantSample.Maui.WinUI;

/// <summary>Creates the MAUI application on Windows.</summary>
[System.Diagnostics.DebuggerDisplay("App")]
public partial class App : MauiWinUIApplication
{
    /// <summary>Initializes a new instance of the <see cref="App"/> class.</summary>
    public App() => InitializeComponent();

    /// <inheritdoc />
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
