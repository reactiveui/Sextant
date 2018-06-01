using NSubstitute;
using ReactiveUI;
using Sextant;
using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Sextant.Abstraction;
using Xamarin.Forms;

namespace Sextant.Tests.Navigation
{
    internal sealed class ViewStackServiceFixture
    {
        public IPageViewModel ModalViewModel { get; }

        public IPageViewModel PageViewModel { get; }

        public IView View { get; }

        public IViewStackService ViewStackService { get; }

        public ViewStackServiceFixture()
        {
            View = Substitute.For<IView>();
            View.PushPage(Arg.Any<IPageViewModel>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(Observable.Return(Unit.Default));
            ModalViewModel = Substitute.For<IPageViewModel>();
            PageViewModel = Substitute.For<IPageViewModel>();
            ViewStackService = new ViewStackService(View);
        }
    }
}