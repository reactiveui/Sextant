// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;

namespace Sextant.Benchmarks
{
    /// <summary>
    /// View for the locator.
    /// </summary>
    /// <seealso cref="ReactiveUI.IViewFor{Benchmarks.ViewModel}" />
    public class TestView : IViewFor<ViewModel>
    {
        /// <summary>
        /// Gets or sets the ViewModel corresponding to this specific View. This should be
        /// a DependencyProperty if you're using XAML.
        /// </summary>
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ViewModel)value;
        }

        /// <summary>
        /// Gets or sets the ViewModel corresponding to this specific View. This should be
        /// a DependencyProperty if you're using XAML.
        /// </summary>
        public ViewModel ViewModel { get; set; }
    }
}
