using ReactiveUI;
using ReactiveUI.XamForms;
using Sextant;
using SextantSample.ViewModels;

namespace SextantSample.Views
{
	public partial class RedView : ReactiveContentPage<RedViewModel>, IBaseNavigationPage<RedViewModel>
    {
        public RedView()
        {
            InitializeComponent();
			this.BindCommand(ViewModel, x => x.PopModal, x => x.PopModal);
        }
    }
}
