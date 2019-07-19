using ReactiveUI;
using Sextant;

namespace SextantSample.ViewModels
{
	public abstract class ViewModelBase : ReactiveObject
	{
		protected readonly IViewStackService ViewStackService;

		protected ViewModelBase(IViewStackService viewStackService)
        {
            ViewStackService = viewStackService;
        }
	}
}
