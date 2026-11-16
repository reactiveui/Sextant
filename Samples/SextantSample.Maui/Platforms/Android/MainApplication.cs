// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using Android.App;
using Android.Runtime;

namespace SextantSample.Maui;

/// <summary>Creates the MAUI application on Android.</summary>
[Application]
public class MainApplication : MauiApplication
{
    /// <summary>Initializes a new instance of the <see cref="MainApplication"/> class.</summary>
    /// <param name="handle">The native application handle.</param>
    /// <param name="ownership">The ownership of the native handle.</param>
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
                : base(handle, ownership)
    {
    }

    /// <inheritdoc />
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
