using Microsoft.Maui.Graphics;
using ReactiveUI;
using Sextant.Maui;

namespace SextantSample.Maui.Views
{
    public class BlueNavigationView : NavigationView, IViewFor
    {
        public BlueNavigationView()
            : base(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, ViewLocator.Current)
        {
            BarBackgroundColor = Colors.Blue;
            BarTextColor = Colors.White;
        }

#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
        public object ViewModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations
    }
}
