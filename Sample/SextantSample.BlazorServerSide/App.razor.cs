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
using static Sextant.Sextant;

namespace SextantSample.BlazorServerSide
{
    public partial class App : ComponentBase
    {
        public App()
        {
            Instance.InitializeBlazor();

            Locator
                .CurrentMutable
                .RegisterBlazorRoute<Pages.HomeView, HomeViewModel>("/", p => new HomeViewModel())
                .RegisterBlazorRoute<Pages.RedView, RedViewModel>("/red", p => new RedViewModel(Locator.Current.GetService<IViewStackService>()))
                .RegisterBlazorRoute<Pages.GreenView, GreenViewModel>("/green", p => new GreenViewModel(Locator.Current.GetService<IViewStackService>()))
                .RegisterBlazorRoute<Pages.FirstModalView, FirstModalViewModel>("/firstModal")
                .RegisterBlazorRoute<Pages.SecondModalView, SecondModalViewModel>("/secondModal")
                .RegisterViewModel(() => new GreenViewModel(Locator.Current.GetService<IViewStackService>()));

            // Blazor apps start via url navigation so there's no need to push a start page.  The start page is determined by the url.
        }
    }
}
