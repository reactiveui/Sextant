using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Sextant.Abstraction;

namespace SextantSample.ViewModels
{
	public class RedViewModel : ViewModelBase, IPageViewModel
	{
		public ReactiveCommand<Unit, Unit> PopModal
		{
			get;
			set;
		}

		public string Id => nameof(RedViewModel);

		public RedViewModel(IViewStackService viewStackService) : base(viewStackService)
		{
			PopModal = ReactiveCommand
				.CreateFromObservable(() =>
                    this.ViewStackService.PopModal(),
                    outputScheduler: RxApp.MainThreadScheduler);

			PopModal.Subscribe(x => Debug.WriteLine("PagePushed"));
		}
	}
}
