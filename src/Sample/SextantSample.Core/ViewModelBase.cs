// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Sextant;

namespace SextantSample.ViewModels;

/// <summary>Shares the navigation service between sample pages.</summary>
/// <param name="viewStackService">The view stack service.</param>
/// <remarks>
/// Initializes a new instance of the <see cref="ViewModelBase"/> class.
/// </remarks>
/// <seealso cref="ReactiveObject" />
/// <seealso cref="IViewModel" />
[System.Diagnostics.DebuggerDisplay("ViewModelBase: {Id}")]
public class ViewModelBase(IViewStackService? viewStackService) : ReactiveObject, IViewModel
{
    /// <summary>Gets the ID of the page.</summary>
    public virtual string? Id { get; }

    /// <summary>Gets the view stack service.</summary>
    protected IViewStackService? ViewStackService { get; } = viewStackService;
}
