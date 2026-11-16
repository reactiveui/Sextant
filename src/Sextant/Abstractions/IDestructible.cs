// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant;

/// <summary>Interface representing an object capable of being destroyed.</summary>
public interface IDestructible
{
    /// <summary>Destroy the destructible object.</summary>
    void Destroy();
}
