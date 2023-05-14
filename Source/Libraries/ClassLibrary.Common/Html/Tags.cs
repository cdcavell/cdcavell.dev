namespace ClassLibrary.Common.Html
{
    /// <summary>
    /// Class to return string of Html tag.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.0.0 | 08/20/2022 | Initial Development |~ 
    /// </revision>
    public static class Tags
    {
        /// <summary>
        /// Method to return one new line
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <method>LineBreak()</method>
        public static string LineBreak()
        {
            return LineBreak(1);
        }

        /// <summary>
        /// Method to return multple new lines
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="count">int</param>
        /// <method>LineBreak(int count)</method>
        public static string LineBreak(int count)
        {
            string value = string.Empty;

            for (int i = 0; i < count; i++)
            {
                value += "<br/>";
            }

            return value;
        }

        /// <summary>
        /// Method to return one space
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <method>Space()</method>
        public static string Space()
        {
            return Space(1);
        }

        /// <summary>
        /// Method to return multipe spaces
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="count">int</param>
        /// <method>Space(int count)</method>
        public static string Space(int count)
        {
            string value = string.Empty;

            for (int i = 0; i < count; i++)
            {
                value += "&nbsp;";
            }

            return value;
        }

        /// <summary>
        /// Method to wrap given string in brakets
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Brackets(string item)</method>
        public static string Brackets(string item)
        {
            return "[ " + item + " ]";
        }

        /// <summary>
        /// Method to wrap given string in bold tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Bold(string item)</method>
        public static string Bold(string item)
        {
            return "<b>" + item + "</b>";
        }

        /// <summary>
        /// Method to wrap given string in italic tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Italic(string item)</method>
        public static string Italic(string item)
        {
            return "<i>" + item + "</i>";
        }

        /// <summary>
        /// Method to wrap given string in strong tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Important(string item)</method>
        public static string Important(string item)
        {
            return "<strong>" + item + "</strong>";
        }

        /// <summary>
        /// Method to wrap given string in em tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Emphasized(string item)</method>
        public static string Emphasized(string item)
        {
            return "<em>" + item + "</em>";
        }

        /// <summary>
        /// Method to wrap given string in mark tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Marked(string item)</method>
        public static string Marked(string item)
        {
            return "<mark>" + item + "</mark>";
        }

        /// <summary>
        /// Method to wrap given string in small tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Small(string item)</method>
        public static string Small(string item)
        {
            return "<small>" + item + "</small>";
        }

        /// <summary>
        /// Method to wrap given string in del tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Deleted(string item)</method>
        public static string Deleted(string item)
        {
            return "<del>" + item + "</del>";
        }

        /// <summary>
        /// Method to wrap given string in ins tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Inserted(string item)</method>
        public static string Inserted(string item)
        {
            return "<ins>" + item + "</ins>";
        }

        /// <summary>
        /// Method to wrap given string in sub tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Subscript(string item)</method>
        public static string Subscript(string item)
        {
            return "<sub>" + item + "</sub>";
        }

        /// <summary>
        /// Method to wrap given string in sup tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Superscript(string item)</method>
        public static string Superscript(string item)
        {
            return "<sup>" + item + "</sup>";
        }

        /// <summary>
        /// Method to wrap given string in q tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Quote(string item)</method>
        public static string Quote(string item)
        {
            return "<q>" + item + "</q>";
        }

        /// <summary>
        /// Method to wrap given string in blockquote tags with given citing
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <param name="cite">string</param>
        /// <method>BlockQuote(string item, string cite)</method>
        public static string BlockQuote(string item, string cite)
        {
            return "<blockquote cite=\"" + cite + "\">" + item + "</blockquote>";
        }

        /// <summary>
        /// Method to wrap given string in abbr tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Abbreviation(string item)</method>
        public static string Abbreviation(string item)
        {
            return "<abbr>" + item + "</abbr>";
        }

        /// <summary>
        /// Method to wrap given string in address tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Address(string item)</method>
        public static string Address(string item)
        {
            return "<address>" + item + "</address>";
        }

        /// <summary>
        /// Method to wrap given string in cite tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Cite(string item)</method>
        public static string Cite(string item)
        {
            return "<cite>" + item + "</cite>";
        }

        /// <summary>
        /// Method to wrap given string in bdo tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>BidirectionalOverride(string item)</method>
        public static string BidirectionalOverride(string item)
        {
            return "<bdo>" + item + "</bdo>";
        }

        /// <summary>
        /// Method to wrap given string in p tags
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        /// <param name="item">string</param>
        /// <method>Paragraph(string item)</method>
        public static string Paragraph(string item)
        {
            return "<p>" + item + "</p>";
        }
    }
}
