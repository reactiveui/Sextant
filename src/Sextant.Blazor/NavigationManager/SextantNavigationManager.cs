// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Sextant.Blazor
{
    internal class SextantNavigationManager
    {
        public static readonly SextantNavigationManager Instance = new SextantNavigationManager();

        private EventHandler<NavigationActionEventArgs> _locationChanged;
        private IJSRuntime _jsRuntime;
        private string _baseUri;
        private string _absoluteUri;

        /// <summary>
        /// An event that fires when the navigation location has changed.
        /// </summary>
        public event EventHandler<NavigationActionEventArgs> LocationChanged
        {
            add
            {
                _locationChanged += value;
            }

            remove
            {
                _locationChanged -= value;
            }
        }

        internal string BaseUri { get => _baseUri; }

        internal string AbsoluteUri { get => _absoluteUri; }

        /// <summary>
        /// Initialize base url.
        /// </summary>
        internal async Task InitializeAsync(IJSRuntime jSRuntime)
        {
            _jsRuntime = jSRuntime;
#pragma warning disable RCS1090 // Call 'ConfigureAwait(false)'.
            _baseUri = await _jsRuntime.InvokeAsync<string>("SextantFunctions.getBaseUri");
            _absoluteUri = await _jsRuntime.InvokeAsync<string>("SextantFunctions.getLocationHref");
#pragma warning restore RCS1090 // Call 'ConfigureAwait(false)'.
        }

        [Obsolete]
        internal ValueTask NavigateToAsync(string uri) =>
            _jsRuntime.InvokeVoidAsync("SextantFunctions.navigateTo", uri);

        internal ValueTask ClearHistory() =>
            _jsRuntime.InvokeVoidAsync("SextantFunctions.clearHistory");

        internal ValueTask ReplaceStateAsync(string viewModelId) =>
            _jsRuntime.InvokeVoidAsync("SextantFunctions.replaceState", new Dictionary<string, object>() { { "id", viewModelId }, { "shouldHandleInternally", false } });

        internal ValueTask GoBackAsync() =>
            _jsRuntime.InvokeVoidAsync("SextantFunctions.goBack");

        internal ValueTask GoToRootAsync(int count)
        {
            return _jsRuntime.InvokeVoidAsync("SextantFunctions.goToRoot", count);
        }

        internal string ToBaseRelativePath(string uri)
        {
            if (uri.StartsWith(_baseUri, StringComparison.Ordinal))
            {
                // The absolute URI must be of the form "{baseUri}something" (where
                // baseUri ends with a slash), and from that we return "something"
                return uri.Substring(_baseUri.Length);
            }

            var hashIndex = uri.IndexOf('#');
            var uriWithoutHash = hashIndex < 0 ? uri : uri.Substring(0, hashIndex);
            if ($"{uriWithoutHash}/".Equals(_baseUri, StringComparison.Ordinal))
            {
                // Special case: for the base URI "/something/", if you're at
                // "/something" then treat it as if you were at "/something/" (i.e.,
                // with the trailing slash). It's a bit ambiguous because we don't know
                // whether the server would return the same page whether or not the
                // slash is present, but ASP.NET Core at least does by default when
                // using PathBase.
                return uri.Substring(_baseUri.Length - 1);
            }

            var message = $"The URI '{uri}' is not contained by the base URI '{_baseUri}'.";
            throw new ArgumentException(message);
        }

        /// <summary>
        /// Triggers the <see cref="LocationChanged"/> event with the current URI value.
        /// </summary>
        internal void NotifyNavigationAction(SextantNavigationType sextantNavigationType, string uri, string id)
        {
            try
            {
                _locationChanged?.Invoke(this, new NavigationActionEventArgs(sextantNavigationType, uri, id));
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occurred while dispatching a location changed event.", ex);
            }
        }
    }
}
