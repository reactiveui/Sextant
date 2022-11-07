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

