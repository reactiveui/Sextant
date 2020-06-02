using System.Reactive;
using ReactiveUI;
using Sextant;
using System;
using System.Diagnostics;
using Splat;

namespace SextantSample.ViewModels
{
    public class FirstModalViewModel : ViewModelBase, IDestructible
    {
        public ReactiveCommand<Unit, Unit> OpenModal { get; set; }

        public ReactiveCommand<Unit, Unit> PopModal { get; set; }

        public override string Id => nameof(FirstModalViewModel);

        public FirstModalViewModel(IViewStackService viewStackService) : base(viewStackService)
        {
            OpenModal = ReactiveCommand
                        .CreateFromObservable(() =>
                            ViewStackService.PushModal(new SecondModalViewModel(viewStackService)),
                            outputScheduler: RxApp.MainThreadScheduler);

            PopModal = ReactiveCommand
                        .CreateFromObservable(() =>
                            ViewStackService.PopModal(),
                            outputScheduler: RxApp.MainThreadScheduler);

            OpenModal.Subscribe(x => Debug.WriteLine("PagePushed"));
            PopModal.Subscribe(x => Debug.WriteLine("PagePopped"));
            PopModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }


        public void Destroy()
        {
            Debug.WriteLine($"Destroy: {nameof(FirstModalViewModel)}");
        }
    }
}
