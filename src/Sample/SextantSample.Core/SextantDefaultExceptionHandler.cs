// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Reactive.Concurrency;

namespace SextantSample.ViewModels;

/// <summary>Reports unhandled command exceptions on the main thread.</summary>
public class SextantDefaultExceptionHandler : IObserver<Exception>
{
    /// <summary>Called when [next].</summary>
    /// <param name="value">The ex.</param>
    public void OnNext(Exception value)
    {
        if (Debugger.IsAttached)
        {
            Debugger.Break();
        }

        _ = RxSchedulers.MainThreadScheduler.Schedule(() => throw value);
    }

    /// <summary>Called when [error].</summary>
    /// <param name="error">The ex.</param>
    public void OnError(Exception error)
    {
        if (Debugger.IsAttached)
        {
            Debugger.Break();
        }

        OnNext(error);
    }

    /// <summary>Notifies the observer that the provider has finished sending push-based notifications.</summary>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public void OnCompleted() => Debug.WriteLine("Exception stream completed.");
}
