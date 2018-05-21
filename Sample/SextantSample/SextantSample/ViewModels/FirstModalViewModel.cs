using System.Reactive;
using ReactiveUI;
using Sextant;
using System;
using System.Diagnostics;

namespace SextantSample.ViewModels
{
	public class FirstModalViewModel : ViewModelBase
	{
		public ReactiveCommand<Unit, bool> OpenModal
        {
            get;
            set;
        }

		public ReactiveCommand<Unit, bool> PopModal
        {
            get;
            set;
        }
              
		public FirstModalViewModel()
		{
			OpenModal = ReactiveCommand
                .CreateFromTask(() =>
                    this.PushModalPageAsync<SecondModalNavigationViewModel, SecondModalViewModel>(),
                    outputScheduler: RxApp.MainThreadScheduler);

			PopModal = ReactiveCommand
                .CreateFromTask(() =>
	                this.PopModalPageAsync(),
                    outputScheduler: RxApp.MainThreadScheduler);

            OpenModal.Subscribe(x => Debug.WriteLine("PagePushed"));
			PopModal.Subscribe(x => Debug.WriteLine("PagePoped"));

		}
	}
}
