using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ScrapeRSS.Web
{
    /// <summary>
    /// Helper class that is responsible for performing web requests in a way
    /// that mimicks the type of requests sent by a normal browser
    /// </summary>
    public static class WebHelper
    {
        private static HtmlWeb instance;

        private static HtmlWeb Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }

                instance = Create();
                return instance;
            }
        }

        /// <summary>
        /// Loads the website at the specified address and returns the resulting <see cref="HtmlDocument"/>
        /// </summary>
        /// <param name="uri">The address of the target website</param>
        /// <param name="cleanNodes">Whether the resulting <see cref="HtmlDocument"/> should be cleaned of unneeded nodes (e.g. meta and script tags)</param>
        /// <returns>A <see cref="HtmlDocument"/> containing the response from the target website</returns>
        public static HtmlDocument Load(string uri, bool cleanNodes = true)
        {
            var document = Instance.Load(uri);

            // Strip document and make sure HTML gets (somewhat) sanitized
            if (cleanNodes)
            {
                // remove link-tags
                var linkNodes = document.DocumentNode.SelectNodes("//link");
                if (linkNodes != null)
                {
                    foreach (var node in linkNodes)
                    {
                        node.ParentNode.RemoveChild(node);
                    }
                }

                // remove meta-tags
                var metaNodes = document.DocumentNode.SelectNodes("//meta");
                if (metaNodes != null)
                {
                    foreach (var node in metaNodes)
                    {
                        node.ParentNode.RemoveChild(node);
                    }
                }

                // remove scripts
                var scriptNodes = document.DocumentNode.SelectNodes("//script");
                if (scriptNodes != null)
                {
                    foreach (var node in scriptNodes)
                    {
                        node.ParentNode.RemoveChild(node);
                    }
                }

                // remove styles
                var styleNodes = document.DocumentNode.SelectNodes("//style");
                if (styleNodes != null)
                {
                    foreach (var node in styleNodes)
                    {
                        node.ParentNode.RemoveChild(node);
                    }
                }
            }

            return document;
        }

        /// <summary>
        /// Asynchronously loads the website at the specified address and returns the resulting <see cref="HtmlDocument"/>
        /// </summary>
        /// <param name="uri">The address of the target website</param>
        /// <param name="cleanNodes">Whether the resulting <see cref="HtmlDocument"/> should be cleaned of unneeded nodes (e.g. meta and script tags)</param>
        /// <returns>A Task that will reutrn a <see cref="HtmlDocument"/> containing the response from the target website</returns>
        public static async Task<HtmlDocument> LoadAsync(string uri, bool cleanNodes = true)
        {
            return await Task.Run(() => Load(uri, cleanNodes));
        }

        private static HtmlWeb Create()
        {
            var htmlWeb = new HtmlWeb
            {
                UseCookies = true,
                UserAgent = "Mozilla / 5.0(Windows NT 10.0; WOW64; rv: 49.0) Gecko / 20100101 Firefox / 49.0"
            };

            htmlWeb.PreRequest += HtmlWeb_PreRequest;
            return htmlWeb;
        }

        private static bool HtmlWeb_PreRequest(HttpWebRequest request)
        {
            if (request.CookieContainer == null)
            {
                request.CookieContainer = new CookieContainer();
            }

            request.ContentType = "application/x-ww-form-urlencoded";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            if (request.Headers[HttpRequestHeader.AcceptEncoding] == null)
            {
                request.Headers.Add(HttpRequestHeader.AcceptEncoding, "deflate");
                request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-GB;en-US;q=0.8,en;q=0.6,*;q=0.3");
                request.Headers.Add(HttpRequestHeader.AcceptCharset, "ISO-8859-1,utf-8;q=0.7,*;q=0.3");
            }

            return true;
        }
    }
}