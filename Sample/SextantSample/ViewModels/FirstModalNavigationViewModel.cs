using System;
using ReactiveUI;
using Sextant;
using Sextant.Abstraction;

namespace SextantSample.ViewModels
{
	public class FirstModalNavigationViewModel : ViewModelBase, IPageViewModel
	{
		public FirstModalNavigationViewModel(IViewStackService viewStackService) : base(viewStackService)
		{
			
		}
		public string Id => nameof(FirstModalNavigationViewModel);
	}
}
