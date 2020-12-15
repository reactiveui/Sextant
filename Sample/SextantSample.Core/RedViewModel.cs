using System;
using System.Diagnostics;
using System.Reactive;
using ReactiveUI;
using Sextant;

namespace SextantSample.ViewModels
{
    public class RedViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> PopModal { get; set; }

        public ReactiveCommand<Unit, Unit> PushPage { get; set; }

        public ReactiveCommand<Unit, Unit> PopPage { get; set; }

        public ReactiveCommand<Unit, Unit> PopToRoot { get; set; }

        public override string Id => nameof(RedViewModel);

        public RedViewModel(IViewStackService viewStackService) : base(viewStackService)
        {
            PopModal = ReactiveCommand
                .CreateFromObservable(() =>
                    ViewStackService.PopModal(),
                    outputScheduler: RxApp.MainThreadScheduler);

            PopPage = ReactiveCommand
                .CreateFromObservable(() =>
                    ViewStackService.PopPage(),
                    outputScheduler: RxApp.MainThreadScheduler);

            PushPage = ReactiveCommand
                .CreateFromObservable(() =>
                    ViewStackService.PushPage(new RedViewModel(ViewStackService)),
                    outputScheduler: RxApp.MainThreadScheduler);
            PopToRoot = ReactiveCommand
                .CreateFromObservable(() =>
                    ViewStackService.PopToRootPage(),
                    outputScheduler: RxApp.MainThreadScheduler);

            PopModal.Subscribe(x => Debug.WriteLine("PagePushed"));
            PopModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            PopPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            PushPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            PopToRoot.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }
    }
}
