using System.Text.RegularExpressions;

namespace FamilyLibrary.Persistence
{
    public static class HtmlToXml
    {
        private static readonly Regex VoidTag =
        new(@"<(br|hr|img)(\s[^>]*?)?\s*(?<!/)>", RegexOptions.IgnoreCase);

        public static string? Normalize(string? html)
        {
            if (html is null) return null;

            string result = html.Replace("&nbsp;", "&#160;");
            return VoidTag.Replace(result, "<$1$2 />");
        }
    }
}
