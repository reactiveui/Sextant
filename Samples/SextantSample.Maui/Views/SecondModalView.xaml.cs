﻿namespace SextantSample.Maui.Views;

public partial class SecondModalView : ReactiveContentPage<SecondModalViewModel>
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
                await DisplayAlert("Error", x.Input.Message, "Done");
                x.SetOutput(true);
            });
    }
}
