[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace SextantSample.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        Locator
            .Current
            .GetService<IViewStackService>()
            .PushPage(new HomeViewModel(), null, true, false)
            .Subscribe();

        MainPage = Locator.Current.GetNavigationView();
    }
}
