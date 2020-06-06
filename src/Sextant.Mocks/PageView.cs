// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using ReactiveUI;

[assembly: InternalsVisibleTo("Sextant.Tests")]

namespace Sextant.Mocks
{
    /// <summary>
    /// A mock of a page view.
    /// </summary>
    /// <seealso cref="ReactiveUI.IViewFor{PageViewModelMock}" />
    public class PageView : IViewFor<NavigableViewModelMock>
    {
        /// <summary>
        /// Gets or sets the ViewModel corresponding to this specific View. This should be
        /// a DependencyProperty if you're using XAML.
        /// </summary>
        object? IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (NavigableViewModelMock?)value;
        }

        /// <summary>
        /// Gets or sets the ViewModel corresponding to this specific View. This should be
        /// a DependencyProperty if you're using XAML.
        /// </summary>
        public NavigableViewModelMock? ViewModel { get; set; }
    }
}
