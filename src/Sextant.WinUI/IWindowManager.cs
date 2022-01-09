// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.UI.Xaml;

namespace Sextant.WinUI
{
    /// <summary>
    /// A helper class which is used to get the current active window in WinUI 3 app.
    /// The previous API <seealso cref="Window.Current"/> is non-functional (see https://github.com/microsoft/WindowsAppSDK/issues/1776).
    /// </summary>
    public interface IWindowManager
    {
        /// <summary>
        /// Gets or sets the current window of the app.
        /// NOTE: This needs to be set by the executing app as there is no api to determine the current window.
        /// </summary>
        Window? CurrentWindow { get; set; }

        /// <summary>
        /// Gets the handle of <seealso cref="CurrentWindow"/>.
        /// If <seealso cref="CurrentWindow"/> is <c>null</c>, an <seealso cref="ArgumentNullException"/> will be thrown.).
        /// </summary>
        /// <returns>The handle of the current window.</returns>
        /// <exception cref="ArgumentNullException">The <seealso cref="CurrentWindow"/> is null.</exception>
        IntPtr GetHandleOfCurrentWindow();
    }
}
