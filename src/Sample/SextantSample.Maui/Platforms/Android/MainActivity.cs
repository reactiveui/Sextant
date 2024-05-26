using Android.App;
using Android.Content.PM;
using Android.OS;

namespace SextantSample.Maui;

/// <summary>
/// MainActivity.
/// </summary>
/// <seealso cref="MauiAppCompatActivity" />
[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
public class MainActivity : MauiAppCompatActivity
{
    /// <summary>
    /// Called when [create].
    /// </summary>
    /// <param name="savedInstanceState">State of the saved instance.</param>
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        Platform.Init(this, savedInstanceState);
    }

    /// <summary>
    /// Called when [request permissions result].
    /// </summary>
    /// <param name="requestCode">The request code.</param>
    /// <param name="permissions">The permissions.</param>
    /// <param name="grantResults">The grant results.</param>
    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
    {
        Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
}
