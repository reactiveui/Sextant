// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Microsoft.Maui.Controls;
namespace Sextant.Maui;

/// <summary>Updates a navigation stack independently of platform event wiring.</summary>
internal static class NavigationOperations
{
    /// <summary>Pushes a page or replaces the current navigation root.</summary>
    /// <param name="navigation">The platform navigation stack.</param>
    /// <param name="page">The page to display.</param>
    /// <param name="resetStack">Whether to replace the stack.</param>
    /// <param name="animate">Whether to animate the push.</param>
    /// <returns>The navigation completion notification.</returns>
    internal static IObservable<Unit> PushPage(INavigation navigation, Page page, bool resetStack, bool animate)
    {
        if (resetStack)
        {
            if (navigation.NavigationStack.Count == 0)
            {
                return navigation.PushAsync(page, false).ToObservable();
            }

            if (navigation.NavigationStack.Count == 1)
            {
                // If there's only the root page, we need to use a different approach.
                // We'll insert the new page before the root, then remove the root.
                navigation.InsertPageBefore(page, navigation.NavigationStack[0]);
                var oldRoot = navigation.NavigationStack[1]; // The old root is now at index 1
                navigation.RemovePage(oldRoot);
                return Observable.Return(Unit.Default);
            }

            // For multiple pages, first pop to root, then replace the root
            return navigation.PopToRootAsync(false).ToObservable().Do(_ =>
            {
                // Now there should only be the root page left
                if (navigation.NavigationStack.Count == 1)
                {
                    navigation.InsertPageBefore(page, navigation.NavigationStack[0]);
                    var oldRoot = navigation.NavigationStack[1];
                    navigation.RemovePage(oldRoot);
                }
            }).Select(static _ => Unit.Default);
        }

        return navigation.PushAsync(page, animate).ToObservable();
    }
}
