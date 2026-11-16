// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using NSubstitute;
using ReactiveUI.Testing;
using Sextant.Mocks;

namespace Sextant.Tests;

/// <summary>A fixture for the view stack.</summary>
internal sealed class ParameterViewStackServiceFixture : IBuilder
{
    /// <summary>The navigation view supplied to the service.</summary>
    private IView _view;

    /// <summary>Creates view models for navigation requests.</summary>
    private IViewModelFactory _viewModelFactory;

    /// <summary>Initializes a new instance of the <see cref = "ParameterViewStackServiceFixture"/> class.</summary>
    public ParameterViewStackServiceFixture()
    {
        _view = Substitute.For<IView>();
        _ = _view.PushPage(Arg.Any<INavigable>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(Observable.Return(Unit.Default));
        _ = _view.PopPage().Returns(Observable.Return(Unit.Default));
        _viewModelFactory = Substitute.For<IViewModelFactory>();
        _ = _viewModelFactory.Create(typeof(NavigableViewModelMock), Arg.Any<string>()).Returns(new NavigableViewModelMock());
    }

    public static implicit operator ParameterViewStackService(ParameterViewStackServiceFixture fixture) => fixture.Build();

    /// <summary>Uses the supplied navigation view.</summary>
    /// <param name="view">The navigation dependency or view model.</param>
    /// <returns>The configured fixture or navigation service.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ParameterViewStackServiceFixture WithView(IView view) => this.With(out _view, view);

    /// <summary>Creates a service with the supplied page on its stack.</summary>
    /// <typeparam name="TViewModel">The page view model type.</typeparam>
    /// <param name="viewModel">The navigation dependency or view model.</param>
    /// <returns>The configured fixture or navigation service.</returns>
    internal ParameterViewStackService WithPushed<TViewModel>(TViewModel viewModel)
            where TViewModel : INavigable
    {
        var stack = Build();
        _ = stack.PushPage(viewModel).Subscribe();
        return stack;
    }

    /// <summary>Creates a service with the supplied modal on its stack.</summary>
    /// <typeparam name="TViewModel">The page view model type.</typeparam>
    /// <param name="viewModel">The navigation dependency or view model.</param>
    /// <returns>The configured fixture or navigation service.</returns>
    internal ParameterViewStackService WithModal<TViewModel>(TViewModel viewModel)
            where TViewModel : INavigable
    {
        var stack = Build();
        _ = stack.PushModal(viewModel).Subscribe();
        return stack;
    }

    /// <summary>Uses the supplied view model factory.</summary>
    /// <param name="viewModelFactory">The navigation dependency or view model.</param>
    /// <returns>The configured fixture or navigation service.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ParameterViewStackService WithFactory(IViewModelFactory viewModelFactory) => this.With(out _viewModelFactory, viewModelFactory);

    /// <summary>Creates the navigation service from the configured dependencies.</summary>
    /// <returns>The configured fixture or navigation service.</returns>
    private ParameterViewStackService Build() => new(_view, _viewModelFactory);
}
