using System;

namespace Sextant.Shell
{
    public interface IShellView : IView
    {
        /// <summary>
        /// Gets an observable notification of a popped view model.
        /// </summary>
        IObservable<NavigationEvent> PoppedNotification { get; }

        /// <summary>
        /// Gets an observable notification of popped to root.
        /// </summary>
        IObservable<NavigationEvent> PoppedToRootNotification { get; }

        /// <summary>
        /// Gets an observable notification of a pushed view model.
        /// </summary>
        IObservable<NavigationEvent> PushedNotification { get; }
    }
}
