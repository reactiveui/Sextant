// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
namespace SextantSample.Maui.Views;

/// <summary>Displays the sample navigation stack with a blue toolbar.</summary>
[System.Diagnostics.DebuggerDisplay("BlueNavigationView: {NavigationStack.Count}")]
public class BlueNavigationView : NavigationView
{
    /// <summary>Initializes a new instance of the <see cref="BlueNavigationView"/> class.</summary>
    public BlueNavigationView()
            : base(RxSchedulers.MainThreadScheduler, ReactiveUI.Binding.Reactive.ViewLocator.GetCurrent())
    {
        BarBackgroundColor = Colors.Blue;
        BarTextColor = Colors.White;
    }
}
