using System.Reactive;
using ReactiveUI;
using Sextant;
using System;
using System.Diagnostics;

namespace SextantSample.ViewModels
{
	public class SecondModalViewModel : ViewModelBase
	{
		public ReactiveCommand<Unit, bool> PushPage
        {
            get;
            set;
        }

		public ReactiveCommand<Unit, bool> PopModal
        {
            get;
            set;
        }
              
		public SecondModalViewModel()
		{
			PushPage = ReactiveCommand
                .CreateFromTask(() =>
                    this.PushModalPageAsync<FirstModalNavigationViewModel, FirstModalViewModel>(),
                    outputScheduler: RxApp.MainThreadScheduler);

			PopModal = ReactiveCommand
                .CreateFromTask(() =>
	                this.PopModalPageAsync(),
                    outputScheduler: RxApp.MainThreadScheduler);

            PushPage.Subscribe(x => Debug.WriteLine("PagePushed"));
			PopModal.Subscribe(x => Debug.WriteLine("PagePoped"));

		}
	}
}
