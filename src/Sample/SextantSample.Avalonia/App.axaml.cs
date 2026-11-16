// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
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

/// <summary>Hosts the Avalonia navigation sample.</summary>
public class App : Application
{
    /// <summary>Initializes the application by loading XAML etc.</summary>
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    /// <summary>Called when [framework initialization completed].</summary>
    public override void OnFrameworkInitializationCompleted()
    {
        var navigationView = new NavigationView();
        var viewStackService = new ViewStackService(navigationView, new DefaultViewModelFactory());
        _ = AppLocator.CurrentMutable
            .RegisterViewForNavigation(static () => new HomeView(), static () => new HomeViewModel())
            .RegisterViewForNavigation(static () => new FirstModalView(), () => new FirstModalViewModel(viewStackService))
            .RegisterViewForNavigation(static () => new SecondModalView(), () => new SecondModalViewModel(viewStackService))
            .RegisterViewForNavigation(static () => new RedView(), () => new RedViewModel(viewStackService))
            .RegisterViewForNavigation(static () => new GreenView(), () => new GreenViewModel(viewStackService));
        AppLocator.CurrentMutable.RegisterConstant<IViewStackService>(viewStackService);
        _ = viewStackService.PushPage(new HomeViewModel()).Subscribe();
        _ = Interactions.ErrorMessage.RegisterHandler(static async context =>
        {
            _ = await MessageBoxManager.GetMessageBoxStandard("Notification", context.Input.ToString()).ShowAsync();
            context.SetOutput(true);
        });
        new Window { Content = navigationView }.Show();
        base.OnFrameworkInitializationCompleted();
    }
}
