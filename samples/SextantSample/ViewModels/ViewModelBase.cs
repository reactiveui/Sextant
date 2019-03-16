using ReactiveUI;
using Sextant.Abstraction;

namespace SextantSample.ViewModels
{
	public class ViewModelBase : ReactiveObject
	{
		protected readonly IViewStackService ViewStackService;

		public ViewModelBase(IViewStackService viewStackService)
        {
            ViewStackService = viewStackService;
        }
	}
}
