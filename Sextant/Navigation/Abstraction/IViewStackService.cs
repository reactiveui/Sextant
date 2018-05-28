using System;
using System.Collections.Immutable;
using System.Reactive;

namespace Sextant.Abstraction
{
    /// <summary>
    /// Interface that defines a methods to interact with the navigation stack.
    /// </summary>
    public interface IViewStackService
    {
        /// <summary>
        /// Gets the modal navigation stack.
        /// </summary>
        IObservable<IImmutableList<IModalViewModel>> ModalStack { get; }

        /// <summary>
        /// Gets the page navigation stack.
        /// </summary>
        IObservable<IImmutableList<IPageViewModel>> PageStack { get; }

        /// <summary>
        /// Gets the current view on the stack.
        /// </summary>
        IView View { get; }

        /// <summary>
        /// Pops the <see cref="IModalViewModel"/> off the stack.
        /// </summary>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns></returns>
        IObservable<Unit> PopModal(bool animate = true);

        /// <summary>
        /// Pops the <see cref="IPageViewModel"/> off the stack.
        /// </summary>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns></returns>
        IObservable<Unit> PopPage(bool animate = true);

        /// <summary>
        /// Pushes the <see cref="IModalViewModel"/> onto the stack.
        /// </summary>
        /// <param name="modal">The modal.</param>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        IObservable<Unit> PushModal(IModalViewModel modal, string contract = null);

        /// <summary>
        /// Pushes the <see cref="IPageViewModel"/> onto the stack.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="resetStack">if set to <c>true</c> [reset stack].</param>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns></returns>
        IObservable<Unit> PushPage(IPageViewModel page, string contract = null, bool resetStack = false, bool animate = true);
    }
}