using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Sextant;
using Sextant.UWP;
using SextantSample.Core.ViewModels;
using SextantSample.UWP.Views;
using Splat;
using static Sextant.Sextant;

namespace SextantSample.UWP
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public AppBootstrapper()
        {
            Instance.InitializeUWP();

            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));

            Locator
                .CurrentMutable
                .RegisterViewUWP<HomeView, HomeViewModel>()
                .RegisterViewUWP<FirstModalView, FirstModalViewModel>()
                .RegisterViewUWP<SecondModalView, SecondModalViewModel>()
                .RegisterViewUWP<RedView, RedViewModel>()
                .RegisterViewUWP<GreenView, GreenViewModel>()
                .RegisterNavigationView(() => new BlueNavigationView())
                .RegisterViewModel(() => new GreenViewModel(Locator.Current.GetService<IViewStackService>()));

            //Locator
            //    .Current
            //    .GetService<IViewStackService>()
            //    .PushPage(new HomeViewModel(), null, true, false)
            //    .Subscribe();

        }

        public RoutingState Router { get; protected set; }
    }
}
