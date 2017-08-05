using FacebookPageApp.FBWrapper;
using FacebookPageApp.ViewModels;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace FacebookPageApp.Views
{
    public sealed partial class PostPage : Page
    {
        private PostViewModel ViewModel
        {
            get { return DataContext as PostViewModel; }
        }

        public PostPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (this.ViewModel == null || this.ViewModel.Page == null)
                this.DataContext = new PostViewModel(e.Parameter as FBPage, this);
            await this.ViewModel.RefreshFeeds();

        }

        private void Grid_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);
        }

        private void Grid_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as FrameworkElement);

        }
    }

    public class PublishedValueConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var checkLogo = new BitmapImage(new Uri("ms-appx:///Assets/check.png", UriKind.RelativeOrAbsolute));
            var crossLogo = new BitmapImage(new Uri("ms-appx:///Assets/cross.png", UriKind.RelativeOrAbsolute));

            if ((Boolean)value)
                return "Yes";
            return "No";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var valString = value.ToString();
            if (valString == "Yes")
                return true;
            else
                return false;
        }
    }

    public class PublishedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ((Boolean)value)
                return new SolidColorBrush(Colors.Green);
            return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    
}
