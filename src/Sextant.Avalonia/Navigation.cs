// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Animation;
using Avalonia.Controls;

namespace Sextant.Avalonia;

/// <summary>A view used within navigation in Sextant with Avalonia.</summary>
[System.Diagnostics.DebuggerDisplay("NavigationView: {ToString(),nq}")]
public sealed partial class NavigationView
{
    /// <summary>This class represents a single navigation layer.</summary>
    internal sealed class Navigation
    {
        /// <summary>The ordered views in this navigation layer.</summary>
        private readonly ObservableCollection<IViewFor> _navigationStack = [];

        /// <summary>The transition restored when animations are enabled.</summary>
        private readonly IPageTransition? _transition;

        /// <summary>Initializes a new instance of the <see cref = "Navigation"/> class.</summary>
        internal Navigation()
        {
            CountChanged = Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                handler => _navigationStack.CollectionChanged += handler,
                handler => _navigationStack.CollectionChanged -= handler)
                .Select(_ => _navigationStack.Count).StartWith(0);
            _transition = Control.PageTransition;
        }

        /// <summary>Gets a value indicating whether the control is visible.</summary>
        internal bool IsVisible => _navigationStack.Count > 1;

        /// <summary>Gets a the page count.</summary>
        internal IObservable<int> CountChanged { get; }

        /// <summary>Gets the control responsible for rendering the current view.</summary>
        internal TransitioningContentControl Control { get; } = new();

        /// <summary>Toggles the animations.</summary>
        /// <param name = "enable">Returns true if we are enabling the animations.</param>
        internal void ToggleAnimations(bool enable) => Control.PageTransition = enable ? _transition : null;

        /// <summary>Adds a view after the current page.</summary>
        /// <param name="view">The view to display.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Push(IViewFor view) => Push(view, false);

        /// <summary>Adds a <see cref = "IViewFor"/> to the navigation stack.</summary>
        /// <param name = "view">The view to add to the navigation stack.</param>
        /// <param name = "resetStack">Defines if we should reset the navigation stack.</param>
        internal void Push(IViewFor view, bool resetStack)
        {
            if (resetStack)
            {
                _navigationStack.Clear();
            }

            _navigationStack.Add(view);
            Control.Content = view;
        }

        /// <summary>Removes the last <see cref = "IViewFor"/> from the navigation stack.</summary>
        internal void Pop()
        {
            _navigationStack.RemoveAt(_navigationStack.Count - 1);
            Control.Content = _navigationStack.Count == 0 ? null : _navigationStack[_navigationStack.Count - 1];
        }

        /// <summary>Removes all pages except the first one <see cref = "IViewFor"/> from the navigation stack.</summary>
        internal void PopToRoot()
        {
            var view = _navigationStack[0];
            _navigationStack.Clear();
            _navigationStack.Add(view);
            Control.Content = view;
        }
    }
}
