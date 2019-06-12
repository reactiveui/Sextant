using System.Reactive;
using ReactiveUI;
using System;
using System.Diagnostics;
using Sextant;
using System.Reactive.Linq;

namespace SextantSample.ViewModels
{
	public class SecondModalViewModel : ViewModelBase, IPageViewModel
	{
		public ReactiveCommand<Unit, Unit> PushPage
        {
            get;
            set;
        }

		public ReactiveCommand<Unit, Unit> PopModal
        {
            get;
            set;
        }

		public string Id => nameof(SecondModalViewModel);

		public SecondModalViewModel(IViewStackService viewStackService) : base(viewStackService)
		{
			PushPage = ReactiveCommand
				.CreateFromObservable(() =>
	                this.ViewStackService.PushPage(new RedViewModel(ViewStackService)),
                    outputScheduler: RxApp.MainThreadScheduler);

			PopModal = ReactiveCommand
				.CreateFromObservable(() =>
	                this.ViewStackService.PopModal(),
                    outputScheduler: RxApp.MainThreadScheduler);

            PushPage.Subscribe(x => Debug.WriteLine("PagePushed"));
			PopModal.Subscribe(x => Debug.WriteLine("PagePoped"));

		    PushPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
		    PopModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }
	}
}
