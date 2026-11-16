// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Reactive;
using Sextant;

namespace SextantSample.ViewModels;

/// <summary>Controls navigation from the second modal page.</summary>
/// <seealso cref="ViewModelBase" />
[System.Diagnostics.DebuggerDisplay("SecondModalViewModel: {PushPage}")]
public class SecondModalViewModel : ViewModelBase
{
    /// <summary>Initializes a new instance of the <see cref="SecondModalViewModel"/> class.</summary>
    /// <param name="viewStackService">The view stack service.</param>
    public SecondModalViewModel(IViewStackService viewStackService)
        : base(viewStackService)
    {
        PushPage = ReactiveCommand
            .CreateFromObservable(() => ViewStackService!.PushPage(new RedViewModel(ViewStackService)), outputScheduler: RxSchedulers.MainThreadScheduler);

        PopModal = ReactiveCommand
            .CreateFromObservable(() => ViewStackService!.PopModal(), outputScheduler: RxSchedulers.MainThreadScheduler);

        _ = PushPage.Subscribe(static _ => Debug.WriteLine("PagePushed"));
        _ = PopModal.Subscribe(static _ => Debug.WriteLine("PagePopped"));

        _ = PushPage.ThrownExceptions.Subscribe(static error => Interactions.ErrorMessage.WhenHandled(error).Subscribe());
        _ = PopModal.ThrownExceptions.Subscribe(static error => Interactions.ErrorMessage.WhenHandled(error).Subscribe());
    }

    /// <summary>Gets or sets the push page.</summary>
    /// <value>
    /// The push page.
    /// </value>
    public ReactiveCommand<Unit, Unit> PushPage { get; set; }

    /// <summary>Gets or sets the pop modal.</summary>
    /// <value>
    /// The pop modal.
    /// </value>
    public ReactiveCommand<Unit, Unit> PopModal { get; set; }

    /// <summary>Gets the identifier.</summary>
    /// <value>
    /// The identifier.
    /// </value>
    public override string Id => nameof(SecondModalViewModel);
}
