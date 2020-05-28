using Xamarin.Forms;

namespace Sextant.Shell
{
    public interface IShellRouteConverter
    {
        ShellNavigationState Convert(IRoute route);
    }
}
