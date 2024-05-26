// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace System.Reactive.Linq;

/// <summary>
/// Adds extension methods to the Observable class.
/// </summary>
public static class WhereNotNullExtension
{
    /// <summary>
    /// Adds a condition to the signaling of an observable which will not fire unless the value is not null.
    /// </summary>
    /// <typeparam name="T">The type of the observable.</typeparam>
    /// <param name="observable">The observable to add the condition to.</param>
    /// <returns>An observable which will not signal unless the value is not null.</returns>
    [Obsolete("This extension method causes conflicts in the System.Reactive.Linq namespace")]
    public static IObservable<T> WhereNotNull<T>(this IObservable<T?> observable) => observable.Where(x => x is not null).Select(x => x!);
}
