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

        public ReactiveCommand<Unit, Unit> PushPage
        {
            get;
            set;
        }

        public ReactiveCommand<Unit, Unit> PopPage
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

            PopPage = ReactiveCommand
                .CreateFromObservable(() =>
                    this.ViewStackService.PopPage(),
                    outputScheduler: RxApp.MainThreadScheduler);
            
            PushPage = ReactiveCommand
                .CreateFromObservable(() =>
                    this.ViewStackService.PushPage(new RedViewModel(ViewStackService)),
                    outputScheduler: RxApp.MainThreadScheduler);

			PopModal.Subscribe(x => Debug.WriteLine("PagePushed"));
		}
	}
}
