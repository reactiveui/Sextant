using ReactiveUI;
using Sextant;
using Splat;

namespace SextantSample.ViewModels
{
	public class ViewModelBase : ReactiveObject, IPageViewModel
	{
		protected readonly IViewStackService ViewStackService;

		public ViewModelBase(IViewStackService viewStackService)
        {
            ViewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();
        }

        public string Id => "ViewModelBase";
    }
}
