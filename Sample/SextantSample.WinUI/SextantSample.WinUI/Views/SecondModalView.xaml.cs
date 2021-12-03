using ReactiveUI;
using SextantSample.ViewModels;

namespace SextantSample.WinUI.Views
{
    public class ReactivePageSecondModalView: ReactivePage<SecondModalViewModel>
    {

    }

    public partial class SecondModalView
    {
		public SecondModalView()
        {
            InitializeComponent();
			this.BindCommand(ViewModel, x => x.PushPage, x => x.PushPage);
            this.BindCommand(ViewModel, x => x.PopModal, x => x.PopModal);

            Interactions
                .ErrorMessage
                .RegisterHandler(async x =>
                {
                    await Alerts.DisplayAlert("Error", x.Input.Message, "Done");
                    x.SetOutput(true);
                });
        }
    }
}
