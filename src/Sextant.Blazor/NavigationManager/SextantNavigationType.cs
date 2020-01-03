// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Sextant.Blazor
{
    /// <summary>
    /// Type of navigation.
    /// </summary>
    public enum SextantNavigationType
    {
        /// <summary>
        /// Forward navigation.
        /// </summary>
        Forward = 0,

        /// <summary>
        /// Back navigation.
        /// </summary>
        Back = 1,

        /// <summary>
        /// Typed in navigation from url or link click.
        /// </summary>
        Url = 2,

        /// <summary>
        /// Browser navigation or history.go/back.
        /// </summary>
        Popstate = 3
    }
}
