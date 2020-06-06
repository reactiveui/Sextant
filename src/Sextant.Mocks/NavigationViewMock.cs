// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Sextant.Mocks
{
    /// <summary>
    /// Mock <see cref="IView"/> implementation.
    /// </summary>
    public class NavigationViewMock : IView, IDisposable
    {
        private Subject<IViewModel> _pagePoppedSubject;
        private Stack<IViewModel> _pageStack;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationViewMock"/> class.
        /// </summary>
        public NavigationViewMock()
        {
            _pagePoppedSubject = new Subject<IViewModel>();
            _pageStack = new Stack<IViewModel>();

            PagePopped = _pagePoppedSubject.AsObservable();
        }

        /// <inheritdoc/>
        public IScheduler MainThreadScheduler { get; } = CurrentThreadScheduler.Instance;

        /// <inheritdoc/>
        public IObservable<IViewModel?> PagePopped { get; }

        /// <inheritdoc/>
        public IObservable<Unit> PopModal() => throw new NotImplementedException();

        /// <inheritdoc/>
        public IObservable<Unit> PopPage(bool animate = true) =>
            Observable
                .Return(Unit.Default)
                .Do(_ => _pagePoppedSubject.OnNext(_pageStack.Pop()));

        /// <inheritdoc/>
        public IObservable<Unit> PopToRootPage(bool animate = true) => throw new NotImplementedException();

        /// <inheritdoc/>
        public IObservable<Unit> PushModal(IViewModel modalViewModel, string? contract, bool withNavigationPage = true) => throw new NotImplementedException();

        /// <inheritdoc/>
        public IObservable<Unit> PushPage(IViewModel viewModel, string? contract, bool resetStack, bool animate = true) =>
            Observable
                .Return(Unit.Default)
                .Do(_ => _pageStack.Push(viewModel));

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pagePoppedSubject?.Dispose();
            }
        }
    }
}
