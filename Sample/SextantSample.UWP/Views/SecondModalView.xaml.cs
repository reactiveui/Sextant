using System;
using ReactiveUI;
using SextantSample.Core;
using SextantSample.Core.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SextantSample.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SecondModalView : Page, IViewFor<SecondModalViewModel>
    {
        public SecondModalView()
        {
            this.InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.BindCommand(ViewModel, x => x.PushPage, x => x.PushPage));
                d(this.BindCommand(ViewModel, x => x.PopModal, x => x.PopModal));

                d(Interactions
                    .ErrorMessage
                    .RegisterHandler(async x =>
                    {
                        var dialog = new Windows.UI.Popups.MessageDialog(x.Input.Message, "Error");
                        dialog.Commands.Add(new Windows.UI.Popups.UICommand("Done"));
                        _ = await dialog.ShowAsync();
                        x.SetOutput(true);
                    }));
            });
           
        }


        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
           .Register(nameof(ViewModel), typeof(SecondModalViewModel), typeof(SecondModalView), null);

        public SecondModalViewModel ViewModel
        {
            get => (SecondModalViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel { get => ViewModel; set => ViewModel = (SecondModalViewModel)value; }
    }
}
