using ReactiveUI;
using ReactiveUI.XamForms;
using Sextant;
using SextantSample.ViewModels;

namespace SextantSample.Views
{
	public partial class HomeView : ReactiveContentPage<HomeViewModel>, IBaseNavigationPage<HomeViewModel>
    {
		public HomeView()
        {
            InitializeComponent();

			this.BindCommand(ViewModel, x => x.OpenModal, x => x.FirstModalButton);
        }
    }
}
