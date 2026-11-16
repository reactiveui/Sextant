// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
namespace SextantSample.Maui;

/// <summary>Hosts the sample navigation window.</summary>
public partial class App : Application
{
    /// <summary>Initializes a new instance of the <see cref="App"/> class.</summary>
    /// <exception cref="InvalidOperationException">Thrown when <c>AppLocator.Current.GetService&lt;IViewStackService&gt;()</c> is <see langword="null"/>.</exception>
    public App()
    {
        InitializeComponent();
        var navigation = AppLocator.Current.GetService<IViewStackService>()
            ?? throw new InvalidOperationException("The navigation service was not registered.");
        _ = navigation.PushPage(new HomeViewModel(), null, true, false).Subscribe();
    }

    /// <inheritdoc />
    protected override Window CreateWindow(IActivationState? activationState) =>
        new(AppLocator.Current.GetNavigationView()
            ?? throw new InvalidOperationException("The navigation view was not registered."));
}
