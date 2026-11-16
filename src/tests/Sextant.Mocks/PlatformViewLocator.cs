// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Sextant.Mocks;

/// <summary>Resolves platform views from a controllable factory.</summary>
internal sealed class PlatformViewLocator : IViewLocator
{
    /// <summary>Gets or sets the factory used for each lookup.</summary>
    public Func<IViewFor?> Factory { get; set; } = static () => null;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IViewFor? ResolveView<TViewModel>(TViewModel viewModel, string? contract)
        where TViewModel : class => Factory();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IViewFor? ResolveView(object? viewModel, string? contract) => Factory();

    /// <inheritdoc />
    [RequiresDynamicCode("Resolves the runtime view model type.")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IViewFor? ResolveViewUnsafe(object? viewModel, string? contract) => ResolveView(viewModel, contract);
}
