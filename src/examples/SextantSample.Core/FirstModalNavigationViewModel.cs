// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Sextant;

namespace SextantSample.ViewModels;

/// <summary>Identifies the first modal navigation page.</summary>
/// <param name="viewStackService">The navigation service for this page.</param>
/// <seealso cref="ViewModelBase" />
public class FirstModalNavigationViewModel(IViewStackService viewStackService) : ViewModelBase(viewStackService)
{
    /// <summary>Gets the identifier.</summary>
    /// <value>
    /// The identifier.
    /// </value>
    public override string Id => nameof(FirstModalNavigationViewModel);
}
