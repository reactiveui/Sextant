// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using ReactiveUI;

namespace Sextant
{
    /// <summary>
    /// Special resolver for UWP that only spits out view Type.
    /// </summary>
    public class ViewTypeResolver
    {
        private readonly Dictionary<(string VmTypeName, string? Contract), Type> _typeDictionary = new();

        /// <summary>
        /// Register view Type with viewmodel Type.
        /// </summary>
        /// <typeparam name="TView">View Type.</typeparam>
        /// <typeparam name="TViewModel">ViewModel Type.</typeparam>
        /// <param name="contract">The contract.</param>
        public void Register<TView, TViewModel>(string? contract = null)
            where TView : IViewFor<TViewModel>
            where TViewModel : class, IViewModel
        {
            if (_typeDictionary.ContainsKey((typeof(TViewModel).AssemblyQualifiedName, contract)))
            {
                throw new Exception("Type already registered.");
            }

            _typeDictionary.Add((typeof(TViewModel).AssemblyQualifiedName, contract), typeof(TView));
        }

        /// <summary>
        /// Method to get view type for viewmodel.
        /// </summary>
        /// <typeparam name="TViewModel">The viewmodel Type.</typeparam>
        /// <param name="contract">The contract.</param>
        /// <returns>The view Type again.</returns>
        public Type? ResolveViewType<TViewModel>(string? contract = null)
            where TViewModel : class
        {
            _typeDictionary.TryGetValue((typeof(TViewModel).AssemblyQualifiedName, contract), out var value);

            return value;
        }

        /// <summary>
        /// Method to get view type for viewmodel.
        /// </summary>
        /// <param name="viewModelType">The viewmodel Type.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>The view Type again.</returns>
        public Type? ResolveViewType(Type viewModelType, string? contract = null)
        {
            if (viewModelType is null)
            {
                throw new ArgumentNullException(nameof(viewModelType));
            }

            _typeDictionary.TryGetValue((viewModelType.AssemblyQualifiedName, contract), out var value);

            return value;
        }
    }
}
