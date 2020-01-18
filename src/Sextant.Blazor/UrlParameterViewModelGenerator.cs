// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sextant.Blazor
{
    /// <summary>
    /// Generates viewmodels using url parameters.
    /// </summary>
    public class UrlParameterViewModelGenerator
    {
        private Dictionary<Type, Func<Dictionary<string, string>, IViewModel>> _generators = new Dictionary<Type, Func<Dictionary<string, string>, IViewModel>>();

        /// <summary>
        /// Registers the instance generator.
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel Type.</typeparam>
        /// <param name="func">The function that generates the viewmodel.</param>
        public void Register<TViewModel>(Func<Dictionary<string, string>, IViewModel> func)
        {
            _generators.Add(typeof(TViewModel), func);
        }

        /// <summary>
        /// Generates the IViewModel from parameters.
        /// </summary>
        /// <typeparam name="TViewModel">ViewModel type.</typeparam>
        /// <param name="parameters">The url parameters.</param>
        /// <returns>The viewmodel.</returns>
        public IViewModel GetViewModel<TViewModel>(Dictionary<string, string> parameters)
        {
            return _generators[typeof(TViewModel)].Invoke(parameters);
        }

        /// <summary>
        /// Generates the IViewModel from parameters.
        /// </summary>
        /// <param name="viewModelType">ViewModel type.</param>
        /// <param name="parameters">The url parameters.</param>
        /// <returns>The viewmodel.</returns>
        public IViewModel GetViewModel(Type viewModelType, Dictionary<string, string> parameters)
        {
            if (viewModelType == null)
            {
                return null;
            }

            if (_generators.ContainsKey(viewModelType))
            {
                return _generators[viewModelType].Invoke(parameters);
            }

            return null;
        }
    }
}
