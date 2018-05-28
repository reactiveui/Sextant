using System;
using System.Collections.Immutable;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Sextant.Abstraction;

namespace Sextant
{
    /// <summary>
    /// Service implementation to handle navigation stack updates.
    /// Taken from https://kent-boogaart.com/blog/custom-routing-in-reactiveui
    /// </summary>
    /// <seealso cref="Sextant.Navigation.IViewStackService" />
    public class ViewStackService : IViewStackService
    {
        private BehaviorSubject<IImmutableList<IModalViewModel>> _modalStack;
        private BehaviorSubject<IImmutableList<IPageViewModel>> _pageStack;
        private IView _view;

        /// <summary>
        /// Gets the modal navigation stack.
        /// </summary>
        public IObservable<IImmutableList<IModalViewModel>> ModalStack => _modalStack;

        /// <summary>
        /// Gets the page navigation stack.
        /// </summary>
        public IObservable<IImmutableList<IPageViewModel>> PageStack => _pageStack;

        /// <summary>
        /// Gets the current view on the stack.
        /// </summary>
        public IView View => _view;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStackService"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public ViewStackService(IView view)
        {
            _view = view;
            _modalStack = new BehaviorSubject<IImmutableList<IModalViewModel>>(ImmutableList<IModalViewModel>.Empty);
            _pageStack = new BehaviorSubject<IImmutableList<IPageViewModel>>(ImmutableList<IPageViewModel>.Empty);

            // TODO: Make this SubscribeSafe();
            this._view.PagePopped.Do(poppedPage => { var currentPageStack = _pageStack.Value; if (currentPageStack.Count > 0 && poppedPage == currentPageStack[currentPageStack.Count - 1]) { var removedPage = PopStackAndTick(_pageStack); } }).Subscribe();
        }

        /// <summary>
        /// Pops the <see cref="IModalViewModel" /> off the stack.
        /// </summary>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns></returns>
        public IObservable<Unit> PopModal(bool animate = true) => _view.PopModal().Do(_ => { PopStackAndTick(_modalStack); });

        /// <summary>
        /// Pops the <see cref="IPageViewModel" /> off the stack.
        /// </summary>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns></returns>
        public IObservable<Unit> PopPage(bool animate = true) => _view.PopPage(animate);

        /// <summary>
        /// Pushes the <see cref="IModalViewModel" /> onto the stack.
        /// </summary>
        /// <param name="modal">The modal.</param>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        public IObservable<Unit> PushModal(IModalViewModel modal, string contract = null)
        {
            if (modal == null)
            {
                throw new ArgumentNullException(nameof(modal));
            }

            return _view.PushModal(modal, contract).Do(_ => AddToStackAndTick(_modalStack, modal, false));
        }

        /// <summary>
        /// Pushes the <see cref="IPageViewModel" /> onto the stack.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="resetStack">if set to <c>true</c> [reset stack].</param>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns></returns>
        public IObservable<Unit> PushPage(IPageViewModel page, string contract = null, bool resetStack = false, bool animate = true)
        {
            if (page == null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            return _view.PushPage(page, contract, resetStack, animate).Do(_ => AddToStackAndTick(_pageStack, page, resetStack));
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