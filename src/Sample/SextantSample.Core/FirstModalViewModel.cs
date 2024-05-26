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
/// FirstModalViewModel.
/// </summary>
/// <seealso cref="ViewModelBase" />
/// <seealso cref="IDestructible" />
public class FirstModalViewModel : ViewModelBase, IDestructible
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FirstModalViewModel"/> class.
    /// </summary>
    /// <param name="viewStackService">The view stack service.</param>
    public FirstModalViewModel(IViewStackService viewStackService)
        : base(viewStackService)
    {
        OpenModal = ReactiveCommand
                    .CreateFromObservable(() => ViewStackService!.PushModal(new SecondModalViewModel(viewStackService)), outputScheduler: RxApp.MainThreadScheduler);

        PopModal = ReactiveCommand
                    .CreateFromObservable(() => ViewStackService!.PopModal(), outputScheduler: RxApp.MainThreadScheduler);

        OpenModal.Subscribe(_ => Debug.WriteLine("PagePushed"));
        PopModal.Subscribe(_ => Debug.WriteLine("PagePopped"));
        PopModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
    }

    /// <summary>
    /// Gets or sets the open modal.
    /// </summary>
    /// <value>
    /// The open modal.
    /// </value>
    public ReactiveCommand<Unit, Unit> OpenModal { get; set; }

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
    public override string Id => nameof(FirstModalViewModel);

    /// <summary>
    /// Destroy the destructible object.
    /// </summary>
    public void Destroy() => Debug.WriteLine($"Destroy: {nameof(FirstModalViewModel)}");
}
