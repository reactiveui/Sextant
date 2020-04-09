// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Sextant.Blazor
{
    /// <summary>
    /// This is the Sextant version of the <see cref="NavigationManager"/> provided by blazor.
    /// </summary>
    public sealed class SextantNavigationManager : IDisposable
    {
        private readonly Subject<NavigationActionEventArgs> _locationChanged;
        private IJSRuntime _jsRuntime;
        private string _baseUri;
        private string _absoluteUri;

        /// <summary>
        /// Initializes a new instance of the <see cref="SextantNavigationManager"/> class.
        /// </summary>
        public SextantNavigationManager()
        {
            _locationChanged = new Subject<NavigationActionEventArgs>();
        }

        /// <summary>
        /// Gets an observable sequence of the navigation location changes.
        /// </summary>
        public IObservable<NavigationActionEventArgs> LocationChanged => _locationChanged.AsObservable();

        /// <summary>
        /// Gets the base uri.
        /// </summary>
        public string BaseUri => _baseUri;

        /// <summary>
        /// Gets the absolute uri.
        /// </summary>
        public string AbsoluteUri => _absoluteUri;

        /// <summary>
        /// Initialize base url.
        /// </summary>
        /// <param name="jSRuntime">The Java Script runtime environment.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task InitializeAsync(IJSRuntime jSRuntime)
        {
            _jsRuntime = jSRuntime;
#pragma warning disable RCS1090 // Call 'ConfigureAwait(false)'.
            _baseUri = await _jsRuntime.InvokeAsync<string>("SextantFunctions.getBaseUri");
            _absoluteUri = await _jsRuntime.InvokeAsync<string>("SextantFunctions.getLocationHref");
#pragma warning restore RCS1090 // Call 'ConfigureAwait(false)'.
        }

        /// <summary>
        /// Clears the browser history.
        /// </summary>
        /// <returns>A notification of completion.</returns>
        public ValueTask ClearHistory() =>
            _jsRuntime.InvokeVoidAsync("SextantFunctions.clearHistory");

        /// <summary>
        /// Replace the state in the browser.
        /// </summary>
        /// <param name="viewModelId">The view model id.</param>
        /// <returns>A notification of completion.</returns>
        public ValueTask ReplaceStateAsync(string viewModelId) =>
            _jsRuntime.InvokeVoidAsync("SextantFunctions.replaceState", new Dictionary<string, object> { { "id", viewModelId }, { "shouldHandleInternally", false } });

        /// <summary>
        /// Go back in the browser.
        /// </summary>
        /// <returns>A notification of completion.</returns>
        public ValueTask GoBackAsync() =>
            _jsRuntime.InvokeVoidAsync("SextantFunctions.goBack");

        /// <summary>
        /// Go to the root of the browser navigation history.
        /// </summary>
        /// <param name="count">number of pages to remove.</param>
        /// <returns>A notification of completion.</returns>
        public ValueTask GoToRootAsync(int count) =>
            _jsRuntime.InvokeVoidAsync("SextantFunctions.goToRoot", count);

        /// <summary>
        /// Converts the uri to a relative path.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <returns>The relative path.</returns>
        public string ToBaseRelativePath(string uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException(nameof(uri));
            }

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
        /// <param name="sextantNavigationType">The navigation type.</param>
        /// <param name="uri">The uri.</param>
        /// <param name="id">The id.</param>
        public void NotifyNavigationAction(SextantNavigationType sextantNavigationType, string uri, string id)
        {
            try
            {
                _locationChanged.OnNext(new NavigationActionEventArgs(sextantNavigationType, uri, id));
            }
            catch (Exception ex)
            {
                throw new Exception("An exception occurred while dispatching a location changed event.", ex);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _locationChanged?.Dispose();
        }
    }
}
