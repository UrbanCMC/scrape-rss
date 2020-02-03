using System.Xml.Serialization;

namespace ScrapeRSS.Models.RSS
{
    /// <summary>
    /// Represents the RSS root element
    /// </summary>
    [XmlRoot("rss")]
    public class Root
    {
        /// <summary>
        /// Gets or sets the channel
        /// </summary>
        [XmlElement("channel")]
        public Channel Channel { get; set; }

        /// <summary>
        /// Gets the RSS version
        /// </summary>
        [XmlAttribute("version")]
        public string Version { get; set; } = "2.0";
    }
}