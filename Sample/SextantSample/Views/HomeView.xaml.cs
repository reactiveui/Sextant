using ReactiveUI;
using ReactiveUI.XamForms;
using Sextant;
using SextantSample.Core;
using SextantSample.Core.ViewModels;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Xamarin.Forms;

namespace SextantSample.Views
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
