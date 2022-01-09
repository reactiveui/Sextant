using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using ReactiveUI;
using Sextant.WinUI;
using Splat;

namespace SextantSample.WinUI
{
    public class BlueNavigationView : NavigationView, IViewFor
    {
        public BlueNavigationView()
            : base(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, ViewLocator.Current, Locator.Current.GetService<IWindowManager>()!)
        {
            Background = new SolidColorBrush(Colors.Blue);
            Foreground = new SolidColorBrush(Colors.White);
        }

        [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public object ViewModel { get { throw new NotImplementedException(); } set => throw new NotImplementedException(); }
    }
}
