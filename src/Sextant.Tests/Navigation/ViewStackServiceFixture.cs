// Copyright (c) 2025 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Linq;
using NSubstitute;
using ReactiveUI.Testing;
using Sextant.Mocks;

namespace Sextant.Tests;

/// <summary>
/// A fixture for the view stack.
/// </summary>
internal sealed class ViewStackServiceFixture : IBuilder
{
    private IView _view;
    private IViewModelFactory _viewModelFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewStackServiceFixture"/> class.
    /// </summary>
    public ViewStackServiceFixture()
    {
        _view = Substitute.For<IView>();
        _view.PushPage(Arg.Any<IViewModel>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(Observable.Return(Unit.Default));
        _viewModelFactory = Substitute.For<IViewModelFactory>();
        _viewModelFactory.Create<NavigableViewModelMock>(Arg.Any<string>()).Returns(new NavigableViewModelMock());
    }

    public static implicit operator ViewStackService(ViewStackServiceFixture fixture) => fixture.Build();

    public ViewStackServiceFixture WithView(IView view) => this.With(out _view, view);

    public ViewStackServiceFixture WithFactory(IViewModelFactory viewModelFactory) =>
        this.With(out _viewModelFactory, viewModelFactory);

    private ViewStackService Build() => new(_view, _viewModelFactory);
}
