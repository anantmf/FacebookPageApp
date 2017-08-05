using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FacebookPageApp.FBWrapper
{
    public class FBBase : ViewModelBase
    {
        public String _id;

        public String Id { get => _id; set => Set(ref _id, value); }
    }
}
