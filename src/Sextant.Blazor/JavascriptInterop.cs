// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Sextant.Blazor
{
    /// <summary>
    /// Javascript interop methods.
    /// </summary>
    public static class JavascriptInterop
    {
        /// <summary>
        /// Interop method for returning location and state.
        /// </summary>
        /// <param name="sextantNavigationType">Forward, back, or other type of navigation.</param>
        /// <param name="uri">The uri.</param>
        /// <param name="state">The state of the viewmodel.</param>
        /// <returns>A task.</returns>
        [JSInvokable]
        public static Task NotifyLocationState(SextantNavigationType sextantNavigationType, string uri, Dictionary<string, object> state)
        {
            if (state == null)
            {
                try
                {
                    SextantNavigationManager.Instance.NotifyNavigationAction(sextantNavigationType, uri, null);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    SextantNavigationManager.Instance.NotifyNavigationAction(sextantNavigationType, uri, ((JsonElement)state["id"]).GetString());
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }

            return Task.CompletedTask;
        }
    }
}
