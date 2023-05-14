using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.AspNetCore.Mvc.Rendering
{
    /// <summary>
    /// Extension methods for existing IHtmlHelper types.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 02/20/2023 | User Role Claims Development |~ 
    /// </revision>
    public static class IHtmlHelperExtensions
    {
        /// <summary>
        /// Creates HTML for an &lt;c&gt;img&lt;/c&gt; element that presents a Gravatar icon (https://en.gravatar.com/).
        /// &lt;br /&gt;&lt;br /&gt;
        /// Original Developer: Ricardo Polo Jaramillo (https://stackoverflow.com/users/909974/ricardo-polo-jaramillo)
        /// &lt;br /&gt;&lt;br /&gt;
        /// Original Source: https://stackoverflow.com/a/3561841/8041900
        /// </summary>
        /// <param name="html">The &lt;see cref="IHtmlHelper"/&gt; upon which this extension method is provided.</param>
        /// <param name="email">The email address used to identify the icon.</param>
        /// <param name="size">An optional parameter that specifies the size of the square image in pixels.</param>
        /// <param name="rating">An optional parameter that specifies the safety level of allowed images.</param>
        /// <param name="defaultImage">An optional parameter that controls what image is displayed for email addresses that don't have associated Gravatar icons.</param>
        /// <param name="htmlAttributes">An optional parameter holding additional attributes to be included on the <c>img</c> element.</param>
        /// <returns>An HTML string of the <c>img</c> element that presents a Gravatar icon.</returns>
        [UnsupportedOSPlatform("browser")]
        public static HtmlString Gravatar(this IHtmlHelper html,
                                      string email,
                                      int? size = null,
                                      GravatarRating rating = GravatarRating.Default,
                                      GravatarDefaultImage defaultImage = GravatarDefaultImage.MysteryMan,
                                      object? htmlAttributes = null)
        {
            if (html is null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            var url = new StringBuilder("https://www.gravatar.com/avatar/", 90);
            url.Append(GetEmailHash(email));

            var isFirst = true;
            void addParam(string p, string v)
            {
                url.Append(isFirst ? '?' : '&');
                isFirst = false;
                url.Append(p);
                url.Append('=');
                url.Append(v);
            }

            if (size != null)
            {
                if (size < 1 || size > 512)
                    throw new ArgumentOutOfRangeException(nameof(size), size, "Must be null or between 1 and 512, inclusive.");
                addParam("s", size.Value.ToString());
            }

            if (rating != GravatarRating.Default)
                addParam("r", rating.ToString().ToLower());

            if (defaultImage != GravatarDefaultImage.Default)
            {
                if (defaultImage == GravatarDefaultImage.Http404)
                    addParam("d", "404");
                else if (defaultImage == GravatarDefaultImage.Identicon)
                    addParam("d", "identicon");
                if (defaultImage == GravatarDefaultImage.MonsterId)
                    addParam("d", "monsterid");
                if (defaultImage == GravatarDefaultImage.MysteryMan)
                    addParam("d", "mm");
                if (defaultImage == GravatarDefaultImage.Wavatar)
                    addParam("d", "wavatar");
            }

            var tag = new TagBuilder("img");
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tag.Attributes.Add("src", url.ToString());

            if (size != null)
            {
                tag.Attributes.Add("width", size.ToString());
                tag.Attributes.Add("height", size.ToString());
            }

            string renderedTag = tag.RenderSelfClosingTag().GetString();
            return new HtmlString(renderedTag);
        }

        [UnsupportedOSPlatform("browser")]
        private static string GetEmailHash(string email)
        {
            if (email == null)
                return new string('0', 32);

            email = email.Trim().ToLower();

            var emailBytes = Encoding.ASCII.GetBytes(email);
            var hashBytes = MD5.Create().ComputeHash(emailBytes);

            Debug.Assert(hashBytes.Length == 16);

            var hash = new StringBuilder();
            foreach (var b in hashBytes)
                hash.Append(b.ToString("x2"));
            return hash.ToString();
        }
    }

    /// <summary>
    /// GravatarRating Enumeration
    /// </summary>
    public enum GravatarRating
    {
        /// <summary>
        /// The default value as specified by the Gravatar service.  That is, no rating value is specified
        /// with the request.  At the time of authoring, the default level was <see cref="G"/>.
        /// </summary>
        Default,

        /// <summary>
        /// Suitable for display on all websites with any audience type.  This is the default.
        /// </summary>
        G,

        /// <summary>
        /// May contain rude gestures, provocatively dressed individuals, the lesser swear words, or mild violence.
        /// </summary>
        Pg,

        /// <summary>
        /// May contain such things as harsh profanity, intense violence, nudity, or hard drug use.
        /// </summary>
        R,

        /// <summary>
        /// May contain hardcore sexual imagery or extremely disturbing violence.
        /// </summary>
        X
    }

    /// <summary>
    /// GravatarDefaultImage Enumeration
    /// </summary>
    public enum GravatarDefaultImage
    {
        /// <summary>
        /// The default value image.  That is, the image returned when no specific default value is included
        /// with the request.  At the time of authoring, this image is the Gravatar icon.
        /// </summary>
        Default,

        /// <summary>
        /// Do not load any image if none is associated with the email hash, instead return an HTTP 404 (File Not Found) response.
        /// </summary>
        Http404,

        /// <summary>
        /// A simple, cartoon-style silhouetted outline of a person (does not vary by email hash).
        /// </summary>
        MysteryMan,

        /// <summary>
        /// A geometric pattern based on an email hash.
        /// </summary>
        Identicon,

        /// <summary>
        /// A generated 'monster' with different colors, faces, etc.
        /// </summary>
        MonsterId,

        /// <summary>
        /// Generated faces with differing features and backgrounds.
        /// </summary>
        Wavatar
    }
}
