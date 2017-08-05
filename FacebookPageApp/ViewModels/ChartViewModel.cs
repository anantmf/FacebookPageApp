using System;
using System.Collections.ObjectModel;

using FacebookPageApp.Models;
using FacebookPageApp.Services;

using GalaSoft.MvvmLight;

namespace FacebookPageApp.ViewModels
{
    public class ChartViewModel : ViewModelBase
    {
        public ChartViewModel()
        {
        }

        public ObservableCollection<DataPoint> Source
        {
            get
            {
                // TODO WTS: Replace this with your actual data
                return SampleDataService.GetChartSampleData();
            }
        }
    }
}
