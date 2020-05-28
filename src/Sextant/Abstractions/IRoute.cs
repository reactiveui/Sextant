// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using ReactiveUI;

[assembly: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]

namespace Sextant.Shell
{
    /// <summary>
    /// Interface representing a navigable route.
    /// </summary>
    public interface IRoute
    {
    }

    /// <summary>
    /// Interface representing a navigable route.
    /// </summary>
    public interface IUriRoute : IRoute
    {
        /// <summary>
        /// Gets the uri represented route.
        /// </summary>
        Uri Route { get; }
    }

    /// <summary>
    /// Interface representing a navigable route.
    /// </summary>
    public interface IViewModelRoute : IRoute
    {
        /// <summary>
        /// Gets the view model represented route.
        /// </summary>
        IViewModel Route { get; }
    }
}
