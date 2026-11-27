// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
namespace SextantSample.Maui;

/// <summary>Starts the native application host on Tizen.</summary>
[System.Diagnostics.DebuggerDisplay("Program")]
internal sealed class Program : MauiApplication
{
    /// <summary>Runs the Tizen application.</summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args) => new Program().Run(args);

    /// <inheritdoc />
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
