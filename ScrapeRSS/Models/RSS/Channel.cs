using System.Collections.Generic;
using System.Xml.Serialization;

namespace ScrapeRSS.Models.RSS
{
    /// <summary>
    /// Represents the channel an RSS feed is generated for
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [XmlElement("description", Order = 3)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the feed items
        /// </summary>
        [XmlElement("item", Order = 4)]
        public List<Item> Items { get; set; }

        /// <summary>
        /// Gets or sets the website URL
        /// </summary>
        [XmlElement("link", Order = 2)]
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [XmlElement("title", Order = 1)]
        public string Title { get; set; }

        public bool ShouldSerializeItems() => Items?.Count > 0;
    }
}