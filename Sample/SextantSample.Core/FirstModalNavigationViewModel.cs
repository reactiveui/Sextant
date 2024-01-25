using Sextant;

namespace SextantSample.ViewModels
{
    public class FirstModalNavigationViewModel(IViewStackService viewStackService) : ViewModelBase(viewStackService)
    {
        public override string Id => nameof(FirstModalNavigationViewModel);
    }
}
