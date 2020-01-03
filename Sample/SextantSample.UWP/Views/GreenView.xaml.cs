using ReactiveUI;
using SextantSample.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace SextantSample.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GreenView : Page, IViewFor<GreenViewModel>
    {
        public GreenView()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
           .Register(nameof(ViewModel), typeof(GreenViewModel), typeof(GreenView), null);

        public GreenViewModel ViewModel
        {
            get => (GreenViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel { get => ViewModel; set => ViewModel = (GreenViewModel)value; }
    }
}
