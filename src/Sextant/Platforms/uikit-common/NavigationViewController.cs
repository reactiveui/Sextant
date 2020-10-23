// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using CoreAnimation;
using ReactiveUI;
using UIKit;

namespace Sextant
{
    /// <summary>
    /// A view that can load navigation to other controllers.
    /// </summary>
    /// <seealso cref="UINavigationController" />
    /// <seealso cref="IView" />
    [SuppressMessage("Design", "CA1010: Implement generic IEnumerable", Justification = "Base class declared IEnumerable.")]
    public class NavigationViewController : UINavigationController, IView
    {
        private readonly IScheduler _backgroundScheduler;
        private readonly IScheduler _mainScheduler;
        private readonly IViewLocator _viewLocator;
        private readonly Stack<UIViewController> _navigationPages = new Stack<UIViewController>();
        private readonly Subject<IViewModel?> _pagePopped = new Subject<IViewModel?>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationViewController" /> class.
        /// </summary>
        /// <param name="mainScheduler">The main scheduler.</param>
        /// <param name="backgroundScheduler">The background scheduler.</param>
        /// <param name="viewLocator">The view locator.</param>
        public NavigationViewController(
            IScheduler? mainScheduler = null,
            IScheduler? backgroundScheduler = null,
            IViewLocator? viewLocator = null)
        {
            _mainScheduler = mainScheduler ?? RxApp.MainThreadScheduler;
            _backgroundScheduler = backgroundScheduler ?? RxApp.TaskpoolScheduler;
            _viewLocator = viewLocator ?? ViewLocator.Current;
        }

        /// <inheritdoc />
        public IScheduler MainThreadScheduler => _mainScheduler;

        /// <inheritdoc />
        public IObservable<IViewModel?> PagePopped => _pagePopped;

        /// <inheritdoc />
        public IObservable<Unit> PopModal() =>
            DismissViewControllerAsync(true)
                .ToObservable()
                .Select(_ => Unit.Default)
                .ObserveOn(_mainScheduler);

        /// <inheritdoc />
        public IObservable<Unit> PopPage(bool animate = true) =>
            Observable.Create<Unit>(observable =>
            {
                CATransaction.Begin();
                CATransaction.CompletionBlock = () =>
                {
                    observable.OnNext(Unit.Default);
                    observable.OnCompleted();
                };
                PopViewController(animate);
                CATransaction.Commit();
                return Disposable.Empty;
            });

        /// <inheritdoc />
        public IObservable<Unit> PopToRootPage(bool animate = true) =>
            Observable.Create<Unit>(observable =>
            {
                CATransaction.Begin();
                CATransaction.CompletionBlock = () =>
                {
                    observable.OnNext(Unit.Default);
                    observable.OnCompleted();
                };
                PopToRootViewController(true);
                CATransaction.Commit();
                return Disposable.Empty;
            });

        /// <inheritdoc />
        public IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract, bool withNavigationPage = true) =>
            Observable.Start(
                    () =>
                    {
                        var page = LocatePageFor(modalViewModel, contract);
                        SetPageTitle(page, modalViewModel.Id);
                        return page;
                    },
                    CurrentThreadScheduler.Instance)
                .ObserveOn(CurrentThreadScheduler.Instance)
                .SelectMany(page => PresentViewControllerAsync(page, true).ToObservable());

        /// <inheritdoc />
        public IObservable<Unit> PushPage(
            IViewModel viewModel,
            string? contract,
            bool resetStack,
            bool animate = true)
        {
            return Observable.Start(
                    () =>
                    {
                        var page = LocatePageFor(viewModel, contract);
                        SetPageTitle(page, viewModel.Id);
                        return page;
                    },
                    CurrentThreadScheduler.Instance)
                .ObserveOn(CurrentThreadScheduler.Instance)
                .SelectMany(page =>
                {
                    return Observable.Create<Unit>(
                        observer =>
                        {
                            CATransaction.Begin();
                            CATransaction.CompletionBlock = () =>
                            {
                                observer.OnNext(Unit.Default);
                                observer.OnCompleted();
                            };

                            if (resetStack)
                            {
                                CATransaction.Begin();
                                CATransaction.CompletionBlock = () =>
                                {
                                    _navigationPages.Clear();
                                    _navigationPages.Push(this);
                                };
                                SetViewControllers(null, false);
                                CATransaction.Commit();
                            }

                            PushViewController(page, animate);
                            CATransaction.Commit();
                            return Disposable.Empty;
                        });
                });
        }

        /// <inheritdoc/>
        public override UIViewController PopViewController(bool animated)
        {
            var poppedController = base.PopViewController(animated);

            var view = poppedController as IViewFor;
            _pagePopped.OnNext(view?.ViewModel as IViewModel);

            return poppedController;
        }

        private UIViewController LocatePageFor(object viewModel, string? contract)
        {
            var viewFor = _viewLocator.ResolveView(viewModel, contract);
            var page = viewFor as UIViewController;

            if (viewFor == null)
            {
                throw new InvalidOperationException(
                    $"No view could be located for type '{viewModel.GetType().FullName}', contract '{contract}'. Be sure Splat has an appropriate registration.");
            }

            if (page == null)
            {
                throw new InvalidOperationException(
                    $"Resolved view '{viewFor.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' is not a Page.");
            }

            viewFor.ViewModel = viewModel;

            return page;
        }

        private void SetPageTitle(UIViewController page, string resourceKey)
        {
            page.Title = resourceKey;
        }
    }
}
