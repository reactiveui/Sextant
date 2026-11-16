// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Splat;

namespace Sextant;

/// <summary>Registers navigation when a mutable resolver becomes available.</summary>
internal static class NavigationRegistration
{
    /// <summary>Registers navigation services for a mutable resolver.</summary>
    /// <param name="resolver">The new resolver, or null when no mutable resolver is available.</param>
    internal static void Initialize(IMutableDependencyResolver? resolver)
    {
        if (resolver is null)
        {
            return;
        }

        _ = resolver.RegisterViewStackService();
    }
}
