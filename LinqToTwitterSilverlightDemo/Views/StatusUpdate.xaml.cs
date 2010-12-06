﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using LinqToTwitter;
using System.Threading;

namespace LinqToTwitterSilverlightDemo.Views
{
    public partial class StatusUpdate : Page
    {
        private TwitterContext m_twitterCtx = null;
        private PinAuthorizer m_pinAuth = null;

        public StatusUpdate()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            m_pinAuth = new PinAuthorizer
            {
                Credentials = new InMemoryCredentials
                {
                    ConsumerKey = "",
                    ConsumerSecret = ""
                },
                UseCompression = true,
                GoToTwitterAuthorization = pageLink => 
                    Dispatcher.BeginInvoke(() => WebBrowser.Navigate(new Uri(pageLink)))
            };

            m_pinAuth.BeginAuthorize(resp => 
                Dispatcher.BeginInvoke(() =>
                {
                    switch (resp.Status)
                    {
                        case TwitterErrorStatus.Success:
                            break;
                        case TwitterErrorStatus.TwitterApiError:
                        case TwitterErrorStatus.RequestProcessingException:
                            MessageBox.Show(
                                resp.Error.ToString(),
                                resp.Message,
                                MessageBoxButton.OK);
                            break;
                    }
                }));

            m_twitterCtx = new TwitterContext(m_pinAuth, "https://api.twitter.com/1/", "https://search.twitter.com/");
        }

        private void PinButton_Click(object sender, RoutedEventArgs e)
        {
            string pin = PinTextBox.Text;

            m_pinAuth.CompleteAuthorize(
                PinTextBox.Text,
                completeResp => Dispatcher.BeginInvoke(() =>
                {
                    switch (completeResp.Status)
                    {
                        case TwitterErrorStatus.Success:
                            UpdatePanel.Visibility = Visibility.Visible;
                            TweetTextBox.Text = "Silverlight OOB Test, " + DateTime.Now.ToString() + " #linqtotwitter";
                            break;
                        case TwitterErrorStatus.TwitterApiError:
                        case TwitterErrorStatus.RequestProcessingException:
                            MessageBox.Show(
                                completeResp.Error.ToString(),
                                completeResp.Message,
                                MessageBoxButton.OK);
                            break;
                    }
                }));
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            m_twitterCtx.UpdateStatus(TweetTextBox.Text,
                updateResp => Dispatcher.BeginInvoke(() =>
                {
                    switch (updateResp.Status)
                    {
                        case TwitterErrorStatus.Success:
                            Status tweet = updateResp.State;
                            User user = tweet.User;
                            UserIdentifier id = user.Identifier;
                            MessageBox.Show(
                                "User: " + id.ScreenName +
                                ", Posted Status: " + tweet.Text,
                                "Update Successfully Posted.",
                                MessageBoxButton.OK);
                            break;
                        case TwitterErrorStatus.TwitterApiError:
                        case TwitterErrorStatus.RequestProcessingException:
                            MessageBox.Show(
                                updateResp.Error.ToString(),
                                updateResp.Message,
                                MessageBoxButton.OK);
                            break;
                    }
                }));
        }
    }
}
