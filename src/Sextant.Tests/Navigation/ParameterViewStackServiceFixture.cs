// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Linq;
using NSubstitute;
using ReactiveUI.Testing;
using Sextant.Mocks;

namespace Sextant.Tests
{
    /// <summary>
    /// A fixture for the view stack.
    /// </summary>
    internal class ParameterViewStackServiceFixture : IBuilder
    {
        private IView _view;
        private IViewModelFactory _viewModelFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterViewStackServiceFixture"/> class.
        /// </summary>
        public ParameterViewStackServiceFixture()
        {
            _view = Substitute.For<IView>();
            _view.PushPage(Arg.Any<INavigable>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(Observable.Return(Unit.Default));
            _view.PopPage().Returns(Observable.Return(Unit.Default));
            _viewModelFactory = Substitute.For<IViewModelFactory>();
            _viewModelFactory.Create<NavigableViewModelMock>(Arg.Any<string>()).Returns(new NavigableViewModelMock());
        }

        public static implicit operator ParameterViewStackService(ParameterViewStackServiceFixture fixture) =>
            fixture.Build();

        public ParameterViewStackServiceFixture WithView(IView view) => this.With(ref _view, view);

        public ParameterViewStackService WithPushed<TViewModel>(TViewModel viewModel)
            where TViewModel : INavigable
        {
            var stack = Build();
            stack.PushPage(viewModel).Subscribe();
            return stack;
        }

        public ParameterViewStackService WithModal<TViewModel>(TViewModel viewModel)
            where TViewModel : INavigable
        {
            var stack = Build();
            stack.PushModal(viewModel).Subscribe();
            return stack;
        }

        public ParameterViewStackService WithFactory(IViewModelFactory viewModelFactory) =>
            this.With(ref _viewModelFactory, viewModelFactory);

        private ParameterViewStackService Build() => new(_view, _viewModelFactory);
    }
}
