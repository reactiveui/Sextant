// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System.Reactive;
using Sextant;
using Splat;

namespace SextantSample.ViewModels;

/// <summary>Offers the sample navigation commands.</summary>
/// <seealso cref = "ViewModelBase"/>
[System.Diagnostics.DebuggerDisplay("HomeViewModel: {Id}")]
public class HomeViewModel : ViewModelBase
{
    /// <summary>Initializes a new instance of the <see cref = "HomeViewModel"/> class.</summary>
    public HomeViewModel()
        : base(AppLocator.Current.GetService<IViewStackService>())
    {
        OpenModal = ReactiveCommand.CreateFromObservable(() => ViewStackService!.PushModal(new FirstModalViewModel(ViewStackService)), outputScheduler: RxSchedulers.MainThreadScheduler);
        PushPage = ReactiveCommand.CreateFromObservable(() => ViewStackService!.PushPage(new RedViewModel(ViewStackService)), outputScheduler: RxSchedulers.MainThreadScheduler);
        PushGenericPage = ReactiveCommand.CreateFromObservable(() => ViewStackService!.PushPage<GreenViewModel>(), outputScheduler: RxSchedulers.MainThreadScheduler);
        _ = PushPage.ThrownExceptions.Subscribe(static error => Interactions.ErrorMessage.WhenHandled(error).Subscribe());
        _ = PushGenericPage.ThrownExceptions.Subscribe(static error => Interactions.ErrorMessage.WhenHandled(error).Subscribe());
        _ = OpenModal.ThrownExceptions.Subscribe(static error => Interactions.ErrorMessage.WhenHandled(error).Subscribe());
    }

    /// <summary>Gets the identifier.</summary>
    /// <value>
    /// The identifier.
    /// </value>
    public override string Id => nameof(HomeViewModel);

    /// <summary>Gets or sets the open modal.</summary>
    /// <value>
    /// The open modal.
    /// </value>
    public ReactiveCommand<Unit, Unit> OpenModal { get; set; }

    /// <summary>Gets or sets the push page.</summary>
    /// <value>
    /// The push page.
    /// </value>
    public ReactiveCommand<Unit, Unit> PushPage { get; set; }

    /// <summary>Gets or sets the push generic page.</summary>
    /// <value>
    /// The push generic page.
    /// </value>
    public ReactiveCommand<Unit, Unit> PushGenericPage { get; set; }
}
