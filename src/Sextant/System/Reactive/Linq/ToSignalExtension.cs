// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace System.Reactive.Linq;

/// <summary>
/// Extension methods for the IObservable.
/// </summary>
public static class ToSignalExtension
{
    /// <summary>
    /// Will convert an observable so that it's value is ignored and converted into just returning <see cref="Unit"/>.
    /// This allows us just to be notified when the observable signals.
    /// </summary>
    /// <typeparam name="T">The current type of the observable.</typeparam>
    /// <param name="observable">The observable to convert.</param>
    /// <returns>The converted observable.</returns>
    [Obsolete("This extension method causes conflicts in the System.Reactive.Linq namespace")]
    public static IObservable<Unit> ToSignal<T>(this IObservable<T> observable)
    {
        if (observable is null)
        {
            throw new ArgumentNullException(nameof(observable));
        }

        return observable.Select(_ => Unit.Default);
    }
}
