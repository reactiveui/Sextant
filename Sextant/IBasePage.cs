namespace Sextant
{
    public interface IBasePage<out TPageModel> where TPageModel : class, IBasePageModel
    {
    }
}