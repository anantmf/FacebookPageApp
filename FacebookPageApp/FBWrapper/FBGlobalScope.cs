using FacebookPageApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using winsdkfb;
using winsdkfb.Graph;

namespace FacebookPageApp.FBWrapper
{
    public static class FBGlobalScope
    {

        public static FBSession Session { get; set; }
        public static List<String> PermissionList = new List<string>()
        {
            "public_profile",
            "user_friends",
            "user_likes",
            "manage_pages",
            "user_location",
            "user_photos",
            "publish_actions",
            "publish_pages",
            "user_posts",
            "user_likes",
            "read_insights"
        };
        public static FBPermissions Permissions = new FBPermissions(PermissionList);

        public static bool IsLoggedIn { get; set; }



        public static FBUser User { get; set; }

        public static List<FBPage> Pages { get; set; }

        static FBGlobalScope()
        {
            Initialize();
        }

        public static void Initialize()
        {
            string SID = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();
            Pages = new List<FBPage>();
            Session = FBSession.ActiveSession;
            //Session.FBAppId = "178502609357560";
            Session.FBAppId = "814971745337446";
            Session.WinAppId = "s-1-15-2-1712479430-1519030426-2536652871-3012372571-2097559892-3697040793-1951306893";

            IsLoggedIn = false;

        }

        internal static async Task PopulateUserData()
        {
            string graphPath = "me";

            var res = await new FBSingleValue(graphPath, null, FBUser.Factory).GetAsync();
            if (res.Succeeded)
            {
                User = (FBUser)res.Object;
            }
            else
            {
                FBErrorHandler.HandleError(res);
            }
            User.AccessToken = FBSession.ActiveSession.AccessTokenData.AccessToken;

            await PopulateUserPages();

        }

        internal static async Task PopulateUserPages()
        {

            Pages.Clear();
            String graphPath = "me/accounts";

            FBPaginatedArray fbPages = new FBPaginatedArray(graphPath, null, FBPage.Factory);
            FBResult result = null;
            do
            {
                if (result == null)
                    result = await fbPages.FirstAsync();
                else
                    result = await fbPages.NextAsync();
                if (result.Succeeded)
                {
                    IReadOnlyList<object> pages = (IReadOnlyList<object>)result.Object;
                    foreach (var p in pages)
                    {
                        FBPage page = (FBPage)p;
                        if (page != null)
                        {
                            Pages.Add(page);
                        }
                    }
                }
                else
                {
                    FBErrorHandler.HandleError(result);
                }

            } while (fbPages.HasNext);

        }
    }
}
