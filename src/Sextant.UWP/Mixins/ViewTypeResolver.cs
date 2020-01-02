// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Sextant.UWP
{
    /// <summary>
    /// Special resolver for UWP that only spits out view Type.
    /// </summary>
    public class ViewTypeResolver
    {
        private Dictionary<(string vmTypeName, string contract), Type> _typeDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewTypeResolver"/> class.
        /// </summary>
        public ViewTypeResolver()
        {
            _typeDictionary = new Dictionary<(string vmTypeName, string contract), Type>();
        }

        /// <summary>
        /// Register view Type with viewmodel Type.
        /// </summary>
        /// <typeparam name="TView">View Type.</typeparam>
        /// <typeparam name="TViewModel">ViewModel Type.</typeparam>
        /// <param name="contract">The contract.</param>
        public void Register<TView, TViewModel>(string contract = null)
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
        public Type ResolveViewType<TViewModel>(string contract = null)
            where TViewModel : class
        {
            if (_typeDictionary.ContainsKey((typeof(TViewModel).AssemblyQualifiedName, contract)))
            {
                return _typeDictionary[(typeof(TViewModel).AssemblyQualifiedName, contract)];
            }

            return null;
        }

        /// <summary>
        /// Method to get view type for viewmodel.
        /// </summary>
        /// <param name="viewModelType">The viewmodel Type.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>The view Type again.</returns>
        public Type ResolveViewType(Type viewModelType, string contract = null)
        {
            if (_typeDictionary.ContainsKey((viewModelType.AssemblyQualifiedName, contract)))
            {
                return _typeDictionary[(viewModelType.AssemblyQualifiedName, contract)];
            }

            return null;
        }
    }
}
