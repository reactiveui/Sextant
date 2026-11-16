// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace SextantSample.ViewModels;

/// <summary>Shares error dialogs between view models and views.</summary>
public static class Interactions
{
    /// <summary>The error message.</summary>
    public static readonly Interaction<Exception, bool> ErrorMessage = new();
}
