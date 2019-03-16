// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Sextant.Abstraction;

namespace Sextant.Tests
{
    internal class PageViewModelMock : IPageViewModel
    {
        private readonly string _id;

        public PageViewModelMock(string id = null)
        {
            _id = id;
        }

        public string Id => _id ?? nameof(PageViewModelMock);
    }
}
