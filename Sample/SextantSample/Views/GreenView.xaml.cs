using System;

using ReactiveUI;
using ReactiveUI.XamForms;

using SextantSample.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SextantSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GreenView : ReactiveContentPage<GreenViewModel>
    {
        public GreenView()
        {
            InitializeComponent();

            this.BindCommand(ViewModel, x => x.OpenModal, x => x.Modal);
        }
    }
}

