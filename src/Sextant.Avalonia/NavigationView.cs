// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;

using ReactiveUI;

namespace Sextant.Avalonia
{
    /// <summary>
    /// The <see cref="IView"/> implementation for Avalonia.
    /// </summary>
    public sealed partial class NavigationView : ContentControl, IView
    {
        private readonly Navigation _modalNavigation = new();
        private readonly Navigation _pageNavigation = new();

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
                                .Select(count => count > 0 ? new SolidColorBrush(Colors.Transparent) : null)
                                .ToBinding()
                    }
                }
            };
        }

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

        /// <summary>
        /// Gets the type by which the element is styled.
        /// </summary>
        /// <remarks>
        /// Usually controls are styled by their own type, but there are instances where you want
        /// an element to be styled by its base type, e.g. creating SpecialButton that
        /// derives from Button and adds extra functionality but is still styled as a regular
        /// Button. Override this property to change the style for a control class, returning the
        /// type that you wish the elements to be styled as.
        /// </remarks>
        protected override Type StyleKeyOverride => typeof(ContentControl);

        /// <inheritdoc />
        public IObservable<Unit> PushPage(
            IViewModel viewModel,
            string? contract,
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
            string? contract,
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

        private IViewFor LocateView(IViewModel viewModel, string? contract)
        {
            var view = ViewLocator.ResolveView(viewModel, contract) ?? throw new InvalidOperationException(
                    $"No view could be located for type '{viewModel.GetType().FullName}', " +
                    $"contract '{contract}'. Be sure Splat has an appropriate registration.");
            view.ViewModel = viewModel;
            return view;
        }
    }
}
