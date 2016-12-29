using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssReader.Classes
{
    public class Channel : INotifyPropertyChanged
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }

        public ObservableCollection<RSSItem> RSSItems { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
