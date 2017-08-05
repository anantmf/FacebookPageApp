using FacebookPageApp.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace FacebookPageApp.Views
{
    public sealed partial class MasterDetailPage : Page
    {
        private MasterDetailViewModel ViewModel
        {
            get { return DataContext as MasterDetailViewModel; }
        }

        public MasterDetailPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadDataAsync(WindowStates.CurrentState);
        }
    }
}
