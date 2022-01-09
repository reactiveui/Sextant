// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat;

namespace Sextant.WinUI.Mixins
{
    /// <summary>
    /// Extension methods associated with the IMutableDependencyResolver interface.
    /// </summary>
    public static class WindowManagerExtensions
    {
        /// <summary>
        /// Initializes the window manager for Sextant.WinUI.
        /// </summary>
        /// <param name="dependencyResolver">The dependency resolver.</param>
        /// <returns>The dependencyResolver.</returns>
        public static IMutableDependencyResolver RegisterWindowManager(
            this IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant<IWindowManager>(new WindowManager());
            return dependencyResolver;
        }
    }
}
