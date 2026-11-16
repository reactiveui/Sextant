// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Runtime.CompilerServices;
using Splat;

namespace System.Reactive.Linq;

/// <summary>Extension methods that provide additional methods for observables.</summary>
public static class SubscribeSafeExtensions
{
    /// <summary>Extension members for <c>IObservable&lt;T&gt;</c>.</summary>
    /// <typeparam name="T">The observable element type.</typeparam>
    /// <param name="observable">The sequence to subscribe to.</param>
    extension<T>(IObservable<T> observable)
    {
        /// <summary>
        /// Subscribes to an Observable and provides default debugging in the case of an exception.
        /// It will provide the caller information as part of the logging.
        /// </summary>
        /// <param name="callerMemberName">The name of the caller member.</param>
        /// <param name="callerFilePath">The file path of the caller member.</param>
        /// <param name="callerLineNumber">The line number of the caller member.</param>
        /// <returns>A disposable which when disposed will unsubscribe from the observable.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IDisposable SubscribeSafe(
            [CallerMemberName] string? callerMemberName = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int callerLineNumber = 0) =>
            observable
                .Subscribe(
                    static _ => { },
                    ex =>
                    {
                        var logger = new DefaultLogManager(AppLocator.Current).GetLogger(typeof(SubscribeSafeExtensions));
                        logger.Error(
                            ex,
                            "An exception went unhandled. Caller member name: '{0}', caller file path: '{1}', caller line number: {2}.",
                            callerMemberName,
                            callerFilePath,
                            callerLineNumber);

                        Debugger.Break();
                    });
    }
}
