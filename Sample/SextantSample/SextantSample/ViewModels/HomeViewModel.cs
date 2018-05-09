using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Sextant;

namespace SextantSample.ViewModels
{
	public class HomeViewModel : ViewModelBase
	{
		public ReactiveCommand<Unit, bool> OpenModal
		{
			get;
			set;
		}

		public HomeViewModel()
		{
			OpenModal = ReactiveCommand
				.CreateFromTask(() =>
								this.PushModalPageAsync<FirstModalNavigationViewModel, FirstModalViewModel>(),
						        outputScheduler: RxApp.MainThreadScheduler);

			OpenModal.Subscribe(x => Debug.WriteLine("PagePushed"));
		}
	}
}
