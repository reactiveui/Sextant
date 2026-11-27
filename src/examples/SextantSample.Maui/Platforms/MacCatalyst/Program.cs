// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using UIKit;

namespace SextantSample.Maui;

/// <summary>Starts the native application host on Mac Catalyst.</summary>
public static class Program
{
    /// <summary>Runs the Mac Catalyst application.</summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args) => UIApplication.Main(args, null, typeof(AppDelegate));
}
