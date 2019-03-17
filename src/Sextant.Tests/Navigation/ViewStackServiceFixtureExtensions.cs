// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using Sextant.Abstraction;

namespace Sextant.Tests.Navigation
{
    /// <summary>
    /// Extension methods to help us with the ViewStackService.
    /// </summary>
    internal static class ViewStackServiceFixtureExtensions
    {
        public static IObservable<Unit> PopModal(this ViewStackServiceFixture viewStackServiceFixture, int pages = 1)
        {
            for (var i = 0; i < pages; i++)
            {
                viewStackServiceFixture.ViewStackService.PopModal().Subscribe();
            }

            return Observable.Return(Unit.Default);
        }

        public static IObservable<Unit> PopPage(this ViewStackServiceFixture viewStackServiceFixture, int pages = 1)
        {
            for (var i = 0; i < pages; i++)
            {
                viewStackServiceFixture.ViewStackService.PopPage().Subscribe();
            }

            return Observable.Return(Unit.Default);
        }

        public static IObservable<Unit> PushModal(this ViewStackServiceFixture viewStackServiceFixture, IPageViewModel viewModel, string contract = null, int pages = 1)
        {
            for (var i = 0; i < pages; i++)
            {
                viewStackServiceFixture.ViewStackService.PushModal(viewModel, contract).Subscribe();
            }

            return Observable.Return(Unit.Default);
        }

        public static IObservable<Unit> PushPage(this ViewStackServiceFixture viewStackServiceFixture, IPageViewModel viewModel, string contract = null, int pages = 1)
        {
            for (var i = 0; i < pages; i++)
            {
                viewStackServiceFixture.ViewStackService.PushPage(viewModel, contract).Subscribe();
            }

            return Observable.Return(Unit.Default);
        }
    }
}
