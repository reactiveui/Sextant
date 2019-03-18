// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Linq;
using NSubstitute;

namespace Sextant.Tests.Navigation
{
    /// <summary>
    /// A fixture for the view stack.
    /// </summary>
    internal sealed class ViewStackServiceFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStackServiceFixture"/> class.
        /// </summary>
        public ViewStackServiceFixture()
        {
            View = Substitute.For<IView>();
            View.PushPage(Arg.Any<IPageViewModel>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(Observable.Return(Unit.Default));
            ViewStackService = new ViewStackService(View);
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        public IView View { get; }

        /// <summary>
        /// Gets the view stack service.
        /// </summary>
        public IViewStackService ViewStackService { get; }
    }
}
