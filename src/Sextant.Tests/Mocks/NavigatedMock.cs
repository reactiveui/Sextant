using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;

namespace Sextant.Mocks.Mocks
{
    internal class NavigatedMock : INavigated
    {
        public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) => Observable.Return(Unit.Default);

        public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) => Observable.Return(Unit.Default);
    }
}
