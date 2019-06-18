﻿using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using ReactiveUI;

namespace Sextant.Mocks
{
    /// <summary>
    /// View model to test passing paramaters.
    /// </summary>
    /// <seealso cref="IViewModel" />
    public class ParameterViewModel : ReactiveObject, INavigable
    {
        private string _text;
        private int _meaning;

        /// <inheritdoc />
        public string Id { get; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        public string Text { get => _text; set => this.RaiseAndSetIfChanged(ref _text, value); }

        /// <summary>
        /// Gets or sets the meaning.
        /// </summary>
        public int Meaning { get => _meaning; set => this.RaiseAndSetIfChanged(ref _meaning, value); }

        /// <inheritdoc />
        public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) => Observable.Return(Unit.Default).Do(_ => Unwrap(parameter));

        /// <inheritdoc />
        public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) => Observable.Return(Unit.Default).Do(_ => Unwrap(parameter));

        /// <inheritdoc />
        public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) => Observable.Return(Unit.Default).Do(_ => Unwrap(parameter));

        private void Unwrap(INavigationParameter parameter)
        {
            Text = (string)parameter["hello"];
            Meaning = (int)parameter["life"];
        }
    }
}
