// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using Avalonia;
using ReactiveUI.Avalonia.Reactive;

namespace SextantSample.Avalonia;

/// <summary>Starts the Avalonia sample application.</summary>
public static class Program
{
    /// <summary>Defines the entry point of the application.</summary>
    /// <param name="args">The arguments.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Main(string[] args) => BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    /// <summary>Builds the avalonia application.</summary>
    /// <returns>An AppBuilder.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UseReactiveUI(static builder => _ = builder.WithExceptionHandler(new SextantSample.ViewModels.SextantDefaultExceptionHandler()))
            .UsePlatformDetect()
            .LogToTrace();
}
