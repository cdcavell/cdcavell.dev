namespace usr.cdcavell.dev.Models.AppSettings
{
    /// <summary>
    /// IdP model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/24/2022 | User Role Claims Development |~ 
    /// </revision>
    public class IdP
    {
        /// <value>string</value>
        public string Authority { get; set; } = string.Empty;
        /// <value>string</value>
        public string ClientId { get; set; } = string.Empty;
        /// <value>string</value>
        public string ClientSecret { get; set; } = string.Empty;
    }
}
