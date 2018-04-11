using System;
namespace Sextant
{
	public interface IBaseView<out TViewModel> where TViewModel : class, IBaseViewModel
    {
    }
}
