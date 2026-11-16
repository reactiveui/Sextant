// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant.Mocks;

/// <summary>A mock of a page view.</summary>
/// <seealso cref="ReactiveUI.IViewFor{PageViewModelMock}" />
[System.Diagnostics.DebuggerDisplay("PageView: {ViewModel}")]
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
