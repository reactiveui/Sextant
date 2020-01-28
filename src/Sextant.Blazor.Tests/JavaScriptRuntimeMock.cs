// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using NSubstitute;
using ReactiveUI.Testing;

namespace Sextant.Blazor.Tests
{
    /// <summary>
    /// Represents a mock <see cref="IJSRuntime"/>.
    /// </summary>
    public class JavaScriptRuntimeMock : IJSRuntime, IBuilder
    {
        private IJSRuntime _jsRuntime;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavaScriptRuntimeMock"/> class.
        /// </summary>
        public JavaScriptRuntimeMock()
        {
            _jsRuntime = Substitute.For<IJSRuntime>();

            _jsRuntime
                .InvokeAsync<string>("SextantFunctions.getLocationHref")
                .Returns(new ValueTask<string>("https://reactiveui.net"));

            _jsRuntime
                .InvokeAsync<string>("SextantFunctions.getBaseUri")
                .Returns(new ValueTask<string>("https://reactiveui.net"));
        }

        /// <inheritdoc />
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object[] args) =>
            _jsRuntime.InvokeAsync<TValue>(identifier, args);

        /// <inheritdoc />
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object[] args) =>
            _jsRuntime.InvokeAsync<TValue>(identifier, cancellationToken, args);
    }
}
