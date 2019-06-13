// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using NSubstitute;
using ReactiveUI.Testing;

namespace Sextant.Tests
{
    /// <summary>
    /// A fixture for the view stack.
    /// </summary>
    internal sealed class ViewStackServiceFixture : IBuilder
    {
        private IView _view;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewStackServiceFixture"/> class.
        /// </summary>
        public ViewStackServiceFixture()
        {
            _view = Substitute.For<IView>();
            _view.PushPage(Arg.Any<IViewModel>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(Observable.Return(Unit.Default));
        }

        public static implicit operator ViewStackService(ViewStackServiceFixture fixture) => fixture.Build();

        public ViewStackServiceFixture WithView(IView view) => this.With(ref _view, view);

        public ViewStackService Push<TViewModel>(TViewModel viewModel)
            where TViewModel : IViewModel
        {
            var stack = Build();
            stack.PushPage(viewModel).Subscribe();
            return stack;
        }

        private ViewStackService Build() => new ViewStackService(_view);
    }
}
