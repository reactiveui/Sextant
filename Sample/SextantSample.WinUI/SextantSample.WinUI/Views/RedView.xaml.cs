using ReactiveUI;
using SextantSample.ViewModels;

namespace SextantSample.WinUI.Views
{
    public class ReactivePageRedView: ReactivePage<RedViewModel>
    {

    }

    public partial class RedView
    {
        public RedView()
        {
            InitializeComponent();
			this.BindCommand(ViewModel, x => x.PopModal, x => x.PopModal);
            this.BindCommand(ViewModel, x => x.PushPage, x => x.PushPage);
            this.BindCommand(ViewModel, x => x.PopPage, x => x.PopPage);
            this.BindCommand(ViewModel, x => x.PopToRoot, x => x.PopToRoot);

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
