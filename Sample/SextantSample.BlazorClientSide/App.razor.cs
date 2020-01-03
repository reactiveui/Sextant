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
        [Inject] 
        private IJSRuntime _jsRuntime { get; set; }

        [Inject] 
        private HttpClient _httpClient { get; set; }

        public App()
        {
            PlatformEnlightenmentProvider.Current.EnableWasm();

            Locator
                .CurrentMutable
                .RegisterViewModelFactory(() => new DefaultViewModelFactory())
                .RegisterUrlParameterViewModelGenerator()
                .RegisterRouteViewViewModelLocator()
                .RegisterBlazorRoute<Pages.HomeView, HomeViewModel>("/")
                .RegisterBlazorRoute<Pages.RedView, RedViewModel>("/red")
                .RegisterBlazorRoute<Pages.GreenView, GreenViewModel>("/green")
                .RegisterViewModel(() => new GreenViewModel(Locator.Current.GetService<IViewStackService>()));

            var urlVmGenerator = Locator.Current.GetService<UrlParameterViewModelGenerator>();
            urlVmGenerator.Register<HomeViewModel>(p => new HomeViewModel());
        }
    }
}
