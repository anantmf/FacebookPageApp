using FacebookPageApp.Services;
using FacebookPageApp.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FacebookPageApp.Views
{
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel { get { return DataContext as ShellViewModel; } }

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame);
        }
    }
}
