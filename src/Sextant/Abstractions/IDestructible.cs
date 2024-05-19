// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant;

/// <summary>
/// Interface representing an object capable of being destroyed.
/// </summary>
public interface IDestructible
{
    /// <summary>
    /// Destroy the destructible object.
    /// </summary>
    public void Destroy();
}
