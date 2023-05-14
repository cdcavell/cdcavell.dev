namespace cdcavell.dev.Models.AppSettings
{
    /// <summary>
    /// IdP model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 11/12/2022 | User Registration Development |~ 
    /// | Christopher D. Cavell | 1.0.2.0 | 10/01/2022 | Duende IdentityServer Development |~ 
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
