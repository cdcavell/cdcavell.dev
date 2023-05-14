namespace dis.cdcavell.dev.Models.AppSettings
{
    /// <summary>
    /// OAuth Authentication model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 11/11/2022 | User Registration Development |~ 
    /// </revision>
    public class OAuth
    {
        /// <value>string</value>
        public string AuthorizationEndpoint { get; set; } = string.Empty;
        /// <value>string</value>
        public string TokenEndpoint { get; set; } = string.Empty;
        /// <value>string</value>
        public string UserInformationEndpoint { get; set; } = string.Empty;
        /// <value>string</value>
        public string ClientId { get; set; } = string.Empty;
        /// <value>string</value>
        public string ClientSecret { get; set; } = string.Empty;
    }
}
