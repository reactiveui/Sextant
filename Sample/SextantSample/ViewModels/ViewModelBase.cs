using ReactiveUI;
using Sextant;

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
