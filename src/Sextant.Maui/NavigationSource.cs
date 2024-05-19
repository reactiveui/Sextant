// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant.Maui;

/// <summary>
/// An enumeration of where navigation signals are sent from.
/// </summary>
public enum NavigationSource
{
    /// <summary>
    /// Navigation sourced from the device.
    /// </summary>
    Device,

    /// <summary>
    /// Navigation sourced from the service.
    /// </summary>
    NavigationService
}
