using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace System
{
    /// <summary>
    /// Extension methods for existing string types.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.2.0 | 10/02/2022 | Duende IdentityServer Development |~ 
    /// | Christopher D. Cavell | 1.0.0.0 | 09/04/2021 | Initial Development |~ 
    /// </revision>
    public static class StringExtensions
    {
        /// <summary>
        /// Method to determine if string is a valid email address
        /// </summary>
        /// <param name="value">this string</param>
        /// <returns>bool</returns>
        /// <method>IsValidEmail(this string value)</method>
        public static bool IsValidEmail(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            try
            {
                // Normalize the domain
                value = Regex.Replace(value, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                static string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(value,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Method to determine if string is a valid url
        /// </summary>
        /// <param name="value">this string</param>
        /// <returns>bool</returns>
        /// <method>IsValidUrl(this string value)</method>
        public static bool IsValidUrl(this string value)
        {
            if (string.IsNullOrEmpty(value.Clean()))
                return false;

            Uri? uriResult;
            return Uri.TryCreate(value.Clean(), UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        /// <summary>
        /// Method to determine if string is a local url
        /// </summary>
        /// <param name="value">this string</param>
        /// <returns>bool</returns>
        /// <method>IsLocalUrl(this string value)</method>
        public static bool IsLocalUrl(this string value)
        {
            string url = value.Clean();

            if (string.IsNullOrEmpty(url))
                return false;

            // Allows "/" or "/foo" but not "//" or "/\".
            if (url[0] == '/')
            {
                // url is exactly "/"
                if (url.Length == 1)
                {
                    return true;
                }

                // url doesn't start with "//" or "/\"
                if (url[1] != '/' && url[1] != '\\')
                {
                    return true;
                }

                return false;
            }

            // Allows "~/" or "~/foo" but not "~//" or "~/\".
            if (url[0] == '~' && url.Length > 1 && url[1] == '/')
            {
                // url is exactly "~/"
                if (url.Length == 2)
                {
                    return true;
                }

                // url doesn't start with "~//" or "~/\"
                if (url[2] != '/' && url[2] != '\\')
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// Method to remove "Carriage Return" and "Line Feed" as well as Html filtering to provide proper neutralization.
        /// </summary>
        /// <param name="value">this string</param>
        /// <returns>string</returns>
        /// <method>Clean(this string value)</method>
        public static string Clean(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            string cleanString = HttpUtility.HtmlEncode(value.Replace("\r", string.Empty).Replace("\n", string.Empty));
            return string.Format("{0}", cleanString);
        }

        /// <summary>
        /// Method to determine if string is a valid Guid
        /// </summary>
        /// <param name="value">this string</param>
        /// <returns>bool</returns>
        /// <method>IsValidGuid(this string value)</method>
        public static bool IsValidGuid(this string value)
        {
            Regex isGuid = new(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);
            bool isValid = false;

            if (!string.IsNullOrEmpty(value))
                if (isGuid.IsMatch(value))
                    isValid = true;

            return isValid;
        }

        /// <summary>
        /// UTF8 Encoding
        /// </summary>
        /// <param name="input">this string</param>
        /// <returns>string</returns>
        /// <method>UTF8(this string input)</method>
        public static string UTF8(this string input)
        {
            byte[] bytes = Encoding.Default.GetBytes(input);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// Creates a SHA256 hash of the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A hash</returns>
        public static string Sha256(this string input)
        {
            if (string.IsNullOrEmpty(input.Trim())) return string.Empty;

            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }
    }
}
