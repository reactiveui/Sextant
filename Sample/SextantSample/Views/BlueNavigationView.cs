using System;
using System.Reactive.Concurrency;
using ReactiveUI;
using Sextant;
using Xamarin.Forms;

namespace SextantSample.Views
{
    public class BlueNavigationView : NavigationView, IViewFor
    {
        public BlueNavigationView(IScheduler mainScheduler, IScheduler backgroundScheduler, IViewLocator viewLocator)
            : base(mainScheduler, backgroundScheduler, viewLocator)
        {
            this.BarBackgroundColor = Color.Blue;
            this.BarTextColor = Color.White;
        }

        public object ViewModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
