using ReactiveUI;

using SextantSample.ViewModels;

namespace SextantSample.Views
{
    public partial class FirstModalView
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
