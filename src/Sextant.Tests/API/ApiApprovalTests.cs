﻿// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Sextant.APITests;
using Xunit;

namespace Sextant.Tests
{
    /// <summary>
    /// Tests to make sure that the API matches the approved ones.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ApiApprovalTests
    {
        /// <summary>
        /// Tests to make sure the splat project is approved.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public Task Sextant() => typeof(IViewStackService).Assembly.CheckApproval(["Sextant"]);
    }
}
