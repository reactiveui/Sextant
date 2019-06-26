// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;

namespace Sextant.Tests
{
    /// <summary>
    /// Extension methods to help us with the ViewStackService.
    /// </summary>
    internal static class ViewStackServiceFixtureExtensions
    {
        public static IObservable<Unit> PopModal(this ViewStackService viewStackService, int pages = 1)
        {
            for (var i = 0; i < pages; i++)
            {
                viewStackService.PopModal().Subscribe();
            }

            return Observable.Return(Unit.Default);
        }

        public static IObservable<Unit> PopPage(this ViewStackService viewStackService, int pages = 1)
        {
            for (var i = 0; i < pages; i++)
            {
                viewStackService.PopPage().Subscribe();
            }

            return Observable.Return(Unit.Default);
        }

        public static IObservable<Unit> PushModal(this ViewStackService viewStackService, IViewModel viewModel, string contract = null, int pages = 1)
        {
            for (var i = 0; i < pages; i++)
            {
                viewStackService.PushModal(viewModel, contract).Subscribe();
            }

            return Observable.Return(Unit.Default);
        }

        public static IObservable<Unit> PushPage(this ViewStackService viewStackService, INavigable viewModel, string contract = null, int pages = 1)
        {
            for (var i = 0; i < pages; i++)
            {
                viewStackService.PushPage(viewModel, contract).Subscribe();
            }

            return Observable.Return(Unit.Default);
        }
    }
}
