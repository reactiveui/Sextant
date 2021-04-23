// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

using Avalonia.Animation;
using Avalonia.ReactiveUI;

using DynamicData.Aggregation;
using DynamicData.Binding;

using ReactiveUI;

namespace Sextant.Avalonia
{
    /// <summary>
    /// A view used within navigation in Sextant with Avalonia.
    /// </summary>
    public sealed partial class NavigationView
    {
        /// <summary>
        /// This class represents a single navigation layer.
        /// </summary>
        private class Navigation
        {
            private readonly ObservableCollection<IViewFor> _navigationStack = new ObservableCollection<IViewFor>();
            private readonly IPageTransition? _transition;

            /// <summary>
            /// Initializes a new instance of the <see cref="Navigation"/> class.
            /// </summary>
            public Navigation()
            {
                var navigationStackObservable = _navigationStack
                    .ToObservableChangeSet()
                    .Publish()
                    .RefCount();

                navigationStackObservable
                    .Select(_ => _navigationStack.LastOrDefault())
                    .Subscribe(page => Control.Content = page);

                CountChanged = navigationStackObservable.Count().AsObservable();
                _transition = Control.PageTransition;
            }

            /// <summary>
            /// Gets a value indicating whether the control is visible.
            /// </summary>
            public bool IsVisible => _navigationStack.Count > 1;

            /// <summary>
            /// Gets a the page count.
            /// </summary>
            public IObservable<int> CountChanged { get; }

            /// <summary>
            /// Gets the control responsible for rendering the current view.
            /// </summary>
            public TransitioningContentControl Control { get; } = new TransitioningContentControl();

            /// <summary>
            /// Toggles the animations.
            /// </summary>
            /// <param name="enable">Returns true if we are enabling the animations.</param>
            public void ToggleAnimations(bool enable) => Control.PageTransition = enable ? _transition : null;

            /// <summary>
            /// Adds a <see cref="IViewFor"/> to the navigation stack.
            /// </summary>
            /// <param name="view">The view to add to the navigation stack.</param>
            /// <param name="resetStack">Defines if we should reset the navigation stack.</param>
            public void Push(IViewFor view, bool resetStack = false)
            {
                if (resetStack)
                {
                    _navigationStack.Clear();
                }

                _navigationStack.Add(view);
            }

            /// <summary>
            /// Removes the last <see cref="IViewFor"/> from the navigation stack.
            /// </summary>
            public void Pop()
            {
                var indexToRemove = _navigationStack.Count - 1;
                _navigationStack.RemoveAt(indexToRemove);
            }

            /// <summary>
            /// Removes all pages except the first one <see cref="IViewFor"/> from the navigation stack.
            /// </summary>
            public void PopToRoot()
            {
                var view = _navigationStack[0];
                _navigationStack.Clear();
                _navigationStack.Add(view);
            }
        }
    }
}
