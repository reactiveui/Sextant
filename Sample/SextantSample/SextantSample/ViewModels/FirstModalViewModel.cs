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
              
		public FirstModalViewModel()
		{
			OpenModal = ReactiveCommand
                .CreateFromTask(() =>
                    this.PushModalPageAsync<FirstModalNavigationViewModel, FirstModalViewModel>(),
                    outputScheduler: RxApp.MainThreadScheduler);

            OpenModal.Subscribe(x => Debug.WriteLine("PagePushed"));

		}
	}
}
