// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using ReactiveUI;
using Sextant;
using Splat;

namespace SextantSample.ViewModels;

/// <summary>
/// HomeViewModel.
/// </summary>
/// <seealso cref="ViewModelBase" />
public class HomeViewModel : ViewModelBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HomeViewModel"/> class.
    /// </summary>
    public HomeViewModel()
        : base(Locator.Current.GetService<IViewStackService>())
    {
        OpenModal = ReactiveCommand
            .CreateFromObservable(() => ViewStackService!.PushModal(new FirstModalViewModel(ViewStackService)), outputScheduler: RxApp.MainThreadScheduler);

        PushPage = ReactiveCommand
            .CreateFromObservable(() => ViewStackService!.PushPage(new RedViewModel(ViewStackService)), outputScheduler: RxApp.MainThreadScheduler);

        PushGenericPage = ReactiveCommand
            .CreateFromObservable(() => ViewStackService!.PushPage<GreenViewModel>(), outputScheduler: RxApp.MainThreadScheduler);

        PushPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        PushGenericPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        OpenModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
    }

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public override string Id => nameof(HomeViewModel);

    /// <summary>
    /// Gets or sets the open modal.
    /// </summary>
    /// <value>
    /// The open modal.
    /// </value>
    public ReactiveCommand<Unit, Unit> OpenModal { get; set; }

    /// <summary>
    /// Gets or sets the push page.
    /// </summary>
    /// <value>
    /// The push page.
    /// </value>
    public ReactiveCommand<Unit, Unit> PushPage { get; set; }

    /// <summary>
    /// Gets or sets the push generic page.
    /// </summary>
    /// <value>
    /// The push generic page.
    /// </value>
    public ReactiveCommand<Unit, Unit> PushGenericPage { get; set; }
}
