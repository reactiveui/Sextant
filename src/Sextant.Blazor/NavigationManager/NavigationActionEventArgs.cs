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
    /// Class for location and state changes.
    /// </summary>
    public class NavigationActionEventArgs
    {
        private string _uri;
        private string _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationActionEventArgs"/> class.
        /// </summary>
        public NavigationActionEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationActionEventArgs"/> class.
        /// </summary>
        /// <param name="sextantNavigationType">Type of navigation: back, forward, uri.</param>
        /// <param name="uri">Current Uri.</param>
        /// <param name="id">Current viewmodel Id.</param>
        public NavigationActionEventArgs(SextantNavigationType sextantNavigationType, string uri, string id)
        {
            NavigationType = sextantNavigationType;
            Uri = uri;
            Id = id;
        }

        /// <summary>
        /// Gets or sets a value indicating whether navigation has actually occurred yet.
        /// </summary>
        public SextantNavigationType NavigationType { get; set; }

        /// <summary>
        /// Gets or sets the Uri.
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }
    }
}
