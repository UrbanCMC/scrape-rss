using System;
using System.Xml.Serialization;

namespace ScrapeRSS.Models.RSS
{
    /// <summary>
    /// Represents a single article in an RSS feed
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Gets or sets the author
        /// </summary>
        [XmlElement("author")]
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [XmlElement("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the article URL
        /// </summary>
        [XmlElement("link")]
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the date when the article was published.
        /// </summary>
        [XmlElement("pubDate")]
        public DateTime? PubDate { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [XmlElement("title")]
        public string Title { get; set; }

        public bool ShouldSerializeAuthor() => !string.IsNullOrWhiteSpace(Author);

        public bool ShouldSerializePubDate() => PubDate != null;
    }
}