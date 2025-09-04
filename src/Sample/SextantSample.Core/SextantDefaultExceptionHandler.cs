// Copyright (c) 2025 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using ReactiveUI;

namespace SextantSample.ViewModels;

/// <summary>
/// SextantDefaultExceptionHandler.
/// </summary>
public class SextantDefaultExceptionHandler : IObserver<Exception>
{
    /// <summary>
    /// Called when [next].
    /// </summary>
    /// <param name="value">The ex.</param>
    public void OnNext(Exception value)
    {
        if (Debugger.IsAttached)
        {
            Debugger.Break();
        }

        RxApp.MainThreadScheduler.Schedule(() => throw value);
    }

    /// <summary>
    /// Called when [error].
    /// </summary>
    /// <param name="error">The ex.</param>
    public void OnError(Exception error)
    {
        if (Debugger.IsAttached)
        {
            Debugger.Break();
        }

        RxApp.MainThreadScheduler.Schedule(() => throw error);
    }

    /// <summary>
    /// Notifies the observer that the provider has finished sending push-based notifications.
    /// </summary>
    public void OnCompleted()
    {
        if (Debugger.IsAttached)
        {
            Debugger.Break();
        }

        RxApp.MainThreadScheduler.Schedule(() => throw new NotImplementedException());
    }
}
