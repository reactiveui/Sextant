// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant;

/// <summary>
/// Service implementation to handle navigation stack updates.
/// Taken from https://kent-boogaart.com/blog/custom-routing-in-reactiveui and adjusted.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ViewStackService"/> class.
/// </remarks>
/// <seealso cref="IViewStackService" />
public sealed class ViewStackService : ViewStackServiceBase
{
    /// <summary>Initializes a new instance of the <see cref="ViewStackService"/> class.</summary>
    /// <param name="view">The navigation view.</param>
    /// <param name="viewModelFactory">The view model factory.</param>
    public ViewStackService(IView view, IViewModelFactory viewModelFactory)
        : base(view, viewModelFactory)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="ViewStackService"/> class.</summary>
    /// <param name="view">The view.</param>
    public ViewStackService(IView view)
        : this(view, new DefaultViewModelFactory())
    {
    }

    /// <summary>Initializes a new instance of the <see cref="ViewStackService"/> class.</summary>
    /// <param name="view">The navigation view.</param>
    /// <param name="viewModelFactory">The view model factory.</param>
    /// <param name="logger">The navigation logger.</param>
    internal ViewStackService(IView view, IViewModelFactory viewModelFactory, Splat.IFullLogger logger)
        : base(view, viewModelFactory, logger)
    {
    }
}
