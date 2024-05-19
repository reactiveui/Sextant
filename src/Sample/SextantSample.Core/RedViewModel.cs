// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Reactive;
using ReactiveUI;
using Sextant;

namespace SextantSample.ViewModels
{
    /// <summary>
    /// RedViewModel.
    /// </summary>
    /// <seealso cref="SextantSample.ViewModels.ViewModelBase" />
    public class RedViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RedViewModel"/> class.
        /// </summary>
        /// <param name="viewStackService">The view stack service.</param>
        public RedViewModel(IViewStackService viewStackService)
            : base(viewStackService)
        {
            PopModal = ReactiveCommand
                .CreateFromObservable(() => ViewStackService.PopModal(), outputScheduler: RxApp.MainThreadScheduler);

            PopPage = ReactiveCommand
                .CreateFromObservable(() => ViewStackService.PopPage(), outputScheduler: RxApp.MainThreadScheduler);

            PushPage = ReactiveCommand
                .CreateFromObservable(() => ViewStackService.PushPage(new RedViewModel(ViewStackService)), outputScheduler: RxApp.MainThreadScheduler);

            PopToRoot = ReactiveCommand
                .CreateFromObservable(() => ViewStackService.PopToRootPage(), outputScheduler: RxApp.MainThreadScheduler);

            PopModal.Subscribe(_ => Debug.WriteLine("PagePushed"));
            PopModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            PopPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            PushPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            PopToRoot.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }

        /// <summary>
        /// Gets or sets the pop modal.
        /// </summary>
        /// <value>
        /// The pop modal.
        /// </value>
        public ReactiveCommand<Unit, Unit> PopModal { get; set; }

        /// <summary>
        /// Gets or sets the push page.
        /// </summary>
        /// <value>
        /// The push page.
        /// </value>
        public ReactiveCommand<Unit, Unit> PushPage { get; set; }

        /// <summary>
        /// Gets or sets the pop page.
        /// </summary>
        /// <value>
        /// The pop page.
        /// </value>
        public ReactiveCommand<Unit, Unit> PopPage { get; set; }

        /// <summary>
        /// Gets or sets the pop to root.
        /// </summary>
        /// <value>
        /// The pop to root.
        /// </value>
        public ReactiveCommand<Unit, Unit> PopToRoot { get; set; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public override string Id => nameof(RedViewModel);
    }
}
