// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using Sextant;
using Sextant.Avalonia;
using SextantSample.Avalonia.Views;
using SextantSample.ViewModels;
using Splat;

namespace SextantSample.Avalonia;

/// <summary>
/// App.
/// </summary>
public class App : Application
{
    /// <summary>
    /// Initializes the application by loading XAML etc.
    /// </summary>
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    /// <summary>
    /// Called when [framework initialization completed].
    /// </summary>
    public override void OnFrameworkInitializationCompleted()
    {
        Locator
            .CurrentMutable
            .RegisterView<HomeView, HomeViewModel>()
            .RegisterView<FirstModalView, FirstModalViewModel>()
            .RegisterView<SecondModalView, SecondModalViewModel>()
            .RegisterView<RedView, RedViewModel>()
            .RegisterView<GreenView, GreenViewModel>()
            .RegisterViewModelFactory(() => new DefaultViewModelFactory())
            .RegisterNavigationView(() => new NavigationView())
            .RegisterViewModel(() => new GreenViewModel(Locator.Current.GetService<IViewStackService>()));

        Locator
            .Current
            .GetService<IViewStackService>()
            .PushPage(new HomeViewModel());

        Interactions.ErrorMessage.RegisterHandler(async context =>
           await MessageBoxManager.GetMessageBoxStandard("Notification", context.Input.ToString())
                .ShowAsync());

        new Window { Content = Locator.Current.GetNavigationView() }.Show();
        base.OnFrameworkInitializationCompleted();
    }
}
