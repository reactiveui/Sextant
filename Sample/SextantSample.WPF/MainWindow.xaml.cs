using Xamarin.Forms;

namespace SextantSample.WPF
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Forms.Init();
            LoadApplication(new SextantSample.App());
        }
    }
}
