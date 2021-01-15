// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace Sextant.XamForms
{
    /// <summary>
    /// Represents and abstract <see cref="Behavior{T}"/>.
    /// </summary>
    /// <typeparam name="T">The bindable object type.
    /// </typeparam>
    public abstract class BehaviorBase<T> : Behavior<T>, IDisposable
        where T : BindableObject
    {
        /// <summary>
        /// Gets the disposables for this behavior.
        /// </summary>
        protected CompositeDisposable BehaviorDisposable { get; } = new();

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);
            Observable.FromEvent<EventHandler, EventArgs>(
                    handler =>
                    {
                        void Handler(object sender, EventArgs args) => handler(args);
                        return Handler;
                    },
                    x => bindable.BindingContextChanged += x,
                    x => bindable.BindingContextChanged -= x)
                .Subscribe(_ => BindingContext = bindable.BindingContext)
                .DisposeWith(BehaviorDisposable);
        }

        /// <inheritdoc/>
        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);
            Dispose();
        }

        /// <summary>
        /// Disposes of resources.
        /// </summary>
        /// <param name="disposing">A value indicating where this instance is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                BehaviorDisposable.Dispose();
            }
        }
    }
}
