﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;

namespace Sextant
{
    /// <summary>
    /// Interface that defines methods for passing parameters on navigation.
    /// </summary>
    public interface IParameterViewStackService
    {
        /// <summary>
        /// Pushes the <see cref="INavigable" /> onto the stack.
        /// </summary>
        /// <param name="navigableViewModel">The navigable view model.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="resetStack">if set to <c>true</c> [reset stack].</param>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns>An observable that signals when the push has been completed.</returns>
        IObservable<Unit> PushPage(INavigable navigableViewModel, INavigationParameter parameter, string contract = null, bool resetStack = false, bool animate = true);

        /// <summary>
        /// Pushes the <see cref="IViewModel" /> onto the stack.
        /// </summary>
        /// <param name="modal">The modal.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>An observable that signals when the push has been completed.</returns>
        IObservable<Unit> PushModal(INavigable modal, INavigationParameter parameter, string contract = null);

        /// <summary>
        /// Pops the <see cref="IViewModel" /> off of the stack.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns>An observable that signals when the push has been completed.</returns>
        IObservable<Unit> PopPage(INavigationParameter parameter, bool animate = true);
    }
}
