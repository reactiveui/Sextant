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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "Blazor standard.")]
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "div");
            builder.AddAttribute(1, "style", "width:100vw;height:100vh;background-color:gray;");

            builder.OpenElement(2, "div");
            builder.AddAttribute(3, "style", "width:80vw;height:80vh;background-color:white;");
            if (_view != null)
            {
                builder.OpenComponent(4, _view);
                builder.AddComponentReferenceCapture(5, (viewReference) =>
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
