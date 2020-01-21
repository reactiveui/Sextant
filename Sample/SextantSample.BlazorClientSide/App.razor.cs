using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.PlatformServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Sextant;
using Sextant.Blazor;
using SextantSample.ViewModels;
using Splat;

namespace SextantSample.BlazorClientSide
{
    public partial class App : ComponentBase
    {
        public App()
        {
            //PlatformEnlightenmentProvider.Current.EnableWasm();

            Locator
                .CurrentMutable
                .RegisterViewModelFactory(() => new DefaultViewModelFactory())
                .RegisterUrlParameterViewModelGenerator()
                .RegisterRouteViewViewModelLocator()
                .RegisterRoute<Pages.HomeView, HomeViewModel>("/", p => new HomeViewModel())
                .RegisterRoute<Pages.RedView, RedViewModel>("/red", p => new RedViewModel(Locator.Current.GetService<IViewStackService>()))
                .RegisterRoute<Pages.GreenView, GreenViewModel>("/green", p => new GreenViewModel(Locator.Current.GetService<IViewStackService>()))
                .RegisterRoute<Pages.FirstModalView, FirstModalViewModel>("/firstModal")
                .RegisterRoute<Pages.SecondModalView, SecondModalViewModel>("/secondModal")
                .RegisterViewModel(() => new GreenViewModel(Locator.Current.GetService<IViewStackService>()));

            // Blazor apps start via url navigation so there's no need to push a start page.  The start page is determined by the url.
        }
    }
}
