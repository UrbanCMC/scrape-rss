using System.IO;
using System.Xml;
using System.Xml.Serialization;
using ScrapeRSS.Models.RSS;

namespace ScrapeRSS.Data
{
    /// <summary>
    /// Class responsible for building an RSS feed
    /// </summary>
    public class RssFeedBuilder
    {
        /// <summary>
        /// Serializes the specified <see cref="Channel"/> into an RSS feed and returns the resulting <see cref="XmlDocument"/>
        /// </summary>
        /// <param name="channel">The channel to serialize</param>
        /// <returns>An <see cref="XmlDocument"/> containing the serialized RSS feed</returns>
        public XmlDocument SerializeToFeed(Channel channel)
        {
            var serializer = new XmlSerializer(typeof(Root));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            var root = new Root()
            {
                Channel = channel
            };

            using var sw = new StringWriter();
            serializer.Serialize(sw, root);

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(sw.ToString());

            return xmlDoc;
        }
    }
}