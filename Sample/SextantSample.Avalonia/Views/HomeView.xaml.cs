using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SextantSample.ViewModels;

namespace SextantSample.Avalonia.Views
{
    public class HomeView : ReactiveUserControl<HomeViewModel>
    {
        public HomeView() => AvaloniaXamlLoader.Load(this);
    }
}