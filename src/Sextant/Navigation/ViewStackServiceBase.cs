// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
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
using Splat;

namespace Sextant
{
    /// <summary>
    /// Abstract base class for view stack services.
    /// </summary>
    /// <seealso cref="IViewStackService" />
    /// <seealso cref="IDisposable" />
    /// <seealso cref="IEnableLogger" />
    public abstract class ViewStackServiceBase : IViewStackService, IDisposable, IEnableLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStackServiceBase"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        protected ViewStackServiceBase(IView view)
        {
            Logger = this.Log();
            View = view;
            ModalSubject = new BehaviorSubject<IImmutableList<IViewModel>>(ImmutableList<IViewModel>.Empty);
            PageSubject = new BehaviorSubject<IImmutableList<IViewModel>>(ImmutableList<IViewModel>.Empty);

            // TODO: Make this SubscribeSafe();
            View.PagePopped.Do(poppedPage =>
            {
                var currentPageStack = PageSubject.Value;
                if (currentPageStack.Count > 0 && poppedPage == currentPageStack[currentPageStack.Count - 1])
                {
                    var removedPage = PopStackAndTick(PageSubject);
                    Logger.Debug("Removed page '{0}' from stack.", removedPage.Id);
                }
            }).SubscribeSafe();
        }

        /// <summary>
        /// Gets the modal navigation stack.
        /// </summary>
        public IObservable<IImmutableList<IViewModel>> ModalStack => ModalSubject.AsObservable();

        /// <summary>
        /// Gets the page navigation stack.
        /// </summary>
        public IObservable<IImmutableList<IViewModel>> PageStack => PageSubject.AsObservable();

        /// <summary>
        /// Gets the current view on the stack.
        /// </summary>
        public IView View { get; private set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected IFullLogger Logger { get; private set; }

        /// <summary>
        /// Gets the modal subject.
        /// </summary>
        protected BehaviorSubject<IImmutableList<IViewModel>> ModalSubject { get; private set; }

        /// <summary>
        /// Gets the page subject.
        /// </summary>
        protected BehaviorSubject<IImmutableList<IViewModel>> PageSubject { get; private set; }

        /// <summary>
        /// Pops the <see cref="INavigable" /> off the stack.
        /// </summary>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns>An observable that signals when the pop is complete.</returns>
        public IObservable<Unit> PopModal(bool animate = true) => View.PopModal().Do(_ => PopStackAndTick(ModalSubject));

        /// <inheritdoc />
        public IObservable<Unit> PopPage(bool animate = true) => View.PopPage(animate);

        /// <inheritdoc />
        public IObservable<Unit> PopToRootPage(bool animate = true) => View.PopToRootPage(animate).Do(_ => PopRootAndTick(PageSubject));

        /// <inheritdoc />
        public IObservable<Unit> PushModal(IViewModel modal, string contract = null)
        {
            if (modal == null)
            {
                throw new ArgumentNullException(nameof(modal));
            }

            return View
                .PushModal(modal, contract)
                .Do(_ =>
                {
                    AddToStackAndTick(ModalSubject, modal, false);
                    Logger.Debug("Added modal '{modal.Id}' (contract '{contract}') to stack.");
                });
        }

        /// <inheritdoc />
        public IObservable<Unit> PushPage(
            INavigable viewModel,
            string contract = null,
            bool resetStack = false,
            bool animate = true) => PushPage((IViewModel)viewModel, contract, resetStack, animate);

        /// <inheritdoc />
        public IObservable<Unit> PushPage(IViewModel viewModel, string contract = null, bool resetStack = false, bool animate = true)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            return View
                .PushPage(viewModel, contract, resetStack, animate)
                .Do(_ =>
                {
                    AddToStackAndTick(PageSubject, viewModel, resetStack);
                    Logger.Debug($"Added page '{viewModel.Id}' (contract '{contract}') to stack.");
                });
        }

        /// <summary>
        /// Returns the top modal from the current modal stack.
        /// </summary>
        /// <returns>An observable that signals the top modal view model.</returns>
        [SuppressMessage("Design", "CA1826: Do not use Enumerable methods on indexable collections.", Justification = "Deliberate usage")]
        public IObservable<IViewModel> TopModal() => ModalSubject.FirstAsync().Select(x => x.Last());

        /// <summary>
        /// Returns the top page from the current navigation stack.
        /// </summary>
        /// <returns>An observable that signals the top page view model.</returns>
        [SuppressMessage("Design", "CA1826: Do not use Enumerable methods on indexable collections.", Justification = "Deliberate usage")]
        public IObservable<IViewModel> TopPage() => PageSubject.FirstAsync().Select(x => x.Last());

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Adds to stack and tick.
        /// </summary>
        /// <typeparam name="T">The view model type.</typeparam>
        /// <param name="stackSubject">The stack subject.</param>
        /// <param name="item">The item.</param>
        /// <param name="reset">if set to <c>true</c> [reset].</param>
        protected static void AddToStackAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject, T item, bool reset)
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

        /// <summary>
        /// Pops the stack and notifies observers.
        /// </summary>
        /// <typeparam name="T">The view model type.</typeparam>
        /// <param name="stackSubject">The stack subject.</param>
        /// <returns>The view model popped.</returns>
        /// <exception cref="InvalidOperationException">Stack is empty.</exception>
        protected static T PopStackAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject)
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

        /// <summary>
        /// Pops the root and notifies observers.
        /// </summary>
        /// <typeparam name="T">The view model type.</typeparam>
        /// <param name="stackSubject">The stack subject.</param>
        /// <exception cref="System.InvalidOperationException">Stack is empty.</exception>
        protected static void PopRootAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject)
        {
            IImmutableList<T> poppedStack = ImmutableList<T>.Empty;

            if (stackSubject?.Value == null || !stackSubject.Value.Any())
            {
                throw new InvalidOperationException("Stack is empty.");
            }

            stackSubject
                .Take(stackSubject.Value.Count - 1)
                .Where(stack => stack != null)
                .Subscribe(stack =>
                {
                    poppedStack = stack.RemoveRange(stack.IndexOf(stack[0]), stack.Count - 1);
                });

            stackSubject.OnNext(poppedStack);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ModalSubject?.Dispose();
                PageSubject?.Dispose();
            }
        }
    }
}
