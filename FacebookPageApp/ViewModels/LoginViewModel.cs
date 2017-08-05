using System;
using System.Windows.Input;

using FacebookPageApp.Models;
using FacebookPageApp.Services;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Windows.UI.Xaml;
using FacebookPageApp.FBWrapper;
using winsdkfb;
using Windows.UI.Xaml.Controls;
using FacebookPageApp.Views;

namespace FacebookPageApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public ICommand LoginLogoutCommand { get; private set; }

        internal void Initialize()
        {
            OnLoginLogout(new EventArgs());
        }

        public LoginViewModel()
        {
            LoginLogoutCommand = new RelayCommand<EventArgs>(OnLoginLogout);
        }

        public NavigationServiceEx NavigationService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<NavigationServiceEx>();
            }
        }

        private async void OnLoginLogout(EventArgs obj)
        {

            if (FBGlobalScope.IsLoggedIn)
            {
                await FBGlobalScope.Session.LogoutAsync();
                FBGlobalScope.IsLoggedIn = false;
                NavigationService.ShellViewModel.PrimaryItems[0].Label = $"Login";
                var loginItem = NavigationService.ShellViewModel.PrimaryItems[0];
                NavigationService.ShellViewModel.PrimaryItems.Clear();
                NavigationService.ShellViewModel.PrimaryItems.Add(loginItem);

            }
            else
            {
                int count = 0;
                FBResult result = null;
                do
                {
                    FBGlobalScope.Initialize();
                    result = await FBGlobalScope.Session.LoginAsync(FBGlobalScope.Permissions, SessionLoginBehavior.WebView);
                    if (result.Succeeded)
                    {
                        FBGlobalScope.IsLoggedIn = true;
                        await FBGlobalScope.PopulateUserData();
                        NavigationService.ShellViewModel.PrimaryItems[0].Label = $"Logged in as {FBGlobalScope.User.Name}";

                        foreach (var page in FBGlobalScope.Pages)
                        {
                            string key = $"{typeof(PostViewModel).FullName}{page.Id}";
                            NavigationService.Configure(key, typeof(PostPage));
                            NavigationService.ShellViewModel.PrimaryItems.Add(new ShellNavigationItem(
                                page.Name,
                                Symbol.Page2,
                                typeof(PostViewModel).FullName,
                                page
                                ));
                        }
                        if(FBGlobalScope.Pages.Count > 0)
                            NavigationService.Navigate($"{typeof(PostViewModel).FullName}{FBGlobalScope.Pages[0].Id}", FBGlobalScope.Pages[0]);

                    }
                    else
                    {
                        await FBGlobalScope.Session.LogoutAsync();
                        FBGlobalScope.IsLoggedIn = false;
                        NavigationService.ShellViewModel.PrimaryItems[0].Label = $"Login";
                        var loginItem = NavigationService.ShellViewModel.PrimaryItems[0];
                        NavigationService.ShellViewModel.PrimaryItems.Clear();
                        NavigationService.ShellViewModel.PrimaryItems.Add(loginItem);
                    }
                    count++;
                }
                while (!result.Succeeded && count < 3);
            }
        }
    }
}
