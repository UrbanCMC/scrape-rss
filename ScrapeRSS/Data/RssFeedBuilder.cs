using System.Xml.Serialization;
using ScrapeRSS.IO;
using ScrapeRSS.Models.RSS;

namespace ScrapeRSS.Data
{
    /// <summary>
    /// Class responsible for building an RSS feed
    /// </summary>
    public static class RssFeedBuilder
    {
        /// <summary>
        /// Serializes the specified <see cref="Channel"/> into an RSS feed and returns the resulting string
        /// </summary>
        /// <param name="channel">The channel to serialize</param>
        /// <returns>A string containing the serialized RSS feed</returns>
        private static string SerializeToFeed(Channel channel)
        {
            var serializer = new XmlSerializer(typeof(Root));
            var ns = new XmlSerializerNamespaces();
            ns.Add("atom", "http://www.w3.org/2005/Atom");

            var root = new Root()
            {
                Channel = channel
            };

            using var sw = new Utf8StringWriter();

            serializer.Serialize(sw, root, ns);
            sw.Flush();

            return sw.ToString();
        }
    }
}