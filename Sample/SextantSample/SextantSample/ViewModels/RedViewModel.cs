using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Sextant;

namespace SextantSample.ViewModels
{
	public class RedViewModel : ViewModelBase
	{
		public ReactiveCommand<Unit, bool> PopModal
		{
			get;
			set;
		}

		public RedViewModel()
		{
			PopModal = ReactiveCommand
                .CreateFromTask(() =>
                    this.PopModalPageAsync(),
                    outputScheduler: RxApp.MainThreadScheduler);

			PopModal.Subscribe(x => Debug.WriteLine("PagePushed"));
		}
	}
}
