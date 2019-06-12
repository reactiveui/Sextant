using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Sextant;
using Splat;

namespace SextantSample.ViewModels
{
    public class HomeViewModel : ViewModelBase, IPageViewModel
    {
        public string Id => nameof(HomeViewModel);

        public ReactiveCommand<Unit, Unit> OpenModal
        {
            get;
            set;
        }

        public ReactiveCommand<Unit, Unit> PushPage
        {
            get;
            set;
        }

        public HomeViewModel()
            : base(Locator.Current.GetService<IViewStackService>())
        {
            OpenModal = ReactiveCommand
                .CreateFromObservable(() =>
                    this.ViewStackService.PushModal(new FirstModalViewModel(ViewStackService)),
                    outputScheduler: RxApp.MainThreadScheduler);

            PushPage = ReactiveCommand
                .CreateFromObservable(() =>
                    this.ViewStackService.PushPage(new RedViewModel(ViewStackService)),
                    outputScheduler: RxApp.MainThreadScheduler);

            OpenModal.Subscribe(x => Debug.WriteLine("PagePushed"));

            PushPage.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
            OpenModal.ThrownExceptions.Subscribe(error => Interactions.ErrorMessage.Handle(error).Subscribe());
        }
    }
}
