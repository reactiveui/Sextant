﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Genesis.Logging;

namespace Sextant
{
    /// <summary>
    /// Service implementation to handle navigation stack updates.
    /// Taken from https://kent-boogaart.com/blog/custom-routing-in-reactiveui and adjusted.
    /// </summary>
    /// <seealso cref="IViewStackService" />
    public sealed class ViewStackService : IViewStackService, IDisposable
    {
        private readonly ILogger _logger;
        private readonly BehaviorSubject<IImmutableList<IPageViewModel>> _modalStack;
        private readonly BehaviorSubject<IImmutableList<IPageViewModel>> _pageStack;
        private readonly IView _view;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStackService"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public ViewStackService(IView view)
        {
            _logger = LoggerService.GetLogger(GetType());
            _view = view;
            _modalStack = new BehaviorSubject<IImmutableList<IPageViewModel>>(ImmutableList<IPageViewModel>.Empty);
            _pageStack = new BehaviorSubject<IImmutableList<IPageViewModel>>(ImmutableList<IPageViewModel>.Empty);

            // TODO: Make this SubscribeSafe();
            _view.PagePopped.Do(poppedPage =>
            {
                var currentPageStack = _pageStack.Value;
                if (currentPageStack.Count > 0 && poppedPage == currentPageStack[currentPageStack.Count - 1])
                {
                    var removedPage = PopStackAndTick(_pageStack);
                    _logger.Debug("Removed page '{0}' from stack.", removedPage.Id);
                }
            }).SubscribeSafe();
        }

        /// <summary>
        /// Gets the modal navigation stack.
        /// </summary>
        public IObservable<IImmutableList<IPageViewModel>> ModalStack => _modalStack;

        /// <summary>
        /// Gets the page navigation stack.
        /// </summary>
        public IObservable<IImmutableList<IPageViewModel>> PageStack => _pageStack;

        /// <summary>
        /// Gets the current view on the stack.
        /// </summary>
        public IView View => _view;

        /// <summary>
        /// Pops the <see cref="IPageViewModel" /> off the stack.
        /// </summary>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns>An observable that signals when the pop is complete.</returns>
        public IObservable<Unit> PopModal(bool animate = true) => _view.PopModal().Do(_ => { PopStackAndTick(_modalStack); });

        /// <summary>
        /// Pops the <see cref="IPageViewModel" /> off the stack.
        /// </summary>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns>An observable that signals when the pop is complete.</returns>
        public IObservable<Unit> PopPage(bool animate = true) => _view.PopPage(animate);

        /// <summary>
        /// Pops to root page.
        /// </summary>
        /// <returns>The to root page.</returns>
        /// <param name="animate">If set to <c>true</c> animate.</param>
        public IObservable<Unit> PopToRootPage(bool animate = true) => _view.PopToRootPage(animate);

        /// <summary>
        /// Pushes the <see cref="IPageViewModel" /> onto the stack.
        /// </summary>
        /// <param name="modal">The modal.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>An observable that signals when the push is complete.</returns>
        public IObservable<Unit> PushModal(IPageViewModel modal, string contract = null)
        {
            if (modal == null)
            {
                throw new ArgumentNullException(nameof(modal));
            }

            return _view
                .PushModal(modal, contract)
                .Do(_ =>
                {
                    AddToStackAndTick(_modalStack, modal, false);
                    _logger.Debug("Added modal '{modal.Id}' (contract '{contract}') to stack.");
                });
        }

        /// <summary>
        /// Pushes the <see cref="IPageViewModel" /> onto the stack.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="resetStack">if set to <c>true</c> [reset stack].</param>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns>An observable that signals when the push is complete.</returns>
        public IObservable<Unit> PushPage(IPageViewModel page, string contract = null, bool resetStack = false, bool animate = true)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            return _view
                .PushPage(page, contract, resetStack, animate)
                .Do(_ =>
                {
                    AddToStackAndTick(_pageStack, page, resetStack);
                    _logger.Debug($"Added page '{page.Id}' (contract '{contract}') to stack.");
                });
        }

        /// <summary>
        /// Returns the top modal from the current modal stack.
        /// </summary>
        /// <returns>An observable that signals the top modal view model.</returns>
        [SuppressMessage("Design", "CA1826: Do not use Enumerable methods on indexable collections.", Justification = "Deliberate usage")]
        public IObservable<IPageViewModel> TopModal() => _modalStack.FirstAsync().Select(x => x.Last());

        /// <summary>
        /// Returns the top page from the current navigation stack.
        /// </summary>
        /// <returns>An observable that signals the top page view model.</returns>
        [SuppressMessage("Design", "CA1826: Do not use Enumerable methods on indexable collections.", Justification = "Deliberate usage")]
        public IObservable<IPageViewModel> TopPage() => _pageStack.FirstAsync().Select(x => x.Last());

        /// <inheritdoc />
        public void Dispose()
        {
            _modalStack?.Dispose();
            _pageStack?.Dispose();
        }

        private static void AddToStackAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject, T item, bool reset)
        {
            var stack = stackSubject.Value;

            if (reset)
            {
                stack = new[] { item }.ToImmutableList();
            }
            else
            {
                stack = stack.Add(item);
            }

            stackSubject.OnNext(stack);
        }

        private static T PopStackAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject)
        {
            var stack = stackSubject.Value;

            if (stack.Count == 0)
            {
                throw new InvalidOperationException("Stack is empty.");
            }

            var removedItem = stack[stack.Count - 1];
            stack = stack.RemoveAt(stack.Count - 1);
            stackSubject.OnNext(stack);
            return removedItem;
        }
    }
}
