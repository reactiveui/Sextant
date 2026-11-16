// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant;

/// <summary><see cref="IViewStackService"/> implementation that passes <see cref="INavigationParameter"/> when navigating.</summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ParameterViewStackService"/> class.
/// </remarks>
/// <seealso cref="ViewStackServiceBase" />
/// <seealso cref="IViewStackService" />
public sealed class ParameterViewStackService : ParameterViewStackServiceBase
{
    /// <summary>Initializes a new instance of the <see cref="ParameterViewStackService"/> class.</summary>
    /// <param name="view">The navigation view.</param>
    /// <param name="viewModelFactory">The view model factory.</param>
    public ParameterViewStackService(IView view, IViewModelFactory viewModelFactory)
        : base(view, viewModelFactory)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="ParameterViewStackService"/> class.</summary>
    /// <param name="view">The view.</param>
    public ParameterViewStackService(IView view)
        : this(view, new DefaultViewModelFactory())
    {
    }

    /// <summary>Initializes a new instance of the <see cref="ParameterViewStackService"/> class.</summary>
    /// <param name="view">The navigation view.</param>
    /// <param name="viewModelFactory">The view model factory.</param>
    /// <param name="logger">The navigation logger.</param>
    internal ParameterViewStackService(IView view, IViewModelFactory viewModelFactory, Splat.IFullLogger logger)
        : base(view, viewModelFactory, logger)
    {
    }
}
