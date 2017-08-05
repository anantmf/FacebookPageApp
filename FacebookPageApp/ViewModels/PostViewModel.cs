using System;

using GalaSoft.MvvmLight;
using FacebookPageApp.FBWrapper;
using GalaSoft.MvvmLight.Ioc;
using winsdkfb;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using System.Collections.Generic;
using FacebookPageApp.Helpers;
using System.Linq;
using Windows.UI.Xaml.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace FacebookPageApp.ViewModels
{
    public class PostViewModel : ViewModelBase
    {
        public ICommand PublishCommand { get; private set; }
        public ICommand NoPublishCommand { get; private set; }
        public ICommand SchedulePostCommand { get; private set; }

        private DateTime _scheduledDate;
        private DateTime _scheduledTime;
        public DateTime ScheduledDate { get => _scheduledDate; set => Set(ref _scheduledDate, value); }
        public DateTime ScheduledTime { get => _scheduledTime; set => Set(ref _scheduledTime, value); }

        private FBPage _page;

        private Page _view;

        public FBPage Page { get => _page; set => Set(ref _page, value); }

        [PreferredConstructor]
        public PostViewModel()
        {
        }

        public PostViewModel(FBPage page, Page view)
        {
            _view = view;
            this.Page = page;

            this.ScheduledDate = new DateTime( DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            
            this.ScheduledTime = DateTime.Now;

            PublishCommand = new RelayCommand<EventArgs>(OnPublish);
            NoPublishCommand = new RelayCommand<EventArgs>(OnNoPublish);
            SchedulePostCommand = new RelayCommand<EventArgs>(OnSchedulePost);
        }

        private async void OnPublish(EventArgs obj)
        {
            await Publish();
        }
        private async void OnNoPublish(EventArgs obj)
        {
            await Publish(published: false);
        }
        private async void OnSchedulePost(EventArgs obj)
        {
            var ts = new TimeSpan(ScheduledTime.Hour, ScheduledTime.Minute, 0);
            var time = GetUnixTimeStamp(ScheduledDate.Add(ts)).ToString();

            await Publish(published: false, scheduledTime: time);
        }

        internal async Task EditPost(FBPagePost post)
        {
            Page.IsProcessing = true;

            FBSession.ActiveSession.AccessTokenData = new FBAccessTokenData(Page.Access_Token, DateTimeOffset.MaxValue);
            string path = "/" + post.Id + "";

            var factory = new FBJsonClassFactory(s => s);
            PropertySet parameters = new PropertySet();
            post.EditedMessage = post.EditedMessage.Replace("\r\n", "\r");
            post.EditedMessage = post.EditedMessage.Replace("\r", "\r\n");
            parameters.Add("message", post.Message);

            var singleValue = new winsdkfb.Graph.FBSingleValue(path, parameters, factory);
            try
            {
                var result = await singleValue.PostAsync();
                if (result.Succeeded)
                {
                    post.Message = post.EditedMessage;
                }
                else
                {
                    FBErrorHandler.HandleError(result);
                }
            }
            catch (Exception ex)
            {
                FBErrorHandler.HandleError(new FBResult(ex));
            }
            FBSession.ActiveSession.AccessTokenData = new FBAccessTokenData(FBGlobalScope.User.AccessToken, DateTimeOffset.MaxValue);

            Page.IsProcessing = false;
            post.IsFlyoutOpen = false;
        }


        internal async Task DeletePost(FBPagePost post)
        {
            Page.IsProcessing = true;

            FBSession.ActiveSession.AccessTokenData = new FBAccessTokenData(Page.Access_Token, DateTimeOffset.MaxValue);
            string path = "/" + post.Id + "";

            var factory = new FBJsonClassFactory(s => s);

            var singleValue = new winsdkfb.Graph.FBSingleValue(path, null, factory);
            try
            {
                var result = await singleValue.DeleteAsync();
                if (result.Succeeded)
                {
                    this.Page.Posts.Remove(post);
                }
                else
                {
                    FBErrorHandler.HandleError(result);
                }
            }
            catch (Exception ex)
            {
                FBErrorHandler.HandleError(new FBResult(ex));
            }
            FBSession.ActiveSession.AccessTokenData = new FBAccessTokenData(FBGlobalScope.User.AccessToken, DateTimeOffset.MaxValue);

            Page.IsProcessing = false;
        }

        public async Task Publish(bool published = true, string scheduledTime = null)
        {
            Page.IsProcessing = true;

            FBSession.ActiveSession.AccessTokenData = new FBAccessTokenData(Page.Access_Token, DateTimeOffset.MaxValue);
            string path = "/" + Page.Id + "/feed";

            PropertySet parameters = new PropertySet();
            Page.NewPost = Page.NewPost.Replace("\r", "\r\n");
            parameters.Add("message", Page.NewPost);
            parameters.Add("published", published.ToString().ToLower());
            if (scheduledTime != null)
                parameters.Add("scheduled_publish_time", scheduledTime);

            var factory = new FBJsonClassFactory(s => s);

            var singleValue = new winsdkfb.Graph.FBSingleValue(path, parameters, factory);
            try
            {
                var result = await singleValue.PostAsync();
                if (result.Succeeded)
                {
                    Page.NewPost = String.Empty;
                    await RefreshFeeds();
                }
                else
                {
                    FBErrorHandler.HandleError(result);
                }
            }
            catch(Exception ex)
            {
                FBErrorHandler.HandleError(new FBResult(ex));
            }
            FBSession.ActiveSession.AccessTokenData = new FBAccessTokenData(FBGlobalScope.User.AccessToken, DateTimeOffset.MaxValue);

            Page.IsProcessing = false;
        }

        public async Task RefreshFeeds()
        {
            Page.IsProcessing = true;
            FBSession.ActiveSession.AccessTokenData = new FBAccessTokenData(Page.Access_Token, DateTimeOffset.MaxValue);

            String graphPath = $"{Page.Id}/promotable_posts";
            Page.Posts.Clear();
            PropertySet ps = new PropertySet();
            ps.Add(new KeyValuePair<String, object>("fields", "message,is_published,created_time,scheduled_publish_time,caption,link,description"));

            winsdkfb.Graph.FBPaginatedArray fbPosts = new winsdkfb.Graph.FBPaginatedArray(graphPath, ps, FBWrapper.FBPagePost.Factory);
            FBResult result = null;
            do
            {
                if (result == null)
                    result = await fbPosts.FirstAsync();
                else
                    result = await fbPosts.NextAsync();
                if (result.Succeeded)
                {
                    IReadOnlyList<object> posts = (IReadOnlyList<object>)result.Object;
                    foreach (var p in posts)
                    {
                        FBPagePost post = (FBPagePost)p;
                        if (post != null)
                        {
                            Page.Posts.Add(post);
                        }
                    }
                }
                else
                {
                    FBErrorHandler.HandleError(result);
                }

            } while (fbPosts.HasNext);
            FBSession.ActiveSession.AccessTokenData = new FBAccessTokenData(FBGlobalScope.User.AccessToken, DateTimeOffset.MaxValue);
            Page.IsProcessing = false;
            await GetAllMetricsForPosts();

        }

        public async Task GetAllMetricsForPosts()
        {
            FBSession.ActiveSession.AccessTokenData = new FBAccessTokenData(Page.Access_Token, DateTimeOffset.MaxValue);
            string path = "/" + Page.Id + "/feed";

            //await Task.Factory.StartNew(() => {
            Parallel.ForEach(Page.Posts, async p =>
            {
                await GetAttachmentsForPost(p);
                await GetInsightsForPost(p);

            });

            FBSession.ActiveSession.AccessTokenData = new FBAccessTokenData(FBGlobalScope.User.AccessToken, DateTimeOffset.MaxValue);

        }

        private async Task GetAttachmentsForPost(FBPagePost p)
        {
            return;
            //string graphPath = $"/{p.Id}/attachments";
            //var fbPosts = new winsdkfb.Graph.FBPaginatedArray(graphPath, null, FBWrapper.FBInsights.Factory);
            //FBResult result = null;
            //do
            //{
            //    if (result == null)
            //        result = await fbPosts.FirstAsync();
            //    else
            //        result = await fbPosts.NextAsync();
            //    if (result.Succeeded)
            //    {
            //        IReadOnlyList<object> posts = (IReadOnlyList<object>)result.Object;
            //        foreach (var i in posts)
            //        {
            //            FBInsights post = (FBInsights)i;
            //            if (post != null)
            //            {
            //                long value = 0;
            //                if (post.Values.Any())
            //                {
            //                    value = post.Values[0].Value;
            //                }
            //                await _view.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            //                {
            //                    switch (post.Name)
            //                    {
            //                        case "post_impressions":
            //                            {
            //                                p.LifetimeImpressions = value;
            //                                break;
            //                            }
            //                        case "post_impressions_unique":
            //                            {
            //                                p.LifetimeUniqueUsers = value;
            //                                break;
            //                            }

            //                    }
            //                });
            //                Console.WriteLine(post);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        FBErrorHandler.HandleError(result);
            //    }

            //} while (fbPosts.HasNext);
        }

        private async Task GetInsightsForPost(FBPagePost p)
        {
            string graphPath = $"/{p.Id}/insights/post_impressions,post_impressions_unique";
            var fbPosts = new winsdkfb.Graph.FBPaginatedArray(graphPath, null, FBWrapper.FBInsights.Factory);
            FBResult result = null;
            do
            {
                if (result == null)
                    result = await fbPosts.FirstAsync();
                else
                    result = await fbPosts.NextAsync();
                if (result.Succeeded)
                {
                    IReadOnlyList<object> posts = (IReadOnlyList<object>)result.Object;
                    foreach (var i in posts)
                    {
                        FBInsights post = (FBInsights)i;
                        if (post != null)
                        {
                            long value = 0;
                            if (post.Values.Any())
                            {
                                value = post.Values[0].Value;
                            }
                            await _view.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                            {
                                switch (post.Name)
                                {
                                    case "post_impressions":
                                        {
                                            p.LifetimeImpressions = value;
                                            break;
                                        }
                                    case "post_impressions_unique":
                                        {
                                            p.LifetimeUniqueUsers = value;
                                            break;
                                        }

                                }
                            });
                            Console.WriteLine(post);
                        }
                    }
                }
                else
                {
                    FBErrorHandler.HandleError(result);
                }

            } while (fbPosts.HasNext);
        }

        public static long GetUnixTimeStamp(DateTime dt)
        {
            return (long)(dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

    }
}
