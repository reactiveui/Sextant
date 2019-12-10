﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Sextant.Abstractions;
using Splat;

namespace Sextant
{
    /// <summary>
    /// An exception that is thrown if we are unable to find the View Model Factory.
    /// </summary>
    [Serializable]
    public class ViewModelFactoryNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelFactoryNotFoundException"/> class.
        /// </summary>
        public ViewModelFactoryNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelFactoryNotFoundException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ViewModelFactoryNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelFactoryNotFoundException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ViewModelFactoryNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelFactoryNotFoundException"/> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization information.</param>
        /// <param name="streamingContext">The streaming context.</param>
        protected ViewModelFactoryNotFoundException(SerializationInfo serializationInfo, StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }
    }
}
