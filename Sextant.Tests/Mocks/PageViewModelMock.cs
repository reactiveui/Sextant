using Sextant.Abstraction;

namespace Sextant.Tests
{
    internal class PageViewModelMock :  IPageViewModel
    {
        private readonly string _id;

        public PageViewModelMock(string id = null)
        {
            _id = id;
        }
        public string Id => _id ?? nameof(PageViewModelMock);
    }
}