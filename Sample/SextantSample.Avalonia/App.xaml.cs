using System;
using Avalonia;
using Avalonia.Controls;
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

            Interactions.ErrorMessage.RegisterHandler(context => 
                MessageBox.Avalonia
                    .MessageBoxManager
                    .GetMessageBoxStandardWindow("Notification", context.Input.ToString())
                    .Show());

            new Window { Content = Locator.Current.GetNavigationView() }.Show();
            base.OnFrameworkInitializationCompleted();
        }
    }

    public static class Workarounds
    {
        public static IMutableDependencyResolver RegisterNavigationView<TView>(
            this IMutableDependencyResolver dependencyResolver, Func<TView> navigationViewFactory)
            where TView : IView
        {
            var navigationView = navigationViewFactory();
            var viewStackService = new ViewStackService(navigationView);

            dependencyResolver.RegisterLazySingleton<IViewStackService>(() => viewStackService);
            dependencyResolver.RegisterLazySingleton<IView>(() => navigationView, "NavigationView");
            return dependencyResolver;
        }

        public static IView GetNavigationView(
            this IReadonlyDependencyResolver dependencyResolver,
            string contract = null)
        {
            if (dependencyResolver is null)
            {
                throw new ArgumentNullException(nameof(dependencyResolver));
            }

            return dependencyResolver.GetService<IView>(contract ?? "NavigationView");
        }
    }
}