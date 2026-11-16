// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive;
using System.Runtime.CompilerServices;
using Sextant.Plugins.Popup;
namespace Sextant.Tests.Navigation;

/// <summary>Uses popup interface defaults while delegating complete operations.</summary>
internal sealed class DefaultPopupNavigationService : DefaultNavigationService, IPopupViewStackService
{
    /// <summary>The popup service.</summary>
    private readonly PopupViewStackService _service;

    /// <summary>Initializes a new instance of the <see cref="DefaultPopupNavigationService"/> class.</summary>
    /// <param name="service">The navigation service.</param>
    internal DefaultPopupNavigationService(PopupViewStackService service)
        : base(service)
    {
        _service = service;
    }

    /// <inheritdoc />
    public IObservable<PopupNavigationEvent> Pushing => _service.Pushing;

    /// <inheritdoc />
    public IObservable<PopupNavigationEvent> Pushed => _service.Pushed;

    /// <inheritdoc />
    public IObservable<PopupNavigationEvent> Popping => _service.Popping;

    /// <inheritdoc />
    public IObservable<PopupNavigationEvent> Popped => _service.Popped;

    /// <inheritdoc />
    public IReadOnlyList<IViewModel> PopupStack => _service.PopupStack;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup(IViewModel viewModel, string? contract, bool animate) =>
        _service.PushPopup(viewModel, contract, animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup(Type viewModelType, string? contract, bool animate) =>
        _service.PushPopup(viewModelType, contract, animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup(INavigable viewModel, INavigationParameter navigationParameter, string? contract, bool animate) =>
        _service.PushPopup(viewModel, navigationParameter, contract, animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPopup(Type viewModelType, INavigationParameter navigationParameter, string? contract, bool animate) =>
        _service.PushPopup(viewModelType, navigationParameter, contract, animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopPopup(bool animate) =>
        _service.PopPopup(animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopAllPopups(bool animate) =>
        _service.PopAllPopups(animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> RemovePopup(IViewModel viewModel, string? contract, bool animate) =>
        _service.RemovePopup(viewModel, contract, animate);
}
