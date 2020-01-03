using System.Reactive;
using ReactiveUI;
using System;
using System.Diagnostics;
using Sextant;
using System.Reactive.Linq;

namespace SextantSample.ViewModels
{
    public class SecondModalViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> PushPage { get; set; }

        public ReactiveCommand<Unit, Unit> PopModal { get; set; }

        public override string Id => nameof(SecondModalViewModel);

        public SecondModalViewModel(IViewStackService viewStackService) : base(viewStackService)
        {
            PushPage = ReactiveCommand
                .CreateFromObservable(() =>
                    ViewStackService.PushPage(new RedViewModel(ViewStackService)),
                    outputScheduler: RxApp.MainThreadScheduler);

            PopModal = ReactiveCommand
                .CreateFromObservable(() =>
                    ViewStackService.PopModal(),
                    outputScheduler: RxApp.MainThreadScheduler);

            PushPage.Subscribe(x => Debug.WriteLine("PagePushed"));
            PopModal.Subscribe(x => Debug.WriteLine("PagePoped"));

            PushPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            PopModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }
    }
}
