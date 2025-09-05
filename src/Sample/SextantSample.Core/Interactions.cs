// Copyright (c) 2025 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using ReactiveUI;

namespace SextantSample.ViewModels;

/// <summary>
/// Interactions.
/// </summary>
public static class Interactions
{
    /// <summary>
    /// The error message.
    /// </summary>
    public static readonly Interaction<Exception, bool> ErrorMessage = new();
}
