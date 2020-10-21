using System.Reactive.Disposables;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SextantSample.ViewModels;

namespace SextantSample.Avalonia.Views
{
    public class HomeView : ReactiveUserControl<HomeViewModel>
    {
        public HomeView()
        {
            AvaloniaXamlLoader.Load(this);
            this.WhenActivated(disposables =>
            {
                this.WhenAnyValue(x => x.ViewModel)
                    .BindTo(this, x => x.DataContext)
                    .DisposeWith(disposables);
            });
        }
    }
}