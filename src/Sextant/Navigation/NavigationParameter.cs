// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Sextant
{
#pragma warning disable CA1710, CA2237
    /// <summary>
    /// Represents parameters that can be passed during navigation.
    /// </summary>
    /// <seealso cref="object" />
    /// <seealso cref="INavigationParameter" />
    public class NavigationParameter : Dictionary<string, object>, INavigationParameter
    {
    }
#pragma warning restore CA1710, CA2237
}
