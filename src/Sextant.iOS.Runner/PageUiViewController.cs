// Copyright (c) 2021 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;

namespace Sextant.IOS.Runner
{
    internal class PageUiViewController : IViewFor<PageViewModelMock>
    {
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (PageViewModelMock)value;
        }

        public PageViewModelMock ViewModel { get; set; } = new PageViewModelMock();
    }
}
