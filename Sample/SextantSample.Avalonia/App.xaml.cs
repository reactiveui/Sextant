using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using Sextant;
using Sextant.Avalonia;
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
            Locator
                .CurrentMutable
                .RegisterView<HomeView, HomeViewModel>()
                .RegisterView<FirstModalView, FirstModalViewModel>()
                .RegisterView<SecondModalView, SecondModalViewModel>()
                .RegisterView<RedView, RedViewModel>()
                .RegisterView<GreenView, GreenViewModel>()
                .RegisterViewModelFactory(() => new DefaultViewModelFactory())
                .RegisterNavigationView(() => new NavigationView())
                .RegisterViewModel(() => new GreenViewModel(Locator.Current.GetService<IViewStackService>()));

            Locator
                .Current
                .GetService<IViewStackService>()
                .PushPage(new HomeViewModel());

            Interactions.ErrorMessage.RegisterHandler(async context =>
               await MessageBoxManager.GetMessageBoxStandard("Notification", context.Input.ToString())
                    .ShowAsync());

            new Window { Content = Locator.Current.GetNavigationView() }.Show();
            base.OnFrameworkInitializationCompleted();
        }
    }
}
