using System.Reactive.Disposables;

namespace SextantSample.Maui.Views
{
    public partial class HomeView : ReactiveContentPage<HomeViewModel>
    {
		public HomeView()
        {
            InitializeComponent();

			this.WhenActivated(disposables =>
            {
				this.BindCommand(ViewModel, x => x.OpenModal, x => x.FirstModalButton).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.PushPage, x => x.PushPage).DisposeWith(disposables);
                this.BindCommand(ViewModel, x => x.PushGenericPage, x => x.PushGenericPage).DisposeWith(disposables);
            });

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
