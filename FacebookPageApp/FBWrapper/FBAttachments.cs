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
    public class FBAttachments : FBBase
    {
        private string _description;
        private FBMedia _media;
        private FBTarget _target;
        private string _type;
        private string _url;

        public string Description { get => _description; set => Set(ref _description, value); }
        public FBMedia Media { get => _media; set => Set(ref _media, value); }
        public FBTarget Target { get => _target; set => Set(ref _target, value); }
        public string Type { get => _type; set => Set(ref _type, value); }
        public string Url { get => _url; set => Set(ref _url, value); }

        public static FBJsonClassFactory Factory = new FBJsonClassFactory((JsonText) => Json.ToObject<FBAttachments>(JsonText));

    }

    public class FBMedia : FBBase
    {
        private FBImage _image;
        public FBImage Image { get => _image; set => Set(ref _image, value); }
    }

    public class FBTarget : FBBase
    {
        private string _url;
        public string Url { get => _url; set => Set(ref _url, value); }

    }

    public class FBImage : FBBase
    {
        private int _height;
        private int _width;
        private string _src;

        public int Height { get => _height; set => Set(ref _height, value); }
        public int Width { get => _width; set => Set(ref _width, value); }
        public string Src { get => _src; set => Set(ref _src, value); }
    }
}
        

