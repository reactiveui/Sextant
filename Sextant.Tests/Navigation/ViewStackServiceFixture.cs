using System.Reactive;
using System.Reactive.Linq;
using NSubstitute;
using Sextant.Abstraction;

namespace Sextant.Tests.Navigation
{
    internal sealed class ViewStackServiceFixture
    {
        public IView View { get; }

        public IViewStackService ViewStackService { get; }

        public ViewStackServiceFixture()
        {
            View = Substitute.For<IView>();
            View.PushPage(Arg.Any<IPageViewModel>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(Observable.Return(Unit.Default));
            ViewStackService = new ViewStackService(View);
        }
    }
}