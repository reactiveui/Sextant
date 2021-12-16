using Microsoft.Maui.Controls.Xaml;
using ReactiveUI;
using ReactiveUI.Maui;
using SextantSample.ViewModels;

namespace SextantSample.Maui.Views
{
    public partial class GreenView : ReactiveContentPage<GreenViewModel>
    {
        public GreenView()
        {
            InitializeComponent();

            this.BindCommand(ViewModel, x => x.OpenModal, x => x.Modal);
        }
    }
}

