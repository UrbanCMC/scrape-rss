namespace ScrapeRSS.Models
{
    /// <summary>
    /// Represents all settings available to customize the feed generation process
    /// </summary>
    public class FeedGeneratorSettings
    {
        /// <summary>
        /// <para>Gets or sets the author to use for all items.</para>
        /// <para>Cannot be combined with <see cref="ItemAuthorSelector"/>.</para>
        /// </summary>
        public string Author { get; set; } = "";

        /// <summary>
        /// <para>Gets or sets the XPath selector used to get the item's author.</para>
        /// <para>Requires <see cref="ItemSelector"/> to be set.</para>
        /// <para>Cannot be combined with <see cref="Author"/>.</para>
        /// </summary>
        public string ItemAuthorSelector { get; set; } = "";

        /// <summary>
        /// <para>Gets or sets the XPath selector used to get the item's publishing date.</para>
        /// <para>Requires <see cref="ItemSelector"/> to be set.</para>
        /// </summary>
        public string ItemDateSelector { get; set; } = "";

        /// <summary>
        /// <para>Gets or sets the XPath selector used to get the item's description.</para>
        /// <para>Requires <see cref="ItemSelector"/> to be set.</para>
        /// </summary>
        public string ItemDescriptionSelector { get; set; } = "";

        /// <summary>
        /// <para>Gets or sets the XPath selector used to get the item.</para>
        /// <para>Cannot be combined with <see cref="MatchIdOrClass"/>.</para>
        /// </summary>
        public string ItemSelector { get; set; } = "";

        /// <summary>
        /// <para>Gets or sets the XPath selector used to get the item's title.</para>
        /// <para>Requires <see cref="ItemSelector"/> to be set.</para>
        /// </summary>
        public string ItemTitleSelector { get; set; } = "";

        /// <summary>
        /// <para>Gets or sets the XPath selector used to get the item's URL.</para>
        /// <para>Requires <see cref="ItemSelector"/> to be set.</para>
        /// </summary>
        public string ItemUrlSelector { get; set; } = "";

        /// <summary>
        /// <para>Gets or sets a value an item's class or id must contain to be added to the feed.</para>
        /// <para>Cannot be combined with <see cref="ItemSelector"/>.</para>
        /// </summary>
        public string MatchIdOrClass { get; set; } = "";

        /// <summary>
        /// Gets or sets the URL an item must match to be added to the feed
        /// </summary>
        public string MatchItemUrl { get; set; } = "";

        /// <summary>
        /// Gets or sets the maximum number of items in the feed
        /// </summary>
        public int MaxResults { get; set; }

        /// <summary>
        /// Gets or sets the title to use for the generated feed
        /// </summary>
        /// <remarks>An empty title will use the target website's title instead</remarks>
        public string Title { get; set; } = "";

        /// <summary>
        /// Gets or sets the URL of the target website
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets a string containing the validation error
        /// </summary>
        public string ValidationError { get; private set; }

        /// <summary>
        /// Validates the current settings. See <see cref="ValidationError"/> for the reason that validation failed.
        /// </summary>
        /// <returns><c>true</c> if the settings are valid; otherwise <c>false</c>.</returns>
        public bool Validate()
        {
            ValidationError = "";

            if (string.IsNullOrWhiteSpace(Url))
            {
                ValidationError = "A value for 'url' must be specified.";
                return false;
            }

            if (MaxResults < 1 || MaxResults > 100)
            {
                ValidationError = "'maxResults' must be between 1-100, inclusive.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(MatchIdOrClass) && !string.IsNullOrWhiteSpace(ItemSelector))
            {
                ValidationError = "Can't use both 'matchIdOrClass' and 'itemSelector' at the same time.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(ItemSelector)
                && (!string.IsNullOrWhiteSpace(ItemAuthorSelector)
                    || !string.IsNullOrWhiteSpace(ItemDateSelector)
                    || !string.IsNullOrWhiteSpace(ItemDescriptionSelector)
                    || !string.IsNullOrWhiteSpace(ItemUrlSelector)
                    || !string.IsNullOrWhiteSpace(ItemTitleSelector)))
            {
                ValidationError = "XPath selectors can only be used if 'itemSelector' is specified.";
                return false;
            }

            if (!string.IsNullOrWhiteSpace(Author) && !string.IsNullOrWhiteSpace(ItemAuthorSelector))
            {
                ValidationError = "Can't use both 'author' and 'itemAuthorSelector' at the same time.";
                return false;
            }

            return true;
        }
    }
}