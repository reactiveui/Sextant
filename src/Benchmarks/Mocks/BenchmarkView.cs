// Copyright (c) 2025 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Concurrency;
using ReactiveUI;

namespace Sextant.Benchmarks
{
    /// <summary>
    /// BenchmarkView for benchmarking.
    /// </summary>
    /// <seealso cref="BenchmarkView" />
    public class BenchmarkView : IView
    {
        private readonly IView _view;

        /// <summary>
        /// Initializes a new instance of the <see cref="BenchmarkView"/> class.
        /// </summary>
        public BenchmarkView() => _view = new BenchmarkView();

        /// <summary>
        /// Gets the main thread scheduler for the <see cref="T:Sextant.IView" /> instance.
        /// </summary>
        public IScheduler MainThreadScheduler => RxApp.MainThreadScheduler;

        /// <summary>
        /// Gets an observable notifying that a page was popped from the navigation stack.
        /// </summary>
        IObservable<IViewModel?> IView.PagePopped => _view.PagePopped;

        /// <inheritdoc />
        public IObservable<Unit> PopModal() => _view.PopModal();

        /// <inheritdoc />
        public IObservable<Unit> PopPage(bool animate = true) => _view.PopPage(animate);

        /// <inheritdoc />
        public IObservable<Unit> PopToRootPage(bool animate = true) => _view.PopToRootPage(animate);

        /// <summary>
        /// Pushes the modal onto the modal stack.
        /// </summary>
        /// <param name="modalViewModel">The modal view model.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="withNavigationPage">Value indicating whether to wrap the modal in a navigation page.</param>
        /// <returns>
        /// An observable that signals when the push has been completed.
        /// </returns>
        public IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract, bool withNavigationPage = true) =>
            _view.PushModal(modalViewModel, contract, withNavigationPage);

        /// <summary>
        /// Pushes the page onto the navigation stack.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="resetStack">if set to <c>true</c> [reset stack].</param>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns>
        /// An observable that signals when the push has been completed.
        /// </returns>
        public IObservable<Unit> PushPage(IViewModel viewModel, string? contract, bool resetStack, bool animate = true) =>
            _view.PushPage(viewModel, contract, resetStack, animate);
    }
}
