using FacebookPageApp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace FacebookPageApp.Views
{
    public sealed partial class LoginPage : Page
    {
        private LoginViewModel ViewModel
        {
            get { return DataContext as LoginViewModel; }
        }

        public LoginPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize();
        }
    }
}
