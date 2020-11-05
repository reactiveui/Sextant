// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

[assembly:SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Contains generic.")]

namespace Sextant.Plugins.Popup
{
    /// <summary>
    /// Base Popup page for that implements <see cref="IViewFor"/>.
    /// </summary>
    /// <typeparam name="TViewModel">The view model type.</typeparam>
    public abstract class SextantPopupPage<TViewModel> : SextantPopupPage, IViewFor<TViewModel>
        where TViewModel : class, IViewModel
    {
        /// <summary>
        /// The view model property.
        /// </summary>
        public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(
            nameof(ViewModel),
            typeof(TViewModel),
            typeof(IViewFor<TViewModel>),
            (IViewFor<TViewModel>)null,
            BindingMode.OneWay,
            (BindableProperty.ValidateValueDelegate)null,
            new BindableProperty.BindingPropertyChangedDelegate(OnViewModelChanged),
            (BindableProperty.BindingPropertyChangingDelegate)null,
            (BindableProperty.CoerceValueDelegate)null,
            (BindableProperty.CreateDefaultValueDelegate)null);

        /// <summary>
        /// Gets or sets the ViewModel to display.
        /// </summary>
        public new TViewModel ViewModel
        {
            get => (TViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        /// <summary>
        /// Gets or sets the ViewModel corresponding to this specific View.
        /// This should be a BindableProperty if you're using XAML.
        /// </summary>
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel)value;
        }

        /// <inheritdoc/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            ViewModel = (BindingContext as TViewModel)!;
        }

        private static void OnViewModelChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            bindableObject.BindingContext = newValue;
        }
    }

    /// <summary>
    /// Base Popup page for that implements <see cref="IViewFor"/>.
    /// </summary>
    public abstract class SextantPopupPage : PopupPage, IViewFor
    {
        /// <summary>
        /// The view model property.
        /// </summary>
        public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(
            nameof(ViewModel),
            typeof(object),
            typeof(IViewFor<object>),
            (object)null,
            BindingMode.OneWay,
            (BindableProperty.ValidateValueDelegate)null,
            new BindableProperty.BindingPropertyChangedDelegate(OnViewModelChanged),
            (BindableProperty.BindingPropertyChangingDelegate)null,
            (BindableProperty.CoerceValueDelegate)null,
            (BindableProperty.CreateDefaultValueDelegate)null);

        /// <summary>
        /// Initializes a new instance of the <see cref="SextantPopupPage"/> class.
        /// </summary>
        protected SextantPopupPage()
        {
            BackgroundClick =
                Observable.FromEvent<EventHandler, Unit>(
                        handler =>
                        {
                            void EventHandler(object sender, EventArgs args) => handler(Unit.Default);
                            return EventHandler;
                        },
                        x => BackgroundClicked += x,
                        x => BackgroundClicked -= x);
        }

        /// <summary>
        /// Gets the background click observable signal.
        /// </summary>
        /// <value>The background click.</value>
        public IObservable<Unit> BackgroundClick { get; }

        /// <summary>
        /// Gets or sets the ViewModel to display.
        /// </summary>
        public object ViewModel
        {
            get => GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        /// <inheritdoc/>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            ViewModel = BindingContext;
        }

        private static void OnViewModelChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            bindableObject.BindingContext = newValue;
        }
    }
}
