using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Sextant;

namespace SextantSample.ViewModels
{
    public class GreenViewModel : ViewModelBase, INavigable
    {
        public GreenViewModel(IViewStackService viewStackService)
            : base(viewStackService)
        {
            OpenModal = ReactiveCommand
                .CreateFromObservable(() =>
                        this.ViewStackService.PushModal(new FirstModalViewModel(viewStackService), string.Empty, false),
                    outputScheduler: RxApp.MainThreadScheduler);
        }

        public ReactiveCommand<Unit, Unit> OpenModal { get; set; }

        public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) =>
            Observable.Return(Unit.Default);

        public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) =>
            Observable.Return(Unit.Default);

        public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) =>
            Observable.Return(Unit.Default);

        public string Id { get; } = string.Empty;
    }
}
