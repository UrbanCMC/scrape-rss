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
        [XmlElement("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the feed items
        /// </summary>
        [XmlElement("item")]
        public List<Item> Items { get; set; }

        /// <summary>
        /// Gets or sets the website URL
        /// </summary>
        [XmlElement("link")]
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }

        public bool ShouldSerializeItems() => Items?.Count > 0;
    }
}