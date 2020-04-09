// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using DynamicData;
using Microsoft.Extensions.DependencyInjection;

namespace Sextant.Blazor
{
    /// <summary>
    /// Microsoft Dependency Injection Extensions for Sextant Blazor registrations.
    /// </summary>
    public static class SextantMicrosoftDependencyInjectionExtensions
    {
        /// <summary>
        /// Adds Sextant dependencies to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The services.</returns>
        public static IServiceCollection AddSextant(this IServiceCollection services)
        {
            services.AddSingleton<NavigationRouter>();
            services.AddSingleton<IParameterViewStackService>();
            services.AddSingleton<SextantNavigationManager>();
            services.AddSingleton<DefaultViewModelFactory>();
            services.AddSingleton<RouteViewViewModelLocator>();
            services.AddSingleton<UrlParameterViewModelGenerator>();
            return services;
        }
    }
}
