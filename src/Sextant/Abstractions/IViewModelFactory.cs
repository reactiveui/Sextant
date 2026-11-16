// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
namespace Sextant;

/// <summary>Interface that represents a view model factory.</summary>
public interface IViewModelFactory
{
    /// <summary>Resolves a registered view model by type and optional contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <returns>The registered view model.</returns>
    IViewModel Create(Type viewModelType) => Create(viewModelType, null);

    /// <summary>Resolves a registered view model by type and optional contract.</summary>
    /// <param name="viewModelType">The registered view model type to resolve.</param>
    /// <param name="contract">The registration contract, or null for the default registration.</param>
    /// <returns>The registered view model.</returns>
    IViewModel Create(Type viewModelType, string? contract);

    /// <summary>Resolves the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <returns>The registered view model.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    TViewModel Create<TViewModel>()
        where TViewModel : IViewModel => (TViewModel)Create(typeof(TViewModel));

    /// <summary>Resolves the registered view model.</summary>
    /// <typeparam name = "TViewModel">The registered view model type.</typeparam>
    /// <param name = "contract">The registration contract, or null for the default registration.</param>
    /// <returns>The registered view model.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "SST2307", Justification = "Callers explicitly select the view model type; generic constraints validate it at compile time.")]
    TViewModel Create<TViewModel>(string? contract)
        where TViewModel : IViewModel => (TViewModel)Create(typeof(TViewModel), contract);
}
