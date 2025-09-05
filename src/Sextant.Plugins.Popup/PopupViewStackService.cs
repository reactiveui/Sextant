// Copyright (c) 2025 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Mopups.Interfaces;
using ReactiveUI;

namespace Sextant.Plugins.Popup;

/// <summary>
/// Abstract base class for view stack services.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="PopupViewStackService"/> class.
/// </remarks>
/// <param name="view">The view.</param>
/// <param name="popupNavigation">The popup navigation.</param>
/// <param name="viewLocator">The view locator.</param>
/// <param name="viewModelFactory">The view model factory.</param>
public sealed class PopupViewStackService(IView view, IPopupNavigation popupNavigation, IViewLocator viewLocator, IViewModelFactory viewModelFactory)
    : PopupViewStackServiceBase(view, popupNavigation, viewLocator, viewModelFactory);
