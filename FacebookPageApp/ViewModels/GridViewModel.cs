using System.Collections.ObjectModel;

using FacebookPageApp.Models;
using FacebookPageApp.Services;

using GalaSoft.MvvmLight;

namespace FacebookPageApp.ViewModels
{
    public class GridViewModel : ViewModelBase
    {
        public ObservableCollection<Order> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetGridSampleData();
            }
        }
    }
}
