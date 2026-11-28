// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace SextantSample.Maui.Views;

/// <summary>Supplies the instructions displayed by the delivery popup.</summary>
/// <param name="Instructions">The note for the driver.</param>
internal sealed record DeliveryNote(string Instructions) : IViewModel
{
    /// <inheritdoc/>
    public string Id => nameof(DeliveryNote);
}
