// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Runtime.CompilerServices;
using Splat;

namespace Sextant;

/// <summary>Default View Model Factory.</summary>
public class DefaultViewModelFactory : IViewModelFactory
{
    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IViewModel Create(Type viewModelType) => Create(viewModelType, null);

    /// <inheritdoc/>
    public IViewModel Create(Type viewModelType, string? contract)
    {
        ArgumentNullException.ThrowIfNull(viewModelType);
        var viewModel = (IViewModel?)AppLocator.Current.GetService(viewModelType, contract);
        return viewModel switch
        {
            null => throw new InvalidOperationException($"ViewModel of type {viewModelType.Name} {contract} not registered."),
            _ => viewModel
        };
    }
}
