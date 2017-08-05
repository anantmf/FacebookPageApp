using FacebookPageApp.Models;
using FacebookPageApp.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FacebookPageApp.Views
{
    public sealed partial class MasterDetailDetailPage : Page
    {
        private MasterDetailDetailViewModel ViewModel
        {
            get { return DataContext as MasterDetailDetailViewModel; }
        }

        public MasterDetailDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.Item = e.Parameter as Order;
        }
    }
}
