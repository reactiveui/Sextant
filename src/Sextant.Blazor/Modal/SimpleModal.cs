// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using ReactiveUI;

namespace Sextant.Blazor.Modal
{
    /// <summary>
    /// Class that implements IModalView to be used in Sextant.Blazor.
    /// </summary>
    public class SimpleModal : ComponentBase, IModalView
    {
        private bool _isOpen = false;
        private Type _view;
        private IViewModel _currentViewModel;

        /// <inheritdoc/>
        public Task HideAsync()
        {
            _isOpen = false;
            return InvokeAsync(StateHasChanged);
        }

        /// <inheritdoc/>
        public Task ShowViewAsync(Type viewType, IViewModel viewModel)
        {
            _view = viewType;
            _isOpen = true;
            _currentViewModel = viewModel;
            return InvokeAsync(StateHasChanged);
        }

        /// <inheritdoc/>
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (_isOpen)
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "style", "position:fixed;width:100%;height:100%;top:0;left:0;");

                builder.OpenElement(2, "div");
                builder.AddAttribute(3, "style", "position:fixed;width:100%;height:100%;top:0;left:0;background-color:gray;opacity:0.6;");
                builder.CloseElement();

                builder.OpenElement(4, "div");
                builder.AddAttribute(5, "style", "width:80%;height:80%;background-color:white;top:10%;margin:0 auto;position:relative;");
                if (_view != null)
                {
                    builder.OpenComponent(6, _view);
                    builder.AddComponentReferenceCapture(7, viewReference =>
                    {
                        ((IViewFor)viewReference).ViewModel = _currentViewModel;
                    });
                    builder.CloseComponent();
                }

                builder.CloseElement();
                builder.CloseElement();
            }
        }
    }
}
