using System;
using System.Windows.Input;

using FacebookPageApp.Models;
using FacebookPageApp.Services;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

using Windows.UI.Xaml;

namespace FacebookPageApp.ViewModels
{
    public class MasterDetailDetailViewModel : ViewModelBase
    {

        public NavigationServiceEx NavigationService
        {
            get
            {
                return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<NavigationServiceEx>();
            }
        }
        const string NarrowStateName = "NarrowState";
        const string WideStateName = "WideState";

        public ICommand StateChangedCommand { get; private set; }

        private Order _item;
        public Order Item
        {
            get { return _item; }
            set { Set(ref _item, value); }
        }

        public MasterDetailDetailViewModel()
        {
            StateChangedCommand = new RelayCommand<VisualStateChangedEventArgs>(OnStateChanged);
        }
        
        private void OnStateChanged(VisualStateChangedEventArgs args)
        {
            if (args.OldState.Name == NarrowStateName && args.NewState.Name == WideStateName)
            {
                NavigationService.GoBack();
            }
        }
    }
}
