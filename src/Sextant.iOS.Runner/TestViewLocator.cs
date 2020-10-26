// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using ReactiveUI;
using UIKit;

namespace Sextant.IOS.Runner
{
    internal class TestViewLocator
        : IViewLocator
    {
        public IViewFor ResolveView<T>(T viewModel, string contract = null)
        {
            return new PageUiViewController();
        }
    }
}
