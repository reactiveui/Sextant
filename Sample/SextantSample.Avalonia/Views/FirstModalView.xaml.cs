using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using SextantSample.ViewModels;

namespace SextantSample.Avalonia.Views
{
    public class FirstModalView : ReactiveUserControl<FirstModalViewModel>
    {
        public FirstModalView() => AvaloniaXamlLoader.Load(this);
    }
}