// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using ReactiveUI;
using Sextant;
using Sextant.XamForms;
using SextantSample.ViewModels;
using SextantSample.Views;
using Splat;
using Xamarin.Forms.Xaml;
using static Sextant.Sextant;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace SextantSample;

/// <summary>
/// App.
/// </summary>
/// <seealso cref="Xamarin.Forms.Application" />
public partial class App
{
    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    public App()
    {
        InitializeComponent();

        RxApp.DefaultExceptionHandler = new SextantDefaultExceptionHandler();
        Instance.InitializeForms();
        Locator
            .CurrentMutable
            .RegisterView<HomeView, HomeViewModel>()
            .RegisterView<FirstModalView, FirstModalViewModel>()
            .RegisterView<SecondModalView, SecondModalViewModel>()
            .RegisterView<RedView, RedViewModel>()
            .RegisterView<GreenView, GreenViewModel>()
            .RegisterNavigationView(() => new BlueNavigationView())
            .RegisterViewModel(() => new GreenViewModel(Locator.Current.GetService<IViewStackService>()));

        Locator
            .Current
            .GetService<IViewStackService>()
            .PushPage(new HomeViewModel(), null, true, false)
            .Subscribe();

        MainPage = Locator.Current.GetNavigationView();
    }

    /// <summary>
    /// Application developers override this method to perform actions when the application starts.
    /// </summary>
    /// <remarks>
    /// To be added.
    /// </remarks>
    protected override void OnStart()
    {
        // Handle when your app starts
    }

    /// <summary>
    /// Application developers override this method to perform actions when the application enters the sleeping state.
    /// </summary>
    /// <remarks>
    /// To be added.
    /// </remarks>
    protected override void OnSleep()
    {
        // Handle when your app sleeps
    }

    /// <summary>
    /// Application developers override this method to perform actions when the application resumes from a sleeping state.
    /// </summary>
    /// <remarks>
    /// To be added.
    /// </remarks>
    protected override void OnResume()
    {
        // Handle when your app resumes
    }
}
