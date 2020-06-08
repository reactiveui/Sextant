// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Sextant
{
    /// <summary>
    /// Abstract base class for view stack services.
    /// </summary>
    public abstract class ParameterViewStackServiceBase : ViewStackServiceBase, IParameterViewStackService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterViewStackServiceBase"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="viewModelFactory">The view model factory.</param>
        protected ParameterViewStackServiceBase(IView view, IViewModelFactory viewModelFactory)
            : base(view, viewModelFactory)
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

            navigableViewModel
                .WhenNavigatingTo(parameter)
                .ObserveOn(View.MainThreadScheduler)
                .Subscribe(navigating =>
                    Logger.Debug(
                        $"Called `WhenNavigatingTo` on '{navigableViewModel.Id}' passing parameter {parameter}"));

            return View
                .PushPage(navigableViewModel, contract, resetStack, animate)
                .Do(_ =>
                {
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

            navigableModal
                .WhenNavigatingTo(parameter)
                .ObserveOn(View.MainThreadScheduler)
                .Subscribe(navigating =>
                    Logger.Debug($"Called `WhenNavigatingTo` on '{navigableModal.Id}' passing parameter {parameter}"));

            return View
                .PushModal(navigableModal, contract, withNavigationPage)
                .Do(_ =>
                {
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
            var viewModel = Factory.Create<TViewModel>();
            return PushPage(viewModel, parameter, contract, resetStack, animate);
        }

        /// <inheritdoc />
        public IObservable<Unit> PushModal<TViewModel>(INavigationParameter parameter, string? contract = null, bool withNavigationPage = true)
            where TViewModel : INavigable
        {
            var viewModel = Factory.Create<TViewModel>(contract);
            return PushModal(viewModel, parameter, contract, withNavigationPage);
        }

        /// <inheritdoc />
        public IObservable<Unit> PopPage(INavigationParameter parameter, bool animate = true)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            IViewModel poppedPage = TopPage().FirstOrDefaultAsync().Wait();
            return View
                .PopPage(animate)
                .Do(_ =>
                {
                    poppedPage
                        .InvokeViewModelAction<INavigable>(x =>
                            x.WhenNavigatedFrom(parameter)
                                .ObserveOn(View.MainThreadScheduler)
                                .Subscribe(navigatedFrom =>
                                    Logger.Debug($"Called `WhenNavigatedFrom` on '{poppedPage.Id}' passing parameter {parameter}")))
                        .InvokeViewModelAction<IDestructible>(x => x.Destroy());

                    IViewModel topPage = TopPage().FirstOrDefaultAsync().Wait();
                    if (topPage is INavigated navigated)
                    {
                        navigated
                            .WhenNavigatedTo(parameter)
                            .ObserveOn(View.MainThreadScheduler)
                            .Subscribe(navigatedTo =>
                                Logger.Debug($"Called `WhenNavigatedTo` passing parameter {parameter}"));
                    }
                });
        }
    }
}
