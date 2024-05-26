// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using ReactiveUI;
using Sextant;

namespace SextantSample.ViewModels;

/// <summary>
/// ViewModelBase.
/// </summary>
/// <seealso cref="ReactiveObject" />
/// <seealso cref="IViewModel" />
/// <remarks>
/// Initializes a new instance of the <see cref="ViewModelBase"/> class.
/// </remarks>
/// <param name="viewStackService">The view stack service.</param>
public abstract class ViewModelBase(IViewStackService? viewStackService) : ReactiveObject, IViewModel
{
    /// <summary>
    /// Gets the ID of the page.
    /// </summary>
    public virtual string? Id { get; }

    /// <summary>
    /// Gets the view stack service.
    /// </summary>
    protected IViewStackService? ViewStackService { get; } = viewStackService;
}
