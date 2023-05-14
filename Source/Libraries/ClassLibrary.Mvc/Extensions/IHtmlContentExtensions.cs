using System.Text.Encodings.Web;

namespace Microsoft.AspNetCore.Html
{
    /// <summary>
    /// Extension methods for existing IHtmlContent types.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/31/2022 | User Role Claims Development |~ 
    /// </revision>
    public static class IHtmlContentExtensions
    {
        /// <summary>
        /// Returns HTML string for IHtmlContent.
        /// &lt;br /&gt;&lt;br /&gt;
        /// Original Developer: cygnim (https://stackoverflow.com/users/1152681/cygnim)
        /// &lt;br /&gt;&lt;br /&gt;
        /// Original Source: https://stackoverflow.com/a/33679999/8041900
        /// </summary>
        /// <param name="content">IHtmlContent</param>
        /// <returns>string</returns>
        public static string GetString(this IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}
