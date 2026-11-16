// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;

namespace Sextant.Mocks;

/// <summary>Resolves a test-owned view model without a global service locator.</summary>
/// <param name="model">The view model returned by the factory.</param>
internal sealed class ViewModelFactoryMock(IViewModel? model = null) : IViewModelFactory
{
    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IViewModel Create(Type viewModelType, string? contract) => model ?? throw new InvalidOperationException("No view model was supplied to the test factory.");
}
