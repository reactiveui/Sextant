// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace System.Reactive.Linq
{
    /// <summary>
    /// Signal extensions.
    /// </summary>
    public static class ToSignalExtension
    {
        /// <summary>
        /// Converts the observable sequence to a signal.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="this">The this.</param>
        /// <returns>A signal.</returns>
        public static IObservable<Unit> ToSignal<T>(this IObservable<T> @this)
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            return @this
                .Select(_ => Unit.Default);
        }
    }
}
