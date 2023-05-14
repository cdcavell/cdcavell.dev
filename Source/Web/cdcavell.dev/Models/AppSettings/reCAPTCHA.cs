namespace cdcavell.dev.Models.AppSettings
{
    /// <summary>
    /// reCAPTCHA Authentication model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/29/2022 | User Role Claims Development |~ 
    /// </revision>
    public class reCAPTCHA
    {
        /// <value>string</value>
        public string SiteKey { get; set; } = string.Empty;
        /// <value>string</value>
        public string SecretKey { get; set; } = string.Empty;
    }
}
