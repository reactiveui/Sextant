// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Sextant;

/// <summary>
/// Class of extension method for object life cycle in Sextant.
/// </summary>
public static class ViewModelActionExtensions
{
    /// <summary>
    /// This is a thing I lifted from Prism.
    /// </summary>
    /// <param name="viewModel">The view model.</param>
    /// <param name="action">An action.</param>
    /// <typeparam name="T">A type.</typeparam>
    /// <returns>The object.</returns>
    public static object InvokeViewModelAction<T>(this object viewModel, Action<T> action)
        where T : class
    {
        if (action is null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        if (viewModel is T viewModelAsT)
        {
            action(viewModelAsT);
        }

        return viewModel;
    }
}
