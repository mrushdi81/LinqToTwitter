﻿using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;
using LinqToTwitter;

namespace LinqToTwitterSilverlightDemo.Views
{
    public partial class PublicStatusQuery : Page
    {
        public PublicStatusQuery()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var twitterCtx = new TwitterContext();

            //(from tweet in twitterCtx.Status
            // where tweet.Type == StatusType.Public
            // select tweet)
            //.AsyncCallback(tweets =>
            //    Dispatcher.BeginInvoke(() =>
            //    {
            //        var projectedTweets =
            //            (from tweet in tweets
            //            select new MyTweet
            //            {
            //                ScreenName = tweet.User.Identifier.ScreenName,
            //                Tweet = tweet.Text
            //            })
            //            .ToList();

            //        dataGrid1.ItemsSource = projectedTweets;
            //    }))
            //.SingleOrDefault();
        }

    }
}
