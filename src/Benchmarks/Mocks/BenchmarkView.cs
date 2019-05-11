// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using ReactiveUI;

namespace Sextant.Benchmarks
{
    /// <summary>
    /// NavigationView for benchmarking.
    /// </summary>
    /// <seealso cref="NavigationView" />
    public class BenchmarkView : IView
    {
        private readonly IView _view;

        /// <summary>
        /// Initializes a new instance of the <see cref="BenchmarkView"/> class.
        /// </summary>
        public BenchmarkView()
        {
            _view = new BenchmarkView();
        }

        /// <inheritdoc />
        public IObservable<IPageViewModel> PagePopped => _view.PagePopped;

        /// <inheritdoc />
        public IObservable<Unit> PopModal() => _view.PopModal();

        /// <inheritdoc />
        public IObservable<Unit> PopPage(bool animate = true) => _view.PopPage(animate);

        /// <inheritdoc />
        public IObservable<Unit> PopToRootPage(bool animate = true) => _view.PopToRootPage(animate);

        /// <inheritdoc />
        public IObservable<Unit> PushModal(IPageViewModel modalViewModel, string contract) =>
            _view.PushModal(modalViewModel, contract);

        /// <inheritdoc />
        public IObservable<Unit> PushPage(
            IPageViewModel pageViewModel,
            string contract,
            bool resetStack,
            bool animate = true) => _view.PushPage(pageViewModel, contract, resetStack, animate);
    }
}
