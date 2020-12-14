using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SextantSample.ViewModels;

namespace SextantSample.Avalonia.Views
{
    public class GreenView : ReactiveUserControl<GreenViewModel>
    {
        public GreenView() => AvaloniaXamlLoader.Load(this);
    }
}