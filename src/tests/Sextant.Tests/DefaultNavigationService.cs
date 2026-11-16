// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Immutable;
using System.Reactive;
using System.Runtime.CompilerServices;
namespace Sextant.Tests;

/// <summary>Uses the interface defaults while delegating complete operations to a real service.</summary>
/// <param name="service">The service receiving complete navigation operations.</param>
internal class DefaultNavigationService(ParameterViewStackServiceBase service) : IParameterViewStackService
{
    /// <inheritdoc />
    public IObservable<IImmutableList<IViewModel>> ModalStack => service.ModalStack;

    /// <inheritdoc />
    public IObservable<IImmutableList<IViewModel>> PageStack => service.PageStack;

    /// <inheritdoc />
    public IView View => service.View;

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopModal(bool animate) =>
        service.PopModal(animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopPage(bool animate) =>
        service.PopPage(animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopPage(INavigationParameter parameter, bool animate) =>
        service.PopPage(parameter, animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PopToRootPage(bool animate) =>
        service.PopToRootPage(animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(IViewModel modal, string? contract, bool withNavigationPage) =>
        service.PushModal(modal, contract, withNavigationPage);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(Type viewModelType, string? contract, bool withNavigationPage) =>
        service.PushModal(viewModelType, contract, withNavigationPage);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(INavigable navigableModal, INavigationParameter parameter, string? contract, bool withNavigationPage) =>
        service.PushModal(navigableModal, parameter, contract, withNavigationPage);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushModal(Type viewModelType, INavigationParameter parameter, string? contract, bool withNavigationPage) =>
        service.PushModal(viewModelType, parameter, contract, withNavigationPage);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(Type viewModelType, string? contract, bool resetStack, bool animate) =>
        service.PushPage(viewModelType, contract, resetStack, animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(IViewModel page, string? contract, bool resetStack, bool animate) =>
        service.PushPage(page, contract, resetStack, animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(INavigable page, string? contract, bool resetStack, bool animate) =>
        service.PushPage(page, contract, resetStack, animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(INavigable navigableViewModel, INavigationParameter parameter, string? contract, bool resetStack, bool animate) =>
        service.PushPage(navigableViewModel, parameter, contract, resetStack, animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> PushPage(Type viewModelType, INavigationParameter parameter, string? contract, bool resetStack, bool animate) =>
        service.PushPage(viewModelType, parameter, contract, resetStack, animate);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<IViewModel> TopPage() =>
        service.TopPage();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<IViewModel> TopModal() =>
        service.TopModal();
}
