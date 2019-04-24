// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using UIKit;

namespace Sextant.IOS.Runner
{
#pragma warning disable SA1649 // File name should match first type name
    /// <summary>
    /// The iOS application.
    /// </summary>
    public class Application
    {
        // This is the main entry point of the application.

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
#pragma warning restore SA1649 // File name should match first type name
