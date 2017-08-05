using FacebookPageApp.Services;
using FacebookPageApp.Views;

using GalaSoft.MvvmLight.Ioc;

using Microsoft.Practices.ServiceLocation;

namespace FacebookPageApp.ViewModels
{
    public class ViewModelLocator
    {
        NavigationServiceEx _navigationService = new NavigationServiceEx();

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => _navigationService);
            SimpleIoc.Default.Register<ShellViewModel>();
            Register<MainViewModel, MainPage>();
            Register<WebViewViewModel, WebViewPage>();
            Register<MasterDetailViewModel, MasterDetailPage>();
            Register<MasterDetailDetailViewModel, MasterDetailDetailPage>();
            Register<TabbedViewModel, TabbedPage>();
            Register<GridViewModel, GridPage>();
            Register<ChartViewModel, ChartPage>();
            Register<SettingsViewModel, SettingsPage>();
            Register<LoginViewModel, LoginPage>();
            Register<PostViewModel, PostPage>();
        }

        public LoginViewModel LoginViewModel => ServiceLocator.Current.GetInstance<LoginViewModel>();

        public PostViewModel PostViewModel => ServiceLocator.Current.GetInstance<PostViewModel>();

        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public ChartViewModel ChartViewModel => ServiceLocator.Current.GetInstance<ChartViewModel>();

        public GridViewModel GridViewModel => ServiceLocator.Current.GetInstance<GridViewModel>();

        public TabbedViewModel TabbedViewModel => ServiceLocator.Current.GetInstance<TabbedViewModel>();

        public MasterDetailDetailViewModel MasterDetailDetailViewModel => ServiceLocator.Current.GetInstance<MasterDetailDetailViewModel>();

        public MasterDetailViewModel MasterDetailViewModel => ServiceLocator.Current.GetInstance<MasterDetailViewModel>();

        public WebViewViewModel WebViewViewModel => ServiceLocator.Current.GetInstance<WebViewViewModel>();

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public void Register<VM, V>() where VM : class
        {
            SimpleIoc.Default.Register<VM>();
            
            _navigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
