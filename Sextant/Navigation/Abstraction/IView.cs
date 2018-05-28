using System;
using System.Reactive;

namespace Sextant.Abstraction
{
    /// <summary>
    /// Defines a view that be add to a navigation or modal stack.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// An observable notifying that a page was popped from the navigation stack.
        /// </summary>
        IObservable<IPageViewModel> PagePopped { get; }

        /// <summary>
        /// Pops the modal from the modal stack.
        /// </summary>
        /// <returns></returns>
        IObservable<Unit> PopModal();

        /// <summary>
        /// Pops the page from the navigation stack.
        /// </summary>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns></returns>
        IObservable<Unit> PopPage(bool animate = true);

        /// <summary>
        /// Pushes the modal onto the modal stack.
        /// </summary>
        /// <param name="modalViewModel">The modal view model.</param>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        IObservable<Unit> PushModal(IPageViewModel modalViewModel, string contract);

        /// <summary>
        /// Pushes the page onto the navigation stack.
        /// </summary>
        /// <param name="pageViewModel">The page view model.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="resetStack">if set to <c>true</c> [reset stack].</param>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns></returns>
        IObservable<Unit> PushPage(IPageViewModel pageViewModel,
            string contract,
            bool resetStack,
            bool animate = true);
    }
}