using ReactiveUI;
using ReactiveUI.XamForms;
using Sextant;
using SextantSample.ViewModels;
using Xamarin.Forms;

namespace SextantSample.Views
{
    public partial class FirstModalView : ReactiveContentPage<FirstModalViewModel>
    {
		public FirstModalView()
        {
			InitializeComponent();
			this.BindCommand(ViewModel, x => x.OpenModal, x => x.OpenSecondModal);
			this.BindCommand(ViewModel, x => x.PopModal, x => x.PopModal);

            Interactions
                .ErrorMessage
                .RegisterHandler(async x =>
                {
                    await DisplayAlert("Error", x.Input.Message, "Done");
                    x.SetOutput(true);
                });
        }
    }
}
