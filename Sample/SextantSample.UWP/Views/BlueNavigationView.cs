using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Windows.UI.Xaml.Controls;

namespace SextantSample.UWP.Views
{
    public class BlueNavigationView : Sextant.NavigationView, IViewFor
    {
        public BlueNavigationView() : base(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, ViewLocator.Current)
        {
            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Windows.UI.Colors.Blue;
            titleBar.ForegroundColor = Windows.UI.Colors.White;
        }

        public object ViewModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
