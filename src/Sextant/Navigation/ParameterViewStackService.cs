// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant
{
    /// <summary>
    /// <see cref="IViewStackService"/> implementation that passes <see cref="INavigationParameter"/> when navigating.
    /// </summary>
    /// <seealso cref="ViewStackServiceBase" />
    /// <seealso cref="IViewStackService" />
    public sealed class ParameterViewStackService : ParameterViewStackServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterViewStackService"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public ParameterViewStackService(IView view)
            : this(view, null!)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterViewStackService"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="viewModelFactory">The view model factory.</param>
        public ParameterViewStackService(IView view, IViewModelFactory viewModelFactory)
            : base(view, viewModelFactory)
        {
        }
    }
}
