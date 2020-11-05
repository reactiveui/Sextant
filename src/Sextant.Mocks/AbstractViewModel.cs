// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Sextant.Mocks
{
    /// <summary>
    /// Abstract view model.
    /// </summary>
    public abstract class AbstractViewModel : INavigable
    {
        /// <inheritdoc/>
        public string Id { get; } = string.Empty;

        /// <inheritdoc/>
        public virtual IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) =>
            Observable.Return(Unit.Default);

        /// <inheritdoc/>
        public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) =>
            Observable.Return(Unit.Default);

        /// <inheritdoc/>
        public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) =>
            Observable.Return(Unit.Default);
    }
}
