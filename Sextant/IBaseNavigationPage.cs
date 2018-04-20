namespace Sextant
{
    public interface IBaseNavigationPage<out TPageModel> where TPageModel : class, IBaseNavigationPageModel
    {
    }
}