// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Sextant;

/// <summary>Class of extension method for object life cycle in Sextant.</summary>
public static class ViewModelActionExtensions
{
    /// <summary>Navigation lifecycle actions for view models.</summary>
    /// <param name="viewModel">The view model to notify when it implements the requested lifecycle.</param>
    extension(IViewModel viewModel)
    {
        /// <summary>Invokes an action when the view model implements the requested lifecycle.</summary>
        /// <typeparam name="T">A type.</typeparam>
        /// <param name="action">An action.</param>
        /// <returns>The object.</returns>
        public IViewModel InvokeViewModelAction<T>(Action<T> action)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(action);

            if (viewModel is T viewModelAsT)
            {
                action(viewModelAsT);
            }

            return viewModel;
        }
    }
}
