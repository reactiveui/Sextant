// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using ReactiveUI;

namespace Sextant.Blazor
{
    /// <summary>
    /// Displays the specified page component, rendering it inside its layout
    /// and any further nested layouts.
    /// </summary>
    public class ReactiveRouteView : IComponent
    {
        private readonly RenderFragment _renderDelegate;
        private readonly RenderFragment _renderPageWithParametersDelegate;
        private RenderHandle _renderHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveRouteView"/> class.
        /// </summary>
        public ReactiveRouteView()
        {
            // Cache the delegate instances
            _renderDelegate = Render;
            _renderPageWithParametersDelegate = RenderPageWithParameters;
        }

        /// <summary>
        /// Gets or sets the sextant router as a cascading parameter so we can get the viewmodel and set the latest view reference.
        /// </summary>
        [CascadingParameter]
        public SextantRouter Router { get; set; }

        /// <summary>
        /// Gets or sets the route data. This determines the page that will be
        /// displayed and the parameter values that will be supplied to the page.
        /// </summary>
        [Parameter]
        public RouteData RouteData { get; set; }

        /// <summary>
        /// Gets or sets the type of a layout to be used if the page does not
        /// declare any layout. If specified, the type must implement <see cref="IComponent"/>
        /// and accept a parameter named <see cref="LayoutComponentBase.Body"/>.
        /// </summary>
        [Parameter]
        public Type DefaultLayout { get; set; }

        /// <inheritdoc />
        public void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
        }

        /// <inheritdoc />
        public Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (RouteData == null)
            {
                throw new InvalidOperationException($"The {nameof(RouteView)} component requires a non-null value for the parameter {nameof(RouteData)}.");
            }

            _renderHandle.Render(_renderDelegate);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Renders the component.
        /// </summary>
        /// <param name="builder">The <see cref="RenderTreeBuilder"/>.</param>
        protected virtual void Render(RenderTreeBuilder builder)
        {
            var pageLayoutType = RouteData.PageType.GetCustomAttribute<LayoutAttribute>()?.LayoutType
                ?? DefaultLayout;

            builder.OpenComponent<LayoutView>(0);
            builder.AddAttribute(1, nameof(LayoutView.Layout), pageLayoutType);
            builder.AddAttribute(2, nameof(LayoutView.ChildContent), _renderPageWithParametersDelegate);
            builder.CloseComponent();
        }

        private void RenderPageWithParameters(RenderTreeBuilder builder)
        {
            builder.OpenComponent(0, RouteData.PageType);

            // Need to force Blazor to render a new page even if the page type is the same or else the ViewModel setting below won't happen.
            // Setting a key will force a re-render even if the type is the same when router state changes.
            builder.SetKey(Guid.NewGuid().ToString());

            builder.AddComponentReferenceCapture(1, compRef =>
            {
                if (compRef is IViewFor)
                {
                    // this works for url and link navigation, but CurrentViewModel not set in time for popstate nav.
                    Debug.WriteLine("RouteView: Checking VM not null");
                    if (Router.CurrentViewModel != null)
                    {
                        Debug.WriteLine("RouteView: Gettings VM type from IViewFor<>");
                        var i = compRef.GetType().GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IViewFor<>));
                        Debug.WriteLine($"RouteView: Type required is {i.GetGenericArguments()[0]}");

                        var args = i.GetGenericArguments();
                        Debug.WriteLine($"RouteView: CurrentViewModel Type is {Router.CurrentViewModel}");
                        if (args.Length > 0 && args[0].FullName == Router.CurrentViewModel.GetType().FullName)
                        {
                            (compRef as IViewFor).ViewModel = Router.CurrentViewModel;
                        }
                    }

                    Router.CurrentView = compRef;
                }
            });

            foreach (var kvp in RouteData.RouteValues)
            {
                builder.AddAttribute(2, kvp.Key, kvp.Value);
            }

            builder.CloseComponent();
        }
    }
}
