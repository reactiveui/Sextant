using System.Reactive.Disposables;

namespace SextantSample.Maui.Views;

public partial class FirstModalView : ReactiveContentPage<FirstModalViewModel>
{
		public FirstModalView()
    {
			InitializeComponent();

        this.WhenActivated(disposables =>
        {
            this.BindCommand(ViewModel, vm => vm.OpenModal, v => v.OpenSecondModal)
                .DisposeWith(disposables);
            this.BindCommand(ViewModel, vm => vm.PopModal, v => v.PopModal)
                .DisposeWith(disposables);
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
