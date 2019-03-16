// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace System.Reactive.Linq
{
    /// <summary>
    /// Where not null extension.
    /// </summary>
    public static class WhereNotNullExtension
    {
        /// <summary>
        /// Returns the elements of an observable sequence that are not null.
        /// </summary>
        /// <typeparam name="T">The sequence type.</typeparam>
        /// <param name="observable">The observable.</param>
        /// <returns>The sequence.</returns>
        public static IObservable<T> WhereNotNull<T>(this IObservable<T> observable)
        {
            return observable.Where(x => x != null);
        }
    }
}
