using ReactiveUI;
using Sextant.Abstraction;

namespace Sextant.Tests
{
    internal class PageMock<T> : IViewFor<T>
        where T : class, IPageViewModel, new()
    {
        public PageMock() { ViewModel = new T(); }

        public T ViewModel { get; set; }
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (T)value;
        }
    }
}