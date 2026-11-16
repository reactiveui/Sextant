// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Reactive;
using System.Runtime.CompilerServices;
using Sextant;

namespace SextantSample.ViewModels;

/// <summary>Opens and closes the first modal page.</summary>
/// <seealso cref="ViewModelBase" />
/// <seealso cref="IDestructible" />
[System.Diagnostics.DebuggerDisplay("FirstModalViewModel: {OpenModal}")]
public class FirstModalViewModel : ViewModelBase, IDestructible
{
    /// <summary>Initializes a new instance of the <see cref="FirstModalViewModel"/> class.</summary>
    /// <param name="viewStackService">The view stack service.</param>
    public FirstModalViewModel(IViewStackService viewStackService)
        : base(viewStackService)
    {
        OpenModal = ReactiveCommand
                    .CreateFromObservable(() => ViewStackService!.PushModal(new SecondModalViewModel(viewStackService)), outputScheduler: RxSchedulers.MainThreadScheduler);

        PopModal = ReactiveCommand
                    .CreateFromObservable(() => ViewStackService!.PopModal(), outputScheduler: RxSchedulers.MainThreadScheduler);

        _ = OpenModal.Subscribe(static _ => Debug.WriteLine("PagePushed"));
        _ = PopModal.Subscribe(static _ => Debug.WriteLine("PagePopped"));
        _ = PopModal.ThrownExceptions.Subscribe(static error => Interactions.ErrorMessage.WhenHandled(error).Subscribe());
    }

    /// <summary>Gets or sets the open modal.</summary>
    /// <value>
    /// The open modal.
    /// </value>
    public ReactiveCommand<Unit, Unit> OpenModal { get; set; }

    /// <summary>Gets or sets the pop modal.</summary>
    /// <value>
    /// The pop modal.
    /// </value>
    public ReactiveCommand<Unit, Unit> PopModal { get; set; }

    /// <summary>Gets the identifier.</summary>
    /// <value>
    /// The identifier.
    /// </value>
    public override string Id => nameof(FirstModalViewModel);

    /// <summary>Destroy the destructible object.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Destroy() => Debug.WriteLine($"Destroy: {nameof(FirstModalViewModel)}");
}
