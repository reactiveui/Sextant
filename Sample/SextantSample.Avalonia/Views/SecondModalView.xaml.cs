using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SextantSample.ViewModels;

namespace SextantSample.Avalonia.Views
{
    public class SecondModalView : ReactiveUserControl<SecondModalViewModel>
    {
        public SecondModalView() => AvaloniaXamlLoader.Load(this);
    }
}