// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using ReactiveUI;

namespace Sextant.Mocks
{
    /// <summary>
    /// View model to test passing parameters.
    /// </summary>
    /// <seealso cref="IViewModel" />
    public class ParameterViewModel : ReactiveObject, INavigable, IDestructible
    {
        private string? _text;
        private int _meaning;

        /// <inheritdoc />
        public string Id { get; } = nameof(ParameterViewModel);

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string? Text { get => _text; set => this.RaiseAndSetIfChanged(ref _text, value); }

        /// <summary>
        /// Gets or sets the meaning.
        /// </summary>
        public int Meaning { get => _meaning; set => this.RaiseAndSetIfChanged(ref _meaning, value); }

        /// <summary>
        /// Gets the disposable.
        /// </summary>
        public CompositeDisposable Disposable { get; } = new CompositeDisposable();

        /// <inheritdoc />
        public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) => Observable.Return(Unit.Default).Do(_ => Unwrap(parameter));

        /// <inheritdoc />
        public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) => Observable.Return(Unit.Default).Do(_ => Unwrap(parameter));

        /// <inheritdoc />
        public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) => Observable.Return(Unit.Default).Do(_ => Unwrap(parameter));

        /// <inheritdoc/>
        public void Destroy()
        {
            Disposable?.Dispose();
        }

        private void Unwrap(INavigationParameter parameter)
        {
            // Note: normally you should check parameter.ContainsKey() before accessing the dictionary.
            Text = (string)parameter["hello"];
            Meaning = (int)parameter["life"];
        }
    }
}
