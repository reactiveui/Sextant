// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Reactive;
using Sextant;

namespace SextantSample.ViewModels;

/// <summary>Offers navigation from the red sample page.</summary>
/// <seealso cref="ViewModelBase" />
[System.Diagnostics.DebuggerDisplay("RedViewModel: {PopModal}")]
public class RedViewModel : ViewModelBase
{
    /// <summary>Initializes a new instance of the <see cref="RedViewModel"/> class.</summary>
    /// <param name="viewStackService">The view stack service.</param>
    public RedViewModel(IViewStackService viewStackService)
        : base(viewStackService)
    {
        PopModal = ReactiveCommand
            .CreateFromObservable(() => ViewStackService!.PopModal(), outputScheduler: RxSchedulers.MainThreadScheduler);

        PopPage = ReactiveCommand
            .CreateFromObservable(() => ViewStackService!.PopPage(), outputScheduler: RxSchedulers.MainThreadScheduler);

        PushPage = ReactiveCommand
            .CreateFromObservable(() => ViewStackService!.PushPage(new RedViewModel(ViewStackService)), outputScheduler: RxSchedulers.MainThreadScheduler);

        PopToRoot = ReactiveCommand
            .CreateFromObservable(() => ViewStackService!.PopToRootPage(), outputScheduler: RxSchedulers.MainThreadScheduler);

        _ = PopModal.Subscribe(static _ => Debug.WriteLine("PagePushed"));
        _ = PopModal.ThrownExceptions.Subscribe(static error => Interactions.ErrorMessage.WhenHandled(error).Subscribe());
        _ = PopPage.ThrownExceptions.Subscribe(static error => Interactions.ErrorMessage.WhenHandled(error).Subscribe());
        _ = PushPage.ThrownExceptions.Subscribe(static error => Interactions.ErrorMessage.WhenHandled(error).Subscribe());
        _ = PopToRoot.ThrownExceptions.Subscribe(static error => Interactions.ErrorMessage.WhenHandled(error).Subscribe());
    }

    /// <summary>Gets or sets the pop modal.</summary>
    /// <value>
    /// The pop modal.
    /// </value>
    public ReactiveCommand<Unit, Unit> PopModal { get; set; }

    /// <summary>Gets or sets the push page.</summary>
    /// <value>
    /// The push page.
    /// </value>
    public ReactiveCommand<Unit, Unit> PushPage { get; set; }

    /// <summary>Gets or sets the pop page.</summary>
    /// <value>
    /// The pop page.
    /// </value>
    public ReactiveCommand<Unit, Unit> PopPage { get; set; }

    /// <summary>Gets or sets the pop to root.</summary>
    /// <value>
    /// The pop to root.
    /// </value>
    public ReactiveCommand<Unit, Unit> PopToRoot { get; set; }

    /// <summary>Gets the identifier.</summary>
    /// <value>
    /// The identifier.
    /// </value>
    public override string Id => nameof(RedViewModel);
}
