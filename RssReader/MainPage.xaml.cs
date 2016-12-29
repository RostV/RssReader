using RssReader.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RssReader
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            DataContext = App.ViewModel;            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
                App.ViewModel.LoadData();
            if (App.ViewModel.Channels.Count > 0)         
                ChannelsLB.SelectedIndex = 0;          
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddChannelPanel.Visibility = Visibility.Visible;
            AddChannelBtn.IsEnabled = false;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.CreateChannel(UriTb.Text);         
            ChannelsLB.SelectedIndex = ChannelsLB.Items.Count - 1;
            AddChannelBtn.IsEnabled = true;
            AddChannelPanel.Visibility = Visibility.Collapsed;          
            
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            AddChannelPanel.Visibility = Visibility.Collapsed;
            AddChannelBtn.IsEnabled = true;
        }

        private void ChannelsLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChannelsLB.SelectedItem != null)
            {
                App.ViewModel.MakeRssItemsToView((Channel)ChannelsLB.SelectedItem);
            }
        }

        private void RSSItemsGV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RSSItemsGV.SelectedItem != null)
            {
                WVPanel.Visibility = Visibility.Visible;
                RssFeedWV.Navigate(new Uri(((RSSItem)RSSItemsGV.SelectedItem).Link));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WVPanel.Visibility = Visibility.Collapsed;
            RSSItemsGV.SelectedIndex = -1;
            
        }

        private void RssFeedWV_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            WebViewPR.IsActive = true;
        }

        private void RssFeedWV_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            WebViewPR.IsActive = false;
        }
    }
}
