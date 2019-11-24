using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Sextant.Mocks
{
    /// <summary>
    /// Null view model.
    /// </summary>
    public class NullViewModelMock : INavigable
    {
        private ISubject<Unit> _navigatedTo;
        private ISubject<Unit> _navigatingTo;
        private ISubject<Unit> _navigatedFrom;

        /// <summary>
        /// Initializes a new instance of the <see cref="NullViewModelMock"/> class.
        /// </summary>
        public NullViewModelMock()
        {
            _navigatedTo = new Subject<Unit>();
            _navigatedFrom = new Subject<Unit>();
            _navigatingTo = new Subject<Unit>();
        }

        /// <summary>
        /// Gets the ID of the page.
        /// </summary>
        public string Id => nameof(NullViewModelMock);

        /// <inheritdoc/>
        public IObservable<Unit> WhenNavigatedTo(INavigationParameter parameter) =>
            Observable.Return(Unit.Default).Do(_ => _navigatedTo.OnNext(Unit.Default));

        /// <inheritdoc/>
        public IObservable<Unit> WhenNavigatedFrom(INavigationParameter parameter) =>
            Observable.Return(Unit.Default).Do(_ => _navigatedFrom.OnNext(Unit.Default));

        /// <inheritdoc/>
        public IObservable<Unit> WhenNavigatingTo(INavigationParameter parameter) =>
            Observable.Return(Unit.Default).Do(_ => _navigatingTo.OnNext(Unit.Default));
    }
}
