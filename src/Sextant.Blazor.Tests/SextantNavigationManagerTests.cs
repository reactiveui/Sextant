// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Shouldly;
using Xunit;

namespace Sextant.Blazor.Tests
{
    /// <summary>
    /// Tests the <see cref="SextantNavigationManager"/>.
    /// </summary>
    public class SextantNavigationManagerTests
    {
        /// <summary>
        /// Tests the absolute uri property.
        /// </summary>
        /// <returns>A completion.</returns>
        [Fact]
        public async Task Should_Return_Absolute_Uri()
        {
            // Given
            SextantNavigationManager manager = new SextantNavigationManagerFixture();
            await manager.InitializeAsync(new JavaScriptRuntimeMock()).ConfigureAwait(false);

            // When
            var result = manager.AbsoluteUri;

            // Then
            result.ShouldBe("https://reactiveui.net");
        }

        /// <summary>
        /// Tests the base uri property.
        /// </summary>
        /// <returns>A completion.</returns>
        [Fact]
        public async Task Should_Return_Base_Uri()
        {
            // Given
            SextantNavigationManager manager = new SextantNavigationManagerFixture();
            await manager.InitializeAsync(new JavaScriptRuntimeMock()).ConfigureAwait(false);

            // When
            var result = manager.BaseUri;

            // Then
            result.ShouldBe("https://reactiveui.net");
        }

        /// <summary>
        /// Tests the GoBackAsync method calls the javascript runtime.
        /// </summary>
        /// <returns>A completion.</returns>
        [Fact]
        public async Task Should_Call_Navigate_To_Js_Runtime()
        {
            // Given
            var received = false;
            SextantNavigationManager manager = new SextantNavigationManagerFixture();

            var jsRuntime = Substitute.For<IJSRuntime>();
            jsRuntime
                .When(x => x.InvokeVoidAsync("SextantFunctions.navigateTo"))
                .Do(_ => received = true);

            // When
            await manager.InitializeAsync(jsRuntime).ConfigureAwait(false);
            await manager.NavigateToAsync("ViewModel");

            // Then
            received.ShouldBeTrue();
        }

        /// <summary>
        /// Tests the GoBackAsync method calls the javascript runtime.
        /// </summary>
        /// <returns>A completion.</returns>
        [Fact]
        public async Task Should_Call_Go_Back_Js_Runtime()
        {
            // Given
            var received = false;
            SextantNavigationManager manager = new SextantNavigationManagerFixture();

            var jsRuntime = Substitute.For<IJSRuntime>();
            jsRuntime
                .When(x => x.InvokeVoidAsync("SextantFunctions.goBack"))
                .Do(_ => received = true);

            // When
            await manager.InitializeAsync(jsRuntime).ConfigureAwait(false);
            await manager.GoBackAsync();

            // Then
            received.ShouldBeTrue();
        }

        /// <summary>
        /// Tests the GoBackAsync method calls the javascript runtime.
        /// </summary>
        /// <returns>A completion.</returns>
        [Fact]
        public async Task Should_Call_Clear_History_Js_Runtime()
        {
            // Given
            var received = false;
            SextantNavigationManager manager = new SextantNavigationManagerFixture();

            var jsRuntime = Substitute.For<IJSRuntime>();
            jsRuntime
                .When(x => x.InvokeVoidAsync("SextantFunctions.clearHistory"))
                .Do(_ => received = true);

            // When
            await manager.InitializeAsync(jsRuntime).ConfigureAwait(false);
            await manager.ClearHistory();

            // Then
            received.ShouldBeTrue();
        }

        /// <summary>
        /// Tests the GoBackAsync method calls the javascript runtime.
        /// </summary>
        /// <returns>A completion.</returns>
        [Fact]
        public async Task Should_Call_Replace_History_Js_Runtime()
        {
            // Given
            var received = false;
            SextantNavigationManager manager = new SextantNavigationManagerFixture();

            var jsRuntime = Substitute.For<IJSRuntime>();
            jsRuntime
                .When(x => x.InvokeVoidAsync("SextantFunctions.replaceState", Arg.Any<Dictionary<string, object>>()))
                .Do(_ => received = true);

            // When
            await manager.InitializeAsync(jsRuntime).ConfigureAwait(false);
            await manager.ReplaceStateAsync("1");

            // Then
            received.ShouldBeTrue();
        }

        /// <summary>
        /// Tests the GoBackAsync method calls the javascript runtime.
        /// </summary>
        /// <returns>A completion.</returns>
        [Fact]
        public async Task Should_Call_Go_To_Root_Js_Runtime()
        {
            // Given
            var received = false;
            SextantNavigationManager manager = new SextantNavigationManagerFixture();

            var jsRuntime = Substitute.For<IJSRuntime>();
            jsRuntime
                .When(x => x.InvokeVoidAsync("SextantFunctions.goToRoot", Arg.Any<int>()))
                .Do(_ => received = true);

            // When
            await manager.InitializeAsync(jsRuntime).ConfigureAwait(false);
            await manager.GoToRootAsync(1);

            // Then
            received.ShouldBeTrue();
        }
    }
}
