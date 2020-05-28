using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using PropertyChangingEventHandler = System.ComponentModel.PropertyChangingEventHandler;

namespace Sextant.Shell
{
    public interface IShell : IShellController
    {
        /// <summary>
        /// The navigated event handler.
        /// </summary>
        event EventHandler<ShellNavigatedEventArgs> Navigated;

        /// <summary>
        /// The navigating event handler.
        /// </summary>
        event EventHandler<ShellNavigatingEventArgs> Navigating;

        /// <summary>
        /// The appearing event.
        /// </summary>
        event EventHandler Appearing;

        /// <summary>
        /// The disappearing event.
        /// </summary>
        event EventHandler Disappearing;

        /// <summary>
        /// Property Changing event handler.
        /// </summary>
        event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// Property Changing event handler.
        /// </summary>
        event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Structure changed event handler.
        /// </summary>
        event EventHandler StructureChanged;

        /// <summary>
        /// Item collection changed.
        /// </summary>
        event NotifyCollectionChangedEventHandler ItemsCollectionChanged;

        /// <summary>
        /// Gets or sets the current displayed <see cref="ShellItem"/>.
        /// </summary>
        ShellItem CurrentItem { get; set; }

        /// <summary>
        /// Gets the current <see cref="ShellRouteState"/>.
        /// </summary>
        ShellRouteState RouteState { get; }

        /// <summary>
        /// Gets the current <see cref="ShellNavigationState"/>.
        /// </summary>
        ShellNavigationState CurrentState { get; }

        /// <summary>
        /// Gets the list of current shell items.
        /// </summary>
        IList<ShellItem> Items { get; }

        /// <summary>
        /// Sets the Shell Navigation service.
        /// </summary>
        /// <param name="service">The service.</param>
        void SetNavigationService(object service);

        /// <summary>
        /// Navigate to the provided state.
        /// </summary>
        /// <param name="state">The navigation state.</param>
        /// <returns>A completion notification.</returns>
        Task GoToAsync(ShellNavigationState state);

        /// <summary>
        /// Navigate to the provided state.
        /// </summary>
        /// <param name="state">The navigation state.</param>
        /// <param name="animate">Animate the navigation.</param>
        /// <returns>A completion notification.</returns>
        Task GoToAsync(ShellNavigationState state, bool animate);
    }

    public interface IShellRouteConverter
    {
        ShellNavigationState Convert(IRoute route);
    }

    public interface IRoute
    {
    }
}
