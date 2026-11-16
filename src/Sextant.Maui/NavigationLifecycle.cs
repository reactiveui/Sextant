// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive.Disposables;
using System.Reactive.Disposables.Fluent;

namespace Sextant.Maui;

/// <summary>Sends lifecycle notifications after device navigation.</summary>
internal static class NavigationLifecycle
{
    /// <summary>Notifies removed and current pages after a device pop.</summary>
    /// <param name="poppedPage">The removed page.</param>
    /// <param name="currentPage">The remaining page, if any.</param>
    /// <param name="subscriptions">Owns the lifecycle subscriptions.</param>
    internal static void OnPagePopped(Page poppedPage, Page? currentPage, CompositeDisposable subscriptions)
    {
        INavigationParameter navigationParameter = new NavigationParameter();
        if (poppedPage.BindingContext is INavigated previous)
        {
            _ = previous.WhenNavigatedFrom(navigationParameter).Subscribe().DisposeWith(subscriptions);
        }

        if (currentPage?.BindingContext is INavigated current)
        {
            _ = current.WhenNavigatedTo(navigationParameter).Subscribe().DisposeWith(subscriptions);
        }

        if (poppedPage.BindingContext is IDestructible destructible)
        {
            destructible.Destroy();
        }
    }
}
