using System;
using System.Collections.Generic;
using ReactiveUI;
using Sextant;

namespace SextantSample.ViewModels
{
    public class MainNavigationViewModel : ViewModelBase
    {
        public List<Func<IViewStackService, ITabViewModel>> TabViewModels
        {
            get => _tabViewModels;
            set => this.RaiseAndSetIfChanged(ref _tabViewModels, value);
        }

        private List<Func<IViewStackService, ITabViewModel>> _tabViewModels;

        public readonly List<IViewStackService> _tabStackServices = new List<IViewStackService>();

        public MainNavigationViewModel(IViewStackService viewStackService = null)
            : base(viewStackService)
        {
            TabViewModels = new List<Func<IViewStackService, ITabViewModel>>()
            {
                (customViewStack) =>
                {
                    _tabStackServices.Add(customViewStack);
                    return new HomeViewModel(customViewStack);
                },
                (customViewStack) =>
                {
                    _tabStackServices.Add(customViewStack);
                    return new RedViewModel(customViewStack);
                },
                (customViewStack) =>
                {
                    _tabStackServices.Add(customViewStack);
                    return new BlueViewModel(customViewStack);
                }
            };
        }
    }
}
