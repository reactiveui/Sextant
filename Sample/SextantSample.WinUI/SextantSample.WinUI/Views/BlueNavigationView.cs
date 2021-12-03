using System;
using System.Drawing;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using ReactiveUI;

namespace SextantSample.WinUI
{
    public class BlueNavigationView : Sextant.WinUI.NavigationView, IViewFor
    {
        public BlueNavigationView()
            : base(RxApp.MainThreadScheduler, RxApp.TaskpoolScheduler, ViewLocator.Current)
        {
            Background = new SolidColorBrush(Microsoft.UI.Colors.Blue);
            Foreground = new SolidColorBrush(Microsoft.UI.Colors.White);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>")]
        public object ViewModel { get { throw new NotImplementedException(); } set => throw new NotImplementedException(); }
    }
}
