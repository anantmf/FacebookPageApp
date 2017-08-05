using FacebookPageApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using winsdkfb;

namespace FacebookPageApp.FBWrapper
{
    public class FBUser : FBBase
    {
        public String AccessToken { get; set; }
        private string _name;

        public string Name { get => _name; set => Set(ref _name, value); }

        public static FBJsonClassFactory Factory = new FBJsonClassFactory(JsonText => Json.ToObject<FBUser>(JsonText));
    }
}
