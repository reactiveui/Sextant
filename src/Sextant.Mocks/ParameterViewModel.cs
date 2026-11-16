// Copyright (c) 2019-2026 ReactiveUI and Contributors. All rights reserved.
// ReactiveUI and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Sextant.Mocks;

/// <summary>View model to test passing parameters.</summary>
/// <seealso cref="IViewModel" />
[System.Diagnostics.DebuggerDisplay("ParameterViewModel: {Id}")]
public sealed class ParameterViewModel : ReactiveObject, INavigable, IDestructible, IDisposable
{
    /// <inheritdoc />
    public string Id { get; } = nameof(ParameterViewModel);

    /// <summary>Gets or sets the text.</summary>
    public string? Text { get => field; set => this.RaiseAndSetIfChanged(ref field, value); }

    /// <summary>Gets or sets the meaning.</summary>
    public int Meaning { get => field; set => this.RaiseAndSetIfChanged(ref field, value); }

    /// <summary>Gets the disposable.</summary>
    public CompositeDisposable Disposable { get; } = new();

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) => Observable.Return(Unit.Default).Do(_ => Unwrap(parameter));

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) => WhenNavigatedTo(parameter);

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) => WhenNavigatedFrom(parameter);

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Destroy() => Disposable.Dispose();

    /// <inheritdoc/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose() => Destroy();

    /// <summary>Reads the greeting and meaning supplied by the navigation caller.</summary>
    /// <param name="parameter">The navigation payload containing hello and life entries.</param>
    private void Unwrap(INavigationParameter parameter)
    {
        // Note: normally you should check parameter.ContainsKey() before accessing the dictionary.
        Text = (string)parameter["hello"];
        Meaning = (int)parameter["life"];
    }
}
