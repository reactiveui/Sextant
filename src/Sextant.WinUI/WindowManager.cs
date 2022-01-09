// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.UI.Xaml;
using WinRT.Interop;

namespace Sextant.WinUI;

/// <summary>
/// <inheritdoc cref="IWindowManager"/>.
/// </summary>
internal class WindowManager : IWindowManager
{
    public WindowManager()
    {
        CurrentWindow = null;
    }

    /// <summary>
    /// Gets or sets the current window of the app.
    /// NOTE: This needs to be set by the executing app as there is no api to determine the current window.
    /// </summary>
    public Window? CurrentWindow { get; set; }

    /// <summary>
    /// Gets the handle of <seealso cref="CurrentWindow"/>.
    /// If <seealso cref="CurrentWindow"/> is <c>null</c>, <seealso cref="IntPtr.Zero"/> will be returned).
    /// </summary>
    /// <returns>The handle of the current window or <seealso cref="IntPtr.Zero"/>.</returns>
    public IntPtr GetHandleOfCurrentWindow()
    {
        Window? currentWindow = CurrentWindow;
        if (currentWindow == null)
        {
            throw new ArgumentNullException(nameof(CurrentWindow), "The current window is null. Please set the current window first.");
        }

        return WindowNative.GetWindowHandle(currentWindow);
    }
}
