// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ReactiveUI;
using Splat;

namespace Sextant.Benchmarks
{
    /// <summary>
    /// A view locator that holds everything in a static dictionary.
    /// </summary>
    /// <seealso cref="IViewLocator" />
    public class InMemoryViewLocator : IViewLocator
    {
        private static readonly Dictionary<Type, IViewFor> _views = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryViewLocator"/> class.
        /// </summary>
        /// <param name="viewModelToViewFunc">The method which will convert a ViewModel name into a View.</param>
        [SuppressMessage("Globalization", "CA1307: operator could change based on locale settings", Justification = "Replace() does not have third parameter on all platforms")]
        public InMemoryViewLocator(Func<string, string>? viewModelToViewFunc = null)
        {
            _views.Add(typeof(TestView), new TestView());

            ViewModelToViewFunc = viewModelToViewFunc ?? (vm => vm.Replace("ViewModel", "View"));
        }

        /// <summary>
        /// Gets or sets a function that is used to convert a view model name to a proposed view name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If unset, the default behavior is to change "ViewModel" to "View". If a different convention is followed, assign an appropriate function to this
        /// property.
        /// </para>
        /// <para>
        /// Note that the name returned by the function is a starting point for view resolution. Variants on the name will be resolved according to the rules
        /// set out by the <see cref="ResolveView{T}"/> method.
        /// </para>
        /// </remarks>
        public Func<string, string> ViewModelToViewFunc { get; set; }

        /// <inheritdoc />
        public IViewFor? ResolveView<T>(T? viewModel, string? contract = null)
        {
            var view = AttemptViewResolutionFor(viewModel?.GetType(), contract);

            if (view != null)
            {
                return view;
            }

            view = AttemptViewResolutionFor(typeof(T), contract);

            if (view != null)
            {
                return view;
            }

            view = AttemptViewResolutionFor(ToggleViewModelType(viewModel?.GetType()), contract);

            if (view != null)
            {
                return view;
            }

            view = AttemptViewResolutionFor(ToggleViewModelType(typeof(T)), contract);

            if (view != null)
            {
                return view;
            }

            this.Log().Warn($"Failed to resolve view for view model type '{typeof(T).FullName}'.");
            return null;
        }

        private static Type? ToggleViewModelType(Type? viewModelType)
        {
            var viewModelTypeName = viewModelType?.AssemblyQualifiedName;

            if (viewModelType?.GetTypeInfo().IsInterface == true)
            {
                if (viewModelType!.Name.StartsWith("I", StringComparison.InvariantCulture))
                {
                    var toggledTypeName = DeinterfaceifyTypeName(viewModelTypeName);
                    return Reflection.ReallyFindType(toggledTypeName, throwOnFailure: false);
                }
            }
            else
            {
                var toggledTypeName = InterfaceifyTypeName(viewModelTypeName);
                return Reflection.ReallyFindType(toggledTypeName, throwOnFailure: false);
            }

            return null;
        }

        private static string? DeinterfaceifyTypeName(string? typeName)
        {
            var idxComma = typeName?.IndexOf(",", 0, StringComparison.InvariantCulture);
            var idxPeriod = typeName?.LastIndexOf('.', idxComma!.Value - 1);
            return typeName?.Substring(0, idxPeriod!.Value + 1) + typeName?.Substring(idxPeriod!.Value + 2);
        }

        private static string? InterfaceifyTypeName(string? typeName)
        {
            var idxComma = typeName?.IndexOf(",", 0, StringComparison.InvariantCulture);
            var idxPeriod = typeName?.LastIndexOf(".", idxComma!.Value - 1, StringComparison.InvariantCulture);
            return typeName?.Insert(idxPeriod!.Value + 1, "I");
        }

        private IViewFor? AttemptViewResolutionFor(Type? viewModelType, string? contract)
        {
            if (viewModelType == null)
            {
                return null;
            }

            var viewModelTypeName = viewModelType.AssemblyQualifiedName;
            var proposedViewTypeName = ViewModelToViewFunc(viewModelTypeName!);
            var view = AttemptViewResolution(proposedViewTypeName, contract);

            if (view != null)
            {
                return view;
            }

            proposedViewTypeName = typeof(IViewFor<>).MakeGenericType(viewModelType).AssemblyQualifiedName;
            view = AttemptViewResolution(proposedViewTypeName!, contract);

            if (view != null)
            {
                return view;
            }

            return null;
        }

        private IViewFor? AttemptViewResolution(string viewTypeName, string? contract)
        {
            try
            {
                var viewType = Reflection.ReallyFindType(viewTypeName, throwOnFailure: false);
                if (viewType == null)
                {
                    return null;
                }

                if (contract == null)
                {
                    this.Log().Warn("contract is null");
                }

                var view = _views[viewType];
                if (view == null)
                {
                    return null;
                }

                this.Log().Debug($"Resolved service type '{viewType.FullName}'");

                return view;
            }
            catch (Exception ex)
            {
                this
                    .Log()
                    .Error(ex, $"Exception occurred whilst attempting to resolve type {viewTypeName} into a view.");
                throw;
            }
        }
    }
}
