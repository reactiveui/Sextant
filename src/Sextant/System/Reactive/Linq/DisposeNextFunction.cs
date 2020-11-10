// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Disposables;

namespace System.Reactive.Linq
{
    internal static class DisposeNextFunction
    {
        /// <summary>
        /// Ensures the provided disposable is disposed with the specified <see cref="SerialDisposable"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the disposable.
        /// </typeparam>
        /// <param name="item">
        /// The disposable we are going to want to be disposed by the CompositeDisposable.
        /// </param>
        /// <param name="serialDisposable">
        /// The <see cref="SerialDisposable"/> to which <paramref name="item"/> will be added.
        /// </param>
        /// <returns>
        /// The disposable.
        /// </returns>
        public static T DisposeNext<T>(this T item, SerialDisposable serialDisposable)
            where T : IDisposable
        {
            if (serialDisposable == null)
            {
                throw new ArgumentNullException(nameof(serialDisposable));
            }

            serialDisposable.Disposable = item;
            return item;
        }
    }
}
