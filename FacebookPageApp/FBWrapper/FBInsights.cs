using FacebookPageApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using winsdkfb;

namespace FacebookPageApp.FBWrapper
{
    public class FBInsights : FBBase
    {
        private string _title;
        private string _description;
        private string _name;
        private string _period;
        private List<FBInsightsValue> _values;

        public List<FBInsightsValue> Values { get => _values; set => _values = value; }
        public string Period { get => _period; set => _period = value; }
        public string Name { get => _name; set => _name = value; }
        public string Description { get => _description; set => _description = value; }
        public string Title { get => _title; set => _title = value; }

        public static FBJsonClassFactory Factory = new FBJsonClassFactory((JsonText) => Json.ToObject<FBInsights>(JsonText));


    }

    public class FBInsightsValue : FBBase
    {
        private long _value;

        public long Value { get => _value; set => _value = value; }
    }
}
