using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssReader.Classes
{
    public class RSSItem
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }

        public int ChannelID { get; set; }
        public Channel Channel { get; set; }

        public RSSItem()
        {

        }
    }
}
