using FacebookPageApp.Helpers;
using FacebookPageApp.ViewModels;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation.Collections;
using winsdkfb;

namespace FacebookPageApp.FBWrapper
{
    public class FBPagePost : FBBase
    {
        private string _message;
        private string _editedMessage;
        private Boolean _is_published;
        private DateTime created_Time;
        private string _caption;
        private string _link;
        private string _desription;
        private long _scheduled_publish_time;
        private long _lifetimeImpressions;
        private long _lifetimeUniqueUsers;
        private bool _isFlyoutOpen;
        private string _imageSrc;
        public ICommand DeletePostCommand { get; private set; }
        public ICommand EditPostCommand { get; private set; }
        public ICommand CancelEditCommand { get; private set; }
        public ICommand FlyoutOpening { get; private set; }
        

        public bool IsFlyoutOpen { get => _isFlyoutOpen; set => Set(ref _isFlyoutOpen, value); }
        public string Message { get => _message; set => Set(ref _message, value); }
        public string EditedMessage { get => _editedMessage; set => Set(ref _editedMessage, value); }

        public bool Is_published { get => _is_published; set => Set(ref _is_published, value); }
        public DateTime Created_Time { get => created_Time; set => Set(ref created_Time, value); }
        public string Caption { get => _caption; set => Set(ref _caption, value); }
        public string Link { get => _link; set => Set(ref _link, value); }
        public string Desription { get => _desription; set => Set(ref _desription, value); }
        public long Scheduled_publish_time { get => _scheduled_publish_time; set => Set(ref _scheduled_publish_time, value); }
        public DateTime ScheduledPublishDateTime { get => new DateTime(1970, 1, 1).AddSeconds(Scheduled_publish_time).ToLocalTime(); }
        public String ScheduledPublishDateTimeFormatted
        {
            get
            {
                if (Scheduled_publish_time != 0)
                    return $"Scheduled Time: {ScheduledPublishDateTime.ToString("dd-MMM-yyyy hh:mm")}";
                return String.Empty;
            }
        }
        public long LifetimeImpressions { get => _lifetimeImpressions; set => Set(ref _lifetimeImpressions, value); }
        public long LifetimeUniqueUsers { get => _lifetimeUniqueUsers; set => Set(ref _lifetimeUniqueUsers, value); }

        public string ImageSrc { get => _imageSrc; set => Set(ref _imageSrc, value); }

        public static FBJsonClassFactory Factory = new FBJsonClassFactory((JsonText) => Json.ToObject<FBPagePost>(JsonText));

        public FBPagePost()
        {
            DeletePostCommand = new RelayCommand<PostViewModel>(OnDelete);
            EditPostCommand = new RelayCommand<PostViewModel>(OnEdit);
            CancelEditCommand = new RelayCommand<PostViewModel>((x) =>
            {
                this.IsFlyoutOpen = false;
            });
            FlyoutOpening = new RelayCommand<EventArgs>(x =>
            {
                EditedMessage = Message;
            });
            ImageSrc = "ms-appx:///Assets/blank.png";
        }

        private async void OnDelete(PostViewModel viewModel)
        {
            await viewModel.DeletePost(this);
        }

        private async void OnEdit(PostViewModel viewModel)
        {
            await viewModel.EditPost(this);
        }
    }
}