using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

