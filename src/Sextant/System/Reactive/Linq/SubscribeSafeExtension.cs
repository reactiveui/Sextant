// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Genesis.Logging;

namespace System.Reactive.Linq
{
    /// <summary>
    /// Subscribe safe extensions method.
    /// </summary>
    public static class SubscribeSafeExtension
    {
        /// <summary>
        /// Subscribes the safely to an observable sequence.
        /// </summary>
        /// <typeparam name="T">The sequence type.</typeparam>
        /// <param name="this">The this.</param>
        /// <param name="callerMemberName">Name of the caller member.</param>
        /// <param name="callerFilePath">The caller file path.</param>
        /// <param name="callerLineNumber">The caller line number.</param>
        /// <returns>A disposable.</returns>
        public static IDisposable SubscribeSafe<T>(
            this IObservable<T> @this,
            [CallerMemberName]string callerMemberName = null,
            [CallerFilePath]string callerFilePath = null,
            [CallerLineNumber]int callerLineNumber = 0)
        {
            return @this
                .Subscribe(
                    _ => { },
                    ex =>
                    {
                        var logger = LoggerService.GetLogger(typeof(SubscribeSafeExtension));
                        logger.Error(ex, "An exception went unhandled. Caller member name: '{0}', caller file path: '{1}', caller line number: {2}.", callerMemberName, callerFilePath, callerLineNumber);

                        Debugger.Break();
                    });
        }
    }
}
