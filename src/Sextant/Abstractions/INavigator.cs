using System;

namespace Sextant
{
    public interface ISextantNavigator
    {
        /// <summary>
        /// Navigate to the specified <see cref="ISextantViewModel"/>.
        /// </summary>
        /// <typeparam name="T">The view model type.</typeparam>
        /// <returns>A navigation result.</returns>
        IObservable<INavigationResult> Navigate<T>()
            where T : ISextantViewModel;

        /// <summary>
        /// Navigate to the specified <see cref="ISextantViewModel"/>.
        /// </summary>
        /// <param name="navigationParameter">The navigation parameter.</param>
        /// <typeparam name="T">The view model type.</typeparam>
        /// <returns>A navigation result.</returns>
        IObservable<INavigationResult> Navigate<T>(INavigationParameter navigationParameter)
            where T : ISextantViewModel;

        /// <summary>
        /// Navigates to the most recent view model back by popping the navigation stack.
        /// </summary>
        /// <returns>A navigation result.</returns>
        IObservable<INavigationResult> GoBack();

        /// <summary>
        /// Navigates to the most recent view model back by popping the navigation stack.
        /// </summary>
        /// <param name="navigationParameter">The navigation parameter.</param>
        /// <returns>A navigation result.</returns>
        IObservable<INavigationResult> GoBack(INavigationParameter navigationParameter);

        /// <summary>
        /// Navigates to the root view model popping all other view models.
        /// </summary>
        /// <param name="navigationParameter">The navigation parameter.</param>
        /// <returns>A navigation result.</returns>
        IObservable<INavigationResult> GoBackToRoot(INavigationParameter navigationParameter);
    }

    public interface ISextantModalNavigator
    {
        IObservable<INavigationResult> NavigateToModal<T>(bool animate)
            where T : ISextantViewModel;

        IObservable<INavigationResult> PopModal(bool animate);
    }

    public interface ISextantNavigatorAnimate
    {
        /// <summary>
        /// Navigate to the specified <see cref="ISextantViewModel"/>.
        /// </summary>
        /// <param name="navigationParameter">The navigation parameter.</param>
        /// <param name="animate">Animate the navigation.</param>
        /// <typeparam name="T">The view model type.</typeparam>
        /// <returns>A navigation result.</returns>
        IObservable<INavigationResult> Navigate<T>(INavigationParameter navigationParameter, bool animate)
            where T : INavigable;

        /// <summary>
        /// Navigate to the specified <see cref="ISextantViewModel"/>.
        /// </summary>
        /// <param name="animate">Animate the navigation.</param>
        /// <typeparam name="T">The view model type.</typeparam>
        /// <returns>A navigation result.</returns>
        IObservable<INavigationResult> Navigate<T>(bool animate)
            where T : ISextantViewModel;
    }

    /// <summary>
    /// Interface representing a sextant view model.
    /// </summary>
    public interface ISextantViewModel
    {
    }

    // TODO: [rlittlesii: December 30, 2020] Not sure if I want a Prism style navigation result, or allow the service to return a value like MVVMCross, maybe both?
    public interface INavigationResult
    {
        bool Succeeded { get; }

        Exception? Exception { get; }
    }
}
