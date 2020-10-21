using Avalonia;
using Avalonia.Markup.Xaml;
using Sextant;
using SextantSample.Avalonia.Views;
using SextantSample.ViewModels;
using Splat;

namespace SextantSample.Avalonia
{
    public class App : Application
    {
        public override void Initialize() => AvaloniaXamlLoader.Load(this);

        public override void OnFrameworkInitializationCompleted()
        {
            // Register the views.
            Locator.CurrentMutable
                .RegisterView<GreenView, GreenViewModel>()
                .RegisterView<FirstModalView, FirstModalViewModel>();

            // Push the view model.
            Locator.Current
                .GetService<IViewStackService>()
                .PushPage(new GreenViewModel(Locator.Current.GetService<IViewStackService>()));

            base.OnFrameworkInitializationCompleted();
        }
    }
}