// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant.Blazor
{
    /// <summary>
    /// Represents the Sextant Functions javascript names in string format.
    /// </summary>
    public static class SextantFunctions
    {
        /// <summary>
        /// Gets the go back function.
        /// </summary>
        public static string GoBack => $"{nameof(SextantFunctions)}.goBack";

        /// <summary>
        /// Gets the go back function.
        /// </summary>
        public static string GoToRoot => $"{nameof(SextantFunctions)}.goToRoot";

        /// <summary>
        /// Gets the clear history function.
        /// </summary>
        public static string ClearHistory => $"{nameof(SextantFunctions)}.clearHistory";

        /// <summary>
        /// Gets the replace state function.
        /// </summary>
        public static string ReplaceState => $"{nameof(SextantFunctions)}.replaceState";

        /// <summary>
        /// Gets the clear history function.
        /// </summary>
        public static string GetBaseUri => $"{nameof(SextantFunctions)}.getBaseUri";

        /// <summary>
        /// Gets the replace state function.
        /// </summary>
        public static string GetLocationHref => $"{nameof(SextantFunctions)}.getLocationHref";
    }
}
