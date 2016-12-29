using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Syndication;

namespace RssReader.Classes
{
    public class MainViewModel : DbContext, INotifyPropertyChanged
    {
        public DbSet<RSSItem> RSSItemsDB { get; set; }
        public DbSet<Channel> ChannelsDB { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=RssFeeds.db");
        }

        private ObservableCollection<RSSItem> _rssItems;
        public ObservableCollection<RSSItem> RSSItems
        {
            get
            {
                return _rssItems;
            }
            set
            {
                if (_rssItems != value)
                {
                    _rssItems = value;
                    NotifyPropertyChanged("RSSItems");
                }
            }
        }

        private ObservableCollection<RSSItem> _rssItemsToView;
        public ObservableCollection<RSSItem> RSSItemsToView
        {
            get
            {
                return _rssItemsToView;
            }
            set
            {
                if (_rssItemsToView != value)
                {
                    _rssItemsToView = value;
                    NotifyPropertyChanged("RSSItemsToView");
                }
            }
        }

        private ObservableCollection<Channel> _channels;
        public ObservableCollection<Channel> Channels
        {
            get
            {
                return _channels;
            }
            set
            {
                if (_channels != value)
                {
                    _channels = value;
                    NotifyPropertyChanged("Channels");
                }
            }
        }

        public MainViewModel()
        {
            Channels = new ObservableCollection<Channel>();
            RSSItems = new ObservableCollection<RSSItem>();

        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        public void LoadData()
        {
            Channels = new ObservableCollection<Channel>(ChannelsDB.ToList());
            RSSItems = new ObservableCollection<RSSItem>(RSSItemsDB.ToList());
            CreateChannel("https://habrahabr.ru/rss/best/");
            CreateChannel("https://habrahabr.ru/rss/best/weekly");
            CreateChannel("https://habrahabr.ru/rss/best/monthly");
            CreateChannel("https://habrahabr.ru/rss/best/alltime");
            IsDataLoaded = true;
        }

        public void CreateChannel(string uri)
        {
            try
            {
                load(new Uri(uri));
            }
            catch
            {

            }
        }

        public void MakeRssItemsToView(Channel ch)
        {
            load(new Uri(ch.Link));
            RSSItemsToView = ch.RSSItems;
        }

        private async void load(Uri uri)
        {
            SyndicationClient client = new SyndicationClient();
            SyndicationFeed feed = await client.RetrieveFeedAsync(uri);
            if (feed != null)
            {
                Channel ch = IsContainChannel(uri.ToString());
                if (ch == null)
                {
                    ch = new Channel { Title = feed.Title.Text, Link = uri.ToString(), Description = feed.Subtitle.Text };
                    ChannelsDB.Add(ch);
                    Channels.Add(ch);
                }
                                  
                foreach (SyndicationItem item in feed.Items)
                {
                    RSSItem RI = new RSSItem();
                    RI.Channel = ch;
                    RI.ChannelID = ch.ID;
                    RI.Link = item.Id;
                    RI.Title = item.Title.Text;
                    RI.Description = item.Summary.Text;
                    if (!IsContainRSSItem(RI))
                    {
                        RSSItemsDB.Add(RI);
                        RSSItems.Add(RI);
                    }                   
                }  
            }
            SaveChanges();
            
        }

        private Channel IsContainChannel(string ch)
        {
            foreach (Channel c in Channels)
            {
                if (c.Link.ToString() == ch)
                    return c;
            }
            return null;
        }

        private bool IsContainRSSItem(RSSItem ri)
        {
            foreach (RSSItem r in RSSItems)
            {
                if (ri.Title == r.Title)
                    return true;
            }
            return false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
