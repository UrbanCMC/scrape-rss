using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.XPath;
using HtmlAgilityPack;
using ScrapeRSS.Models;
using ScrapeRSS.Models.RSS;

namespace ScrapeRSS.Web
{
    /// <summary>
    /// Responsible for parsing a website for the data used to build an RSS feed
    /// </summary>
    public class WebsiteParser
    {
        private readonly string url;
        private HtmlNodeNavigator htmlNavigator;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebsiteParser"/> class
        /// </summary>
        /// <param name="url">The URL of the website to parse</param>
        public WebsiteParser(string url)
        {
            this.url = url;
        }

        /// <summary>
        /// Gets a list of feed items that can be parsed from the website
        /// </summary>
        /// <param name="settings">The settings to use</param>
        /// <returns>A list of <see cref="Item"/> containing the feed items that matched the parsing settings</returns>
        public List<Item> GetEligibleItems(FeedGeneratorSettings settings)
        {
            var items = new List<Item>();

            XPathNodeIterator itemNodes;
            if (!string.IsNullOrWhiteSpace(settings.MatchIdOrClass))
            {
                itemNodes = htmlNavigator.Select($"//a[@href and ancestor-or-self::*[@id=\"{settings.MatchIdOrClass}\" or contains(concat(\" \", normalize-space(@class),\" \"), \" {settings.MatchIdOrClass} \")]]");
            }
            else if (!string.IsNullOrWhiteSpace(settings.ItemSelector))
            {
                itemNodes = htmlNavigator.Select(settings.ItemSelector);
            }
            else
            {
                itemNodes = htmlNavigator.Select("//a[@href]");
            }

            var baseUrl = new Uri(settings.Url);

            foreach (HtmlNodeNavigator nodeNav in itemNodes)
            {
                if (string.IsNullOrWhiteSpace(settings.ItemSelector))
                {
                    // Nodes contain a simple <a> we can create the item from
                    var title = nodeNav.SelectSingleNode("./text()")?.Value;
                    var url = nodeNav.SelectSingleNode("./@href").Value;

                    // Convert relative URL to absolute (works fine if URL is already absolute)
                    url = new Uri(baseUrl, url).AbsoluteUri;

                    if (!url.Contains(settings.MatchItemUrl)
                        || items.Any(a => a.Title == title || a.Link == url))
                    {
                        continue;
                    }

                    items.Add(new Item
                    {
                        Title = title,
                        Link = url,
                        Description = ""
                    });
                }
                else
                {
                    string title;
                    string url;
                    var description = "";
                    var author = "";
                    DateTime? date = null;

                    var firstMatchingLink = nodeNav.SelectSingleNode("./a[@href]");

                    // Nodes may contain any kind of HTML, so we have to apply further selectors before we get our item

                    // Title
                    if (!string.IsNullOrWhiteSpace(settings.ItemTitleSelector))
                    {
                        title = nodeNav.SelectSingleNode(settings.ItemTitleSelector).Value;
                    }
                    else
                    {
                        title = firstMatchingLink.SelectSingleNode("./text()").Value;
                    }

                    // URL
                    if (!string.IsNullOrWhiteSpace(settings.ItemUrlSelector))
                    {
                        url = nodeNav.SelectSingleNode(settings.ItemUrlSelector).Value;
                    }
                    else
                    {
                        url = firstMatchingLink.SelectSingleNode("./@href").Value;
                    }

                    // Description
                    if (!string.IsNullOrWhiteSpace(settings.ItemDescriptionSelector))
                    {
                        description = nodeNav.SelectSingleNode(settings.ItemDescriptionSelector).Value;
                    }

                    // Author
                    if (!string.IsNullOrWhiteSpace(settings.Author))
                    {
                        author = settings.Author;
                    }
                    else if (!string.IsNullOrWhiteSpace(settings.ItemAuthorSelector))
                    {
                        author = nodeNav.SelectSingleNode(settings.ItemAuthorSelector).Value;
                    }

                    // Publish date
                    if (!string.IsNullOrWhiteSpace(settings.ItemDateSelector))
                    {
                        if (DateTime.TryParse(nodeNav.SelectSingleNode(settings.ItemTitleSelector).Value, out var parsedDate))
                        {
                            date = parsedDate;
                        }
                    }

                    // Convert relative URL to absolute (works fine if URL is already absolute)
                    url = new Uri(baseUrl, url).AbsoluteUri;

                    if (!url.Contains(settings.MatchItemUrl)
                        || items.Any(a => a.Title == title || a.Link == url))
                    {
                        continue;
                    }

                    items.Add(new Item
                    {
                        Title = title,
                        Link = url,
                        Description = description,
                        Author = author,
                        PubDate = date
                    });
                }

                // Stop parsing once we reach the desired number of items
                if (items.Count == settings.MaxResults)
                {
                    break;
                }
            }

            return items;
        }

        /// <summary>
        /// Gets the title of the parsed website
        /// </summary>
        /// <returns>A string containing the parsed website's title; or 'No title available' if none was found</returns>
        public string GetTitle()
        {
            if (htmlNavigator == null)
            {
                throw new InvalidOperationException("No website has been loaded for parsing.");
            }

            var title = htmlNavigator.SelectSingleNode("//head/title")?.Value;
            return title ?? "No title available";
        }

        /// <summary>
        /// Asynchronously loads the HTML document returned by the target website.
        /// Must be called before parsing can begin.
        /// </summary>
        /// <returns>A task signaling the completion of the operation</returns>
        public async Task LoadWebsite()
        {
            var htmlDoc = await WebHelper.LoadAsync(url);
            htmlNavigator = htmlDoc.CreateNavigator() as HtmlNodeNavigator;
        }
    }
}