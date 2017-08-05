using FacebookPageApp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using winsdkfb;

namespace FacebookPageApp.FBWrapper
{
    public class FBPage : FBBase
    {
        private String _access_Token;
        private String _category;
        private String _name;
        private String _newPost;
        private ObservableCollection<String> _perms;
        private ObservableCollection<FBPagePost> _posts;
        public bool _isProcessing;
        public Boolean _postActionEnabled;


        public string Access_Token { get => _access_Token; set => _access_Token = value; }
        public string Category { get => _category; set => _category = value; }
        public string Name { get => _name; set => Set(ref _name, value); }
        public Boolean IsProcessing{ get => _isProcessing; set => Set(ref _isProcessing, value); }
        public String NewPost { get => _newPost;
            set
            {
                Set(ref _newPost, value);
                //PostActionEnabled = _newPost.Trim().Length > 0;
            }
        }
        public ObservableCollection<string> Perms { get => _perms; set => Set(ref _perms, value); }
        public ObservableCollection<FBPagePost> Posts { get => _posts; set => Set(ref _posts, value); }
        public Boolean PostActionEnabled { get => _postActionEnabled; set => Set(ref _postActionEnabled, value); }

        public static FBJsonClassFactory Factory = new FBJsonClassFactory((JsonText) => Json.ToObject<FBPage>(JsonText));


        public FBPage()
        {
            Posts = new ObservableCollection<FBPagePost>();
            PostActionEnabled = true;
        }

    }
}
