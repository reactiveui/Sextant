using System;
using ReactiveUI;
using SextantSample.ViewModels;

namespace SextantSample.WinUI.Views
{
    public class ReactivePageGreenView: ReactivePage<GreenViewModel>
    {

    }

    public partial class GreenView
    {
        public GreenView()
        {
            InitializeComponent();

            this.BindCommand(ViewModel, x => x.OpenModal, x => x.Modal);
        }
    }
}

