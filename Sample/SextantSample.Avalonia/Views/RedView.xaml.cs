using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SextantSample.ViewModels;

namespace SextantSample.Avalonia.Views
{
    public class RedView : ReactiveUserControl<RedViewModel>
    {
        public RedView() => AvaloniaXamlLoader.Load(this);
    }
}