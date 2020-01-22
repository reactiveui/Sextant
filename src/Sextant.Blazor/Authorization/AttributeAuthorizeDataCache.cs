// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Sextant.Blazor
{
    /// <summary>
    /// Class used to store auth stuff.  Blazor native version is internal so we can use it.  This is a clone.
    /// </summary>
    internal static class AttributeAuthorizeDataCache
    {
        private static ConcurrentDictionary<Type, IAuthorizeData[]> _cache
            = new ConcurrentDictionary<Type, IAuthorizeData[]>();

        public static IAuthorizeData[] GetAuthorizeDataForType(Type type)
        {
            IAuthorizeData[] result;
            if (!_cache.TryGetValue(type, out result))
            {
                result = ComputeAuthorizeDataForType(type);
                _cache[type] = result; // Safe race - doesn't matter if it overwrites
            }

            return result;
        }

        private static IAuthorizeData[] ComputeAuthorizeDataForType(Type type)
        {
            // Allow Anonymous skips all authorization
            var allAttributes = type.GetCustomAttributes(inherit: true);
            if (allAttributes.OfType<IAllowAnonymous>().Any())
            {
                return null;
            }

            var authorizeDataAttributes = allAttributes.OfType<IAuthorizeData>().ToArray();
            return authorizeDataAttributes.Length > 0 ? authorizeDataAttributes : null;
        }
    }
}
