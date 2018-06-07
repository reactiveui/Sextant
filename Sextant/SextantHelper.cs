using System;
using System.Reactive.Concurrency;
using ReactiveUI;
using Sextant.Abstraction;
using Splat;
using Xamarin.Forms;

namespace Sextant
{
    public static class SextantHelper
    {
        public static void RegisterView<TView, TViewModel>(string contract = null)
            where TView : IViewFor, new()
            where TViewModel : class, IPageViewModel
        {
            Locator.CurrentMutable.Register(() => new TView(), typeof(IViewFor<TViewModel>), contract);
        }

        public static void RegisterNavigation<TView, TViewModel>(IScheduler mainThreadScheduler = null, IScheduler backgroundScheduler = null, IViewLocator viewLocator = null)
            where TView : IViewFor
            where TViewModel : class, IPageViewModel
        {
            var bgScheduler = mainThreadScheduler ?? RxApp.TaskpoolScheduler;
            var mScheduler = backgroundScheduler ?? RxApp.MainThreadScheduler;
            var vLocator = viewLocator ?? Locator.Current.GetService<IViewLocator>();

            Locator.CurrentMutable.Register(
                () => Activator.CreateInstance(typeof(TView), mScheduler, bgScheduler, vLocator),
                typeof(IViewFor<TViewModel>),
                "NavigationView");
        }

        public static NavigationView Initialise<TViewModel>(IScheduler mainThreadScheduler = null, IScheduler backgroundScheduler = null, IViewLocator viewLocator = null)
            where TViewModel : class, IPageViewModel
        {
            var bgScheduler = mainThreadScheduler ?? RxApp.TaskpoolScheduler;
            var mScheduler = backgroundScheduler ?? RxApp.MainThreadScheduler;
            var vLocator = viewLocator ?? Locator.Current.GetService<IViewLocator>();


            var navigationView = new NavigationView(mScheduler, bgScheduler, vLocator);
            var viewStackService = new ViewStackService(navigationView);

            Locator.CurrentMutable.Register<IViewStackService>(() => viewStackService);
            navigationView.PushPage(Activator.CreateInstance(typeof(TViewModel), viewStackService) as TViewModel, null, true, false).Subscribe();

            return navigationView;
        }
    }
}
