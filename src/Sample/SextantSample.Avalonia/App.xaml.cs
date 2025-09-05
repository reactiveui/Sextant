// Copyright (c) 2025 .NET Foundation and Contributors. All rights reserved.
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
        IViewStackService viewStackService = default!;

        AppLocator
                .CurrentMutable
                .RegisterViewForNavigation<HomeView, HomeViewModel>()
                .RegisterViewForNavigation<FirstModalView, FirstModalViewModel>(() => new(), () => new(viewStackService))
                .RegisterViewForNavigation<SecondModalView, SecondModalViewModel>(() => new(), () => new(viewStackService))
                .RegisterViewForNavigation<RedView, RedViewModel>(() => new(), () => new(viewStackService))
                .RegisterViewForNavigation<GreenView, GreenViewModel>(() => new(), () => new(viewStackService))
                .RegisterViewModelFactory(() => new DefaultViewModelFactory())
                .RegisterNavigationView(() => new NavigationView());
        viewStackService = AppLocator.Current.GetService<IViewStackService>()!;
        viewStackService
            .PushPage(new HomeViewModel());

        Interactions.ErrorMessage.RegisterHandler(async context =>
           await MessageBoxManager.GetMessageBoxStandard("Notification", context.Input.ToString())
                .ShowAsync());

        new Window { Content = AppLocator.Current.GetNavigationView() }.Show();
        base.OnFrameworkInitializationCompleted();
    }
}
