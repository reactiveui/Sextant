using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Avalonia.Styling;
using DynamicData.Aggregation;
using DynamicData.Binding;
using ReactiveUI;
using Sextant;

namespace SextantSample.Avalonia.Views
{
    /// <summary>
    /// The <see cref="IView"/> implementation for Avalonia.
    /// </summary>
    public sealed class NavigationView : ContentControl, IStyleable, IView
    {
        private readonly Navigation _modalNavigation = new Navigation();
        private readonly Navigation _pageNavigation = new Navigation();

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationView"/> class.
        /// </summary>
        public NavigationView()
        {
            MainThreadScheduler = RxApp.MainThreadScheduler;
            ViewLocator = ReactiveUI.ViewLocator.Current;
            Content = new Grid
            {
                Children =
                {
                    new Grid
                    {
                        ZIndex = 1,
                        Children =
                        {
                            _pageNavigation.Control
                        }
                    },
                    new Grid
                    {
                        ZIndex = 2,
                        Children =
                        {
                            _modalNavigation.Control
                        },
                        [!Panel.BackgroundProperty] = 
                            _modalNavigation
                                .CountChanged
                                .Select(count => count > 0 ? new SolidColorBrush(Colors.White) : null)
                                .ToBinding()
                    }
                }
            };
        }

        /// <summary>
        /// Gets the style key of a <see cref="ContentControl"/>.
        /// </summary>
        Type IStyleable.StyleKey => typeof(ContentControl);

        /// <summary>
        /// Gets or sets the scheduler used by the <see cref="NavigationView"/>.
        /// </summary>
        public IScheduler MainThreadScheduler { get; set; }

        /// <summary>
        /// Gets or sets the scheduler used by the <see cref="NavigationView"/>.
        /// </summary>
        public IViewLocator ViewLocator { get; set; }

        /// <inheritdoc />
        public IObservable<IViewModel> PagePopped { get; } = Observable.Never<IViewModel>();

        /// <inheritdoc />
        public IObservable<Unit> PushPage(
            IViewModel viewModel,
            string contract,
            bool resetStack,
            bool animate = true)
        {
            var view = LocateView(viewModel, contract);
            _pageNavigation.ToggleAnimations(!_modalNavigation.IsVisible);
            _pageNavigation.Push(view, resetStack);
            return Observable.Return(Unit.Default);
        }

        /// <inheritdoc />
        public IObservable<Unit> PopPage(bool animate = true)
        {
            _pageNavigation.ToggleAnimations(!_modalNavigation.IsVisible);
            _pageNavigation.Pop();
            return Observable.Return(Unit.Default);
        }

        /// <inheritdoc />
        public IObservable<Unit> PopToRootPage(bool animate = true)
        {
            _pageNavigation.ToggleAnimations(!_modalNavigation.IsVisible);
            _pageNavigation.PopToRoot();
            return Observable.Return(Unit.Default);
        }

        /// <inheritdoc />
        public IObservable<Unit> PushModal(
            IViewModel modalViewModel,
            string contract,
            bool withNavigationPage = true)
        {
            var view = LocateView(modalViewModel, contract);
            _modalNavigation.Push(view);
            return Observable.Return(Unit.Default);
        }

        /// <inheritdoc />
        public IObservable<Unit> PopModal()
        {
            _modalNavigation.Pop();
            return Observable.Return(Unit.Default);
        }

        private IViewFor LocateView(IViewModel viewModel, string contract)
        {
            var view = ViewLocator.ResolveView(viewModel, contract);
            if (view is null)
            {
                throw new InvalidOperationException(
                    $"No view could be located for type '{viewModel.GetType().FullName}', " +
                    $"contract '{contract}'. Be sure Splat has an appropriate registration.");
            }

            view.ViewModel = viewModel;
            return view;
        }

        /// <summary>
        /// This class represents a single navigation layer.
        /// </summary>
        private class Navigation
        {
            private readonly ObservableCollection<IViewFor> _navigationStack = new ObservableCollection<IViewFor>();
            private readonly IPageTransition _transition;

            /// <summary>
            /// Initializes a new instance of the <see cref="Navigation"/> helper.
            /// </summary>
            public Navigation()
            {
                var navigationStackObservable = _navigationStack
                    .ToObservableChangeSet()
                    .Publish()
                    .RefCount();

                navigationStackObservable
                    .Select(_ => _navigationStack.LastOrDefault())
                    .Subscribe(page => Control.Content = page);

                CountChanged = navigationStackObservable.Count().AsObservable();
                _transition = Control.PageTransition;
            }

            /// <summary>
            /// Toggles the animations.
            /// </summary>
            /// <param name="enable">Returns true if we are enabling the animations.</param>
            public void ToggleAnimations(bool enable) => Control.PageTransition = enable ? _transition : null;

            /// <summary>
            /// Gets a bool indicating whether the control is visible.
            /// </summary>
            public bool IsVisible => _navigationStack.Count > 1;

            /// <summary>
            /// Publishes new values when page count changes.
            /// </summary>
            public IObservable<int> CountChanged { get; }

            /// <summary>
            /// The control responsible for rendering the current view.
            /// </summary>
            public TransitioningContentControl Control { get; } = new TransitioningContentControl();

            /// <summary>
            /// Adds a <see cref="IViewFor"/> to the navigation stack.
            /// </summary>
            /// <param name="view">The view to add to the navigation stack.</param>
            /// <param name="resetStack">Defines if we should reset the navigation stack.</param>
            public void Push(IViewFor view, bool resetStack = false)
            {
                if (resetStack)
                {
                    _navigationStack.Clear();
                }

                _navigationStack.Add(view);
            }

            /// <summary>
            /// Removes the last <see cref="IViewFor"/> from the navigation stack.
            /// </summary>
            public void Pop()
            {
                var indexToRemove = _navigationStack.Count - 1;
                _navigationStack.RemoveAt(indexToRemove);
            }

            /// <summary>
            /// Removes all pages except the first one <see cref="IViewFor"/> from the navigation stack.
            /// </summary>
            public void PopToRoot()
            {
                var view = _navigationStack[0];
                _navigationStack.Clear();
                _navigationStack.Add(view);
            }
        }
    }
}