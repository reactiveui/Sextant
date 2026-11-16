// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using NSubstitute;
using ReactiveUI.Testing;
using Sextant.Mocks;

namespace Sextant.Tests;

/// <summary>A fixture for the view stack.</summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "CA1812:Avoid uninstantiated internal classes", Justification = "Used by test framework")]
internal sealed class ViewStackServiceFixture : IBuilder
{
    /// <summary>The navigation view supplied to the service.</summary>
    private IView _view;

    /// <summary>Creates view models for navigation requests.</summary>
    private IViewModelFactory _viewModelFactory;

    /// <summary>Initializes a new instance of the <see cref = "ViewStackServiceFixture"/> class.</summary>
    public ViewStackServiceFixture()
    {
        _view = Substitute.For<IView>();
        _ = _view.PushPage(Arg.Any<IViewModel>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(Observable.Return(Unit.Default));
        _viewModelFactory = Substitute.For<IViewModelFactory>();
        _ = _viewModelFactory.Create(typeof(NavigableViewModelMock), Arg.Any<string>()).Returns(new NavigableViewModelMock());
    }

    public static implicit operator ViewStackService(ViewStackServiceFixture fixture) => fixture.Build();

    /// <summary>Uses the supplied navigation view.</summary>
    /// <param name="view">The navigation dependency or view model.</param>
    /// <returns>The configured fixture or navigation service.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ViewStackServiceFixture WithView(IView view) => this.With(out _view, view);

    /// <summary>Uses the supplied view model factory.</summary>
    /// <param name="viewModelFactory">The navigation dependency or view model.</param>
    /// <returns>The configured fixture or navigation service.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ViewStackServiceFixture WithFactory(IViewModelFactory viewModelFactory) => this.With(out _viewModelFactory, viewModelFactory);

    /// <summary>Creates the navigation service from the configured dependencies.</summary>
    /// <returns>The configured fixture or navigation service.</returns>
    private ViewStackService Build() => new(_view, _viewModelFactory);
}
