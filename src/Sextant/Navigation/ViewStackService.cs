// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Genesis.Logging;
using Sextant.Abstraction;

namespace Sextant
{
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    /// <summary>
    /// Service implementation to handle navigation stack updates.
    /// </summary>
    /// <remarks>
    /// Taken from https://kent-boogaart.com/blog/custom-routing-in-reactiveui blog post.
    /// </remarks>
    /// <seealso cref="IViewStackService" />
    public sealed class ViewStackService : IViewStackService
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

        /// <inheritdoc />
        public IObservable<IImmutableList<IPageViewModel>> ModalStack => _modalStack;

        /// <inheritdoc />
        public IObservable<IImmutableList<IPageViewModel>> PageStack => _pageStack;

        /// <inheritdoc />
        public IView View => _view;

        /// <inheritdoc />
        public IObservable<Unit> PopModal(bool animate = true) => _view.PopModal().Do(_ => { PopStackAndTick(_modalStack); });

        /// <inheritdoc />
        public IObservable<Unit> PopPage(bool animate = true) => _view.PopPage(animate);

        /// <inheritdoc />
        public IObservable<Unit> PopToRootPage(bool animate = true) => _view.PopToRootPage(animate);

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public IObservable<IPageViewModel> TopModal() => _modalStack.FirstAsync().Select(x => x.Last());

        /// <inheritdoc />
        public IObservable<IPageViewModel> TopPage() => _pageStack.FirstAsync().Select(x => x.Last());

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
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
