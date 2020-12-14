using System;

using ReactiveUI;

using Sextant.XamForms;
using Xamarin.Forms;

namespace SextantSample.Views
{
    public class BlueNavigationView : NavigationView, IViewFor
    {
        public BlueNavigationView()
            : base(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, ViewLocator.Current)
        {
            BarBackgroundColor = Color.Blue;
            BarTextColor = Color.White;
        }

        public object ViewModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
