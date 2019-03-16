// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;
using Sextant.Abstraction;

namespace Sextant.Tests
{
    internal class PageMock<T> : IViewFor<T>
        where T : class, IPageViewModel, new()
    {
        public PageMock()
        {
            ViewModel = new T();
        }

        public T ViewModel { get; set; }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (T)value;
        }
    }
}
