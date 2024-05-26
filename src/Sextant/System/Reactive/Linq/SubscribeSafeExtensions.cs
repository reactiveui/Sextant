// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using Splat;

namespace System.Reactive.Linq;

/// <summary>
/// Extension methods that provide additional methods for observables.
/// </summary>
public static class SubscribeSafeExtensions
{
    /// <summary>
    /// Subscribes to an Observable and provides default debugging in the case of an exception.
    /// It will provide the caller information as part of the logging.
    /// </summary>
    /// <typeparam name="T">The type of item signaled as part of the observable.</typeparam>
    /// <param name="observable">The observable to subscribe to.</param>
    /// <param name="callerMemberName">The name of the caller member.</param>
    /// <param name="callerFilePath">The file path of the caller member.</param>
    /// <param name="callerLineNumber">The line number of the caller member.</param>
    /// <returns>A disposable which when disposed will unsubscribe from the observable.</returns>
    public static IDisposable SubscribeSafe<T>(
        this IObservable<T> observable,
        [CallerMemberName]string? callerMemberName = null,
        [CallerFilePath]string? callerFilePath = null,
        [CallerLineNumber]int callerLineNumber = 0) =>
        observable
            .Subscribe(
                _ => { },
                ex =>
                {
                    var logger = new DefaultLogManager().GetLogger(typeof(SubscribeSafeExtensions));
                    logger.Error(ex, "An exception went unhandled. Caller member name: '{0}', caller file path: '{1}', caller line number: {2}.", callerMemberName, callerFilePath, callerLineNumber);

                    Debugger.Break();
                });
}
