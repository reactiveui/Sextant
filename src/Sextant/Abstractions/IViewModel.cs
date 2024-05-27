// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant;

/// <summary>
/// Interface representing a Sextant view model.
/// </summary>
public interface IViewModel
{
    /// <summary>
    /// Gets the ID of the page.
    /// </summary>
    string? Id { get; }
}
