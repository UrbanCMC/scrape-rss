using System.IO;
using System.Text;

namespace ScrapeRSS.IO
{
    /// <summary>
    /// Custom <see cref="StringWriter"/> implementation that uses UTF-8 instead of UTF-16
    /// </summary>
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}