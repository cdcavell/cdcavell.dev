using System.Net;

namespace ClassLibrary.Mvc.Services.Email
{
    /// <summary>
    /// Email Web Service Options
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 11/13/2022 | User Registration Development |~ 
    /// </revision>
    public class EmailServiceOptions
    {
        /// <value>string</value>
        public string Host { get; set; } = string.Empty;
        /// <value>int</value>
        public int Port { get; set; }
        /// <value>NetworkCredential</value>
        public NetworkCredential Credentials { get; set; } = new();
        /// <value>bool</value>
        public bool EnableSsl { get; set; }
    }
}
