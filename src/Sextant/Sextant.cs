// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Splat;

namespace Sextant;

/// <summary>The main registration point for Sextant.</summary>
[System.Diagnostics.DebuggerDisplay("Sextant: {MutableLocator}")]
public class Sextant
{
    /// <summary>The shared registration point.</summary>
    private static readonly Lazy<Sextant> _sextant = new();

    /// <summary>Registers the dependency resolver callback.</summary>
    static Sextant() =>
        AppLocator.RegisterResolverCallbackChanged(static () => NavigationRegistration.Initialize(AppLocator.CurrentMutable));

    /// <summary>Gets the instance of <see cref="Sextant"/>.</summary>
    public static Sextant Instance => _sextant.Value;

    /// <summary>Gets the mutable dependency resolver.</summary>
    [SuppressMessage("Design", "CA1822: Implement statically", Justification = "Existing API.")]
    public IMutableDependencyResolver MutableLocator => AppLocator.CurrentMutable;
}
