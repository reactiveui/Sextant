// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using ReactiveUI;

namespace Sextant
{
    /// <summary>
    /// <see cref="IViewStackService"/> implementation that passes <see cref="INavigationParameter"/> when navigating.
    /// </summary>
    /// <seealso cref="ViewStackServiceBase" />
    /// <seealso cref="IViewStackService" />
    public sealed class ParameterViewStackService : ViewStackServiceBase, IParameterViewStackService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterViewStackService"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public ParameterViewStackService(IView view)
            : base(view)
        {
        }

        /// <inheritdoc />
        public IObservable<Unit> PushPage(
            INavigable navigableViewModel,
            INavigationParameter parameter,
            string? contract = null,
            bool resetStack = false,
            bool animate = true)
        {
            if (navigableViewModel == null)
            {
                throw new ArgumentNullException(nameof(navigableViewModel));
            }

            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            return View
                .PushPage(navigableViewModel, contract, resetStack, animate)
                .Do(_ =>
                {
                    navigableViewModel
                        .WhenNavigatingTo(parameter)
                        .ObserveOn(View.MainThreadScheduler)
                        .Subscribe(navigating => Logger.Debug($"Called `WhenNavigatingTo` on '{navigableViewModel.Id}' passing parameter {parameter}"));

                    AddToStackAndTick(PageSubject, navigableViewModel, resetStack);
                    Logger.Debug($"Added page '{navigableViewModel.Id}' (contract '{contract}') to stack.");

                    navigableViewModel
                        .WhenNavigatedTo(parameter)
                        .ObserveOn(View.MainThreadScheduler)
                        .Subscribe(navigated => Logger.Debug($"Called `WhenNavigatedTo` on '{navigableViewModel.Id}' passing parameter {parameter}"));
                });
        }

        /// <inheritdoc />
        public IObservable<Unit> PushModal(INavigable navigableModal, INavigationParameter parameter, string? contract = null, bool withNavigationPage = true)
        {
            if (navigableModal == null)
            {
                throw new ArgumentNullException(nameof(navigableModal));
            }

            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            return View
                .PushModal(navigableModal, contract, withNavigationPage)
                .Do(_ =>
                {
                    navigableModal
                        .WhenNavigatingTo(parameter)
                        .ObserveOn(View.MainThreadScheduler)
                        .Subscribe(navigating => Logger.Debug($"Called `WhenNavigatingTo` on '{navigableModal.Id}' passing parameter {parameter}"));

                    AddToStackAndTick(ModalSubject, navigableModal, false);
                    Logger.Debug("Added modal '{modal.Id}' (contract '{contract}') to stack.");

                    navigableModal
                        .WhenNavigatedTo(parameter)
                        .ObserveOn(View.MainThreadScheduler)
                        .Subscribe(navigated => Logger.Debug($"Called `WhenNavigatedTo` on '{navigableModal.Id}' passing parameter {parameter}"));
                });
        }

        /// <inheritdoc />
        public IObservable<Unit> PushPage<TViewModel>(INavigationParameter parameter, string? contract = null, bool resetStack = false, bool animate = true)
            where TViewModel : INavigable
        {
            var viewModel = ViewModelFactory.Current.Create<TViewModel>();
            return PushPage(viewModel, parameter, contract, resetStack, animate);
        }

        /// <inheritdoc />
        public IObservable<Unit> PushModal<TViewModel>(INavigationParameter parameter, string? contract = null, bool withNavigationPage = true)
            where TViewModel : INavigable
        {
            var viewModel = ViewModelFactory.Current.Create<TViewModel>();
            return PushModal(viewModel, parameter, contract, withNavigationPage);
        }

        /// <inheritdoc />
        public IObservable<Unit> PopPage(INavigationParameter parameter, bool animate = true)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var topPage = TopPage().FirstOrDefaultAsync().Wait();
            return View
                .PopPage(animate)
                .Do(_ =>
                {
                    if (topPage is INavigable navigable)
                    {
                        navigable
                            .WhenNavigatedFrom(parameter)
                            .ObserveOn(View.MainThreadScheduler)
                            .Subscribe(navigated =>
                                Logger.Debug($"Called `WhenNavigatedFrom` on '{navigable.Id}' passing parameter {parameter}"));
                    }
                });
        }
    }
}
