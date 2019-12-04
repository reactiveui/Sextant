using ReactiveUI;

namespace Sextant.IOS.Runner
{
    internal class PageUiViewController : IViewFor<PageViewModelMock>
    {
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (PageViewModelMock) value;
        }

        public PageViewModelMock ViewModel { get; set; } = new PageViewModelMock();
    }
}
