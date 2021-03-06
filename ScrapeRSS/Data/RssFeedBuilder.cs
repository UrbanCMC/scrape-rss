﻿using System.Threading.Tasks;
using System.Xml.Serialization;
using ScrapeRSS.IO;
using ScrapeRSS.Models;
using ScrapeRSS.Models.RSS;
using ScrapeRSS.Web;

namespace ScrapeRSS.Data
{
    /// <summary>
    /// Class responsible for building an RSS feed
    /// </summary>
    public static class RssFeedBuilder
    {
        /// <summary>
        /// Asynchronously gets an RSS feed using the specified settings
        /// </summary>
        /// <param name="settings">The settings defining how the feed should be generated</param>
        /// <returns>A string containing the XML of the generated feed</returns>
        public static async Task<string> GetFeed(FeedGeneratorSettings settings)
        {
            var channel = await BuildFeed(settings);
            return SerializeToFeed(channel);
        }

        /// <summary>
        /// Asynchronously builds the feed from the specified settings
        /// </summary>
        /// <param name="settings">The settings defining how the feed should be generated</param>
        /// <returns>The <see cref="Channel"/> that was built</returns>
        private static async Task<Channel> BuildFeed(FeedGeneratorSettings settings)
        {
            var parser = new WebsiteParser(settings.Url);
            await parser.LoadWebsite();

            var channel = new Channel()
            {
                Link = settings.Url,
                Description = "Generated by ScrapeRSS"
            };

            if (!string.IsNullOrWhiteSpace(settings.Title))
            {
                channel.Title = settings.Title;
            }
            else
            {
                channel.Title = parser.GetTitle();
            }

            channel.Items = parser.GetEligibleItems(settings);

            return channel;
        }

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