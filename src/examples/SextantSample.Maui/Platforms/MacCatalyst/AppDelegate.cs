using Foundation;

namespace SextantSample.Maui;
    /// <summary>Creates the MAUI application on Apple platforms.</summary>
[Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        /// <inheritdoc />
protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
