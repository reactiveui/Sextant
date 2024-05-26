// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;

namespace Sextant;

/// <summary>
/// An interface that defines methods for when a view mode is navigated to.
/// </summary>
public interface INavigated
{
    /// <summary>
    /// An observable sequence that notifies subscribers this item was navigated to.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    /// <returns>An observable sequence. </returns>
    IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter);

    /// <summary>
    /// An observable sequence that notifies subscribers this item was navigated from.
    /// </summary>
    /// <param name="parameter">The parameter.</param>
    /// <returns>An observable sequence. </returns>
    IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter);
}
