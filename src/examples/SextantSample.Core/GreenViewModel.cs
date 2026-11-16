// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Sextant;

namespace SextantSample.ViewModels;

/// <summary>Offers modal navigation from the green page.</summary>
/// <seealso cref="ViewModelBase" />
/// <seealso cref="INavigable" />
/// <seealso cref="IDestructible" />
[System.Diagnostics.DebuggerDisplay("GreenViewModel: {Id}")]
public class GreenViewModel : ViewModelBase, INavigable, IDestructible
{
    /// <summary>Initializes a new instance of the <see cref="GreenViewModel"/> class.</summary>
    /// <param name="viewStackService">The view stack service.</param>
    public GreenViewModel(IViewStackService viewStackService)
        : base(viewStackService) =>
        OpenModal = ReactiveCommand
            .CreateFromObservable(() => ViewStackService!.PushModal(new FirstModalViewModel(viewStackService), string.Empty, false), outputScheduler: RxSchedulers.MainThreadScheduler);

    /// <summary>Gets the identifier.</summary>
    /// <value>
    /// The identifier.
    /// </value>
    public override string Id { get; } = string.Empty;

    /// <summary>Gets or sets the open modal.</summary>
    /// <value>
    /// The open modal.
    /// </value>
    public ReactiveCommand<Unit, Unit> OpenModal { get; set; }

    /// <summary>Whens the navigated to.</summary>
    /// <param name="parameter">The parameter.</param>
    /// <returns>A Unit.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) =>
        Observable.Return(Unit.Default);

    /// <summary>Whens the navigated from.</summary>
    /// <param name="parameter">The parameter.</param>
    /// <returns>A Unit.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) =>
        WhenNavigatedTo(parameter);

    /// <summary>Whens the navigating to.</summary>
    /// <param name="parameter">The parameter.</param>
    /// <returns>A Unit.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) =>
        WhenNavigatedFrom(parameter);

    /// <summary>Destroy the destructible object.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Destroy() => Debug.WriteLine($"Destroy: {nameof(GreenViewModel)}");
}
