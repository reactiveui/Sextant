// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Sextant;

namespace SextantSample.ViewModels;

/// <summary>
/// GreenViewModel.
/// </summary>
/// <seealso cref="SextantSample.ViewModels.ViewModelBase" />
/// <seealso cref="Sextant.INavigable" />
/// <seealso cref="Sextant.IDestructible" />
public class GreenViewModel : ViewModelBase, INavigable, IDestructible
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GreenViewModel"/> class.
    /// </summary>
    /// <param name="viewStackService">The view stack service.</param>
    public GreenViewModel(IViewStackService viewStackService)
        : base(viewStackService) =>
        OpenModal = ReactiveCommand
            .CreateFromObservable(() => ViewStackService.PushModal(new FirstModalViewModel(viewStackService), string.Empty, false), outputScheduler: RxApp.MainThreadScheduler);

    /// <summary>
    /// Gets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public override string Id { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the open modal.
    /// </summary>
    /// <value>
    /// The open modal.
    /// </value>
    public ReactiveCommand<Unit, Unit> OpenModal { get; set; }

    /// <summary>
    /// Whens the navigated to.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    /// <returns>A Unit.</returns>
    public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) =>
        Observable.Return(Unit.Default);

    /// <summary>
    /// Whens the navigated from.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    /// <returns>A Unit.</returns>
    public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) =>
        Observable.Return(Unit.Default);

    /// <summary>
    /// Whens the navigating to.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    /// <returns>A Unit.</returns>
    public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) =>
        Observable.Return(Unit.Default);

    /// <summary>
    /// Destroy the destructible object.
    /// </summary>
    public void Destroy() => Debug.WriteLine($"Destroy: {nameof(GreenViewModel)}");
}
