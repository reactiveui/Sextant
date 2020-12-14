using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Sextant;

namespace SextantSample.ViewModels
{
    public class GreenViewModel : ViewModelBase, INavigable, IDestructible
    {
        public GreenViewModel(IViewStackService viewStackService)
            : base(viewStackService) =>
            OpenModal = ReactiveCommand
                .CreateFromObservable(() =>
                        ViewStackService.PushModal(new FirstModalViewModel(viewStackService), string.Empty, false),
                    outputScheduler: RxApp.MainThreadScheduler);

        public override string Id { get; } = string.Empty;

        public ReactiveCommand<Unit, Unit> OpenModal { get; set; }

        public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) =>
            Observable.Return(Unit.Default);

        public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) =>
            Observable.Return(Unit.Default);

        public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) =>
            Observable.Return(Unit.Default);

        public void Destroy() => Debug.WriteLine($"Destroy: {nameof(GreenViewModel)}");
    }
}
