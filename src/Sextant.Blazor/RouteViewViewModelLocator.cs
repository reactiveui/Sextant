// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Sextant.Blazor
{
    /// <summary>
    /// Special resolver for Blazor that stores view, viewmodel, and relative url info.
    /// </summary>
    public class RouteViewViewModelLocator
    {
        /// <summary>
        /// Used to resolve View Type from ViewModel Type string and contract.
        /// </summary>
        private Dictionary<(string vmTypeName, string contract), Type> _viewModelToViewTypeDictionary;

        /// <summary>
        /// Used to resolve ViewModel Type from View Type string.
        /// </summary>
        private Dictionary<string, Type> _viewToViewModelTypeDictionary;

        /// <summary>
        /// Used to resolve ViewModel Type from a url route.
        /// This is Blazor specific as a user can just type in a URL directly (or a bookmark).
        /// </summary>
        private Dictionary<string, Type> _routeToViewModelTypeDictionary;

        /// <summary>
        /// Used to resolve a url route for a ViewModel Type string.
        /// This is Blazor specific as a user can just type in a URL directly (or a bookmark).
        /// </summary>
        private Dictionary<string, string> _viewModelTypeToRouteDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteViewViewModelLocator"/> class.
        /// </summary>
        public RouteViewViewModelLocator()
        {
            _viewModelToViewTypeDictionary = new Dictionary<(string vmTypeName, string contract), Type>();
            _viewToViewModelTypeDictionary = new Dictionary<string, Type>();

            _routeToViewModelTypeDictionary = new Dictionary<string, Type>();
            _viewModelTypeToRouteDictionary = new Dictionary<string, string>();
        }

        /// <summary>
        /// Register view Type with viewmodel Type.
        /// </summary>
        /// <typeparam name="TView">View Type.</typeparam>
        /// <typeparam name="TViewModel">ViewModel Type.</typeparam>
        /// <param name="route">The route (relative url segment).</param>
        /// <param name="contract">The contract.</param>
        public void Register<TView, TViewModel>(string route, string contract = null)
            where TView : IComponent
            where TViewModel : class, IViewModel
        {
            if (_viewModelToViewTypeDictionary.ContainsKey((typeof(TViewModel).AssemblyQualifiedName, contract)))
            {
                throw new Exception("Type already registered.");
            }

            _viewModelToViewTypeDictionary.Add((typeof(TViewModel).AssemblyQualifiedName, contract), typeof(TView));

            if (!_routeToViewModelTypeDictionary.ContainsKey(route))
            {
                _routeToViewModelTypeDictionary.Add(route, typeof(TViewModel));
            }

            if (!_viewModelTypeToRouteDictionary.ContainsKey(typeof(TViewModel).AssemblyQualifiedName))
            {
                _viewModelTypeToRouteDictionary.Add(typeof(TViewModel).AssemblyQualifiedName, route);
            }

            if (!_viewToViewModelTypeDictionary.ContainsKey(typeof(TView).AssemblyQualifiedName))
            {
                _viewToViewModelTypeDictionary.Add(typeof(TView).AssemblyQualifiedName, typeof(TViewModel));
            }
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
            if (_viewModelToViewTypeDictionary.ContainsKey((typeof(TViewModel).AssemblyQualifiedName, contract)))
            {
                return _viewModelToViewTypeDictionary[(typeof(TViewModel).AssemblyQualifiedName, contract)];
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
            if (viewModelType == null)
            {
                throw new ArgumentNullException(nameof(viewModelType));
            }

            if (_viewModelToViewTypeDictionary.ContainsKey((viewModelType.AssemblyQualifiedName, contract)))
            {
                return _viewModelToViewTypeDictionary[(viewModelType.AssemblyQualifiedName, contract)];
            }

            return null;
        }

        /// <summary>
        /// Method to get viewmodel type for route.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The viewmodel Type.</returns>
        public Type ResolveViewModelType(string route)
        {
            if (_routeToViewModelTypeDictionary.ContainsKey(route))
            {
                return _routeToViewModelTypeDictionary[route];
            }

            return null;
        }

        /// <summary>
        /// Method to get viewmodel type for view.
        /// </summary>
        /// <typeparam name="TView">The view Type.</typeparam>
        /// <returns>The viewmodel Type.</returns>
        public Type ResolveViewModelType<TView>()
            where TView : class
        {
            if (_viewToViewModelTypeDictionary.ContainsKey(typeof(TView).AssemblyQualifiedName))
            {
                return _viewToViewModelTypeDictionary[typeof(TView).AssemblyQualifiedName];
            }

            return null;
        }

        /// <summary>
        /// Method to get viewmodel type for view.
        /// </summary>
        /// <param name="viewType">The view Type.</param>
        /// <returns>The viewmodel Type.</returns>
        public Type ResolveViewModelType(Type viewType)
        {
            if (viewType == null)
            {
                throw new ArgumentNullException(nameof(viewType));
            }

            if (_viewToViewModelTypeDictionary.ContainsKey(viewType.AssemblyQualifiedName))
            {
                return _viewToViewModelTypeDictionary[viewType.AssemblyQualifiedName];
            }

            return null;
        }

        /// <summary>
        /// Method to get route for viewmodel type.
        /// </summary>
        /// <param name="viewModelType">The viewmodel Type.</param>
        /// <returns>The route.</returns>
        public string ResolveRoute(Type viewModelType)
        {
            if (viewModelType == null)
            {
                throw new ArgumentNullException(nameof(viewModelType));
            }

            if (_viewModelTypeToRouteDictionary.ContainsKey(viewModelType.AssemblyQualifiedName))
            {
                return _viewModelTypeToRouteDictionary[viewModelType.AssemblyQualifiedName];
            }

            return null;
        }
    }
}
