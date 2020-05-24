﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Sextant
{
    /// <summary>
    /// Represents parameters that can be passed during navigation.
    /// </summary>
    /// <seealso cref="object" />
    /// <seealso cref="INavigationParameter" />
    [SuppressMessage("Design", "CA1710: Identifiers should have correct suffix.", Justification = "Deliberate usage")]
    [SuppressMessage("Design", "CA2237: Mark ISerializable types with SerializableAttribute.", Justification = "Deliberate usage")]
    public class NavigationParameter : Dictionary<string, object>, INavigationParameter
    {
    }
}
