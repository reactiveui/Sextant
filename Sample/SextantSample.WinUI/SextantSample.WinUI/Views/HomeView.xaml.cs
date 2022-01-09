using System.Reactive;
using System.Reactive.Disposables;
using ReactiveUI;
using SextantSample.ViewModels;

namespace SextantSample.WinUI.Views
{
    public class ReactivePageHomeView: ReactivePage<HomeViewModel>
    {

    }

    public partial class HomeView
    {
		public HomeView()
        {
            InitializeComponent();

			this.WhenActivated(disposables =>
            {
				this.BindCommand(ViewModel, x => x.OpenModal, x => x.FirstModalButton).DisposeWith<IReactiveBinding<HomeView, ReactiveCommand<Unit, Unit>>>(disposables);
                this.BindCommand(ViewModel, x => x.PushPage, x => x.PushPage).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.PushGenericPage, x => x.PushGenericPage).DisposeWith(disposables);
            });

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
