namespace dis.cdcavell.dev.Models.AppSettings
{
    /// <summary>
    /// Email service model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 11/13/2022 | User Registration Development |~ 
    /// </revision>
    public class EmailService
    {
        /// <value>string</value>
        public string Host { get; set; } = string.Empty;
        /// <value>int</value>
        public int Port { get; set; }
        /// <value>bool</value>
        public bool EnableSsl { get; set; }
        /// <value>string</value>
        public string UserId { get; set; } = string.Empty;
        /// <value>string</value>
        public string Password { get; set; } = string.Empty;
        /// <value>string</value>
        public string Email { get; set; } = string.Empty;
    }
}
