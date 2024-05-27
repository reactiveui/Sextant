// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Reactive;
using ReactiveUI;
using Sextant;

namespace SextantSample.ViewModels;

/// <summary>
/// SecondModalViewModel.
/// </summary>
/// <seealso cref="ViewModelBase" />
public class SecondModalViewModel : ViewModelBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SecondModalViewModel"/> class.
    /// </summary>
    /// <param name="viewStackService">The view stack service.</param>
    public SecondModalViewModel(IViewStackService viewStackService)
        : base(viewStackService)
    {
        PushPage = ReactiveCommand
            .CreateFromObservable(() => ViewStackService!.PushPage(new RedViewModel(ViewStackService)), outputScheduler: RxApp.MainThreadScheduler);

        PopModal = ReactiveCommand
            .CreateFromObservable(() => ViewStackService!.PopModal(), outputScheduler: RxApp.MainThreadScheduler);

        PushPage.Subscribe(_ => Debug.WriteLine("PagePushed"));
        PopModal.Subscribe(_ => Debug.WriteLine("PagePopped"));

        PushPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        PopModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
    }

    /// <summary>
    /// Gets or sets the push page.
    /// </summary>
    /// <value>
    /// The push page.
    /// </value>
    public ReactiveCommand<Unit, Unit> PushPage { get; set; }

    /// <summary>
    /// Gets or sets the pop modal.
    /// </summary>
    /// <value>
    /// The pop modal.
    /// </value>
    public ReactiveCommand<Unit, Unit> PopModal { get; set; }

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public override string Id => nameof(SecondModalViewModel);
}
