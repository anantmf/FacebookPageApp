using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using winsdkfb;

namespace FacebookPageApp.Helpers
{
    public static class FBErrorHandler
    {
        public static async void HandleError(FBResult result)
        {
            try
            {
                var messageDialog = new MessageDialog($"Issue with connecting to Facebook service : {result.ErrorInfo.Message}");
                await messageDialog.ShowAsync();
            }
            catch(Exception ex)
            { }

        }
    }
}
