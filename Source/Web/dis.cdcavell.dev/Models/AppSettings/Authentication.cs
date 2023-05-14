namespace dis.cdcavell.dev.Models.AppSettings
{
    /// <summary>
    /// Authentication model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/29/2022 | User Role Claims Development |~ 
    /// | Christopher D. Cavell | 1.0.3.3 | 12/14/2022 | Migrate to universeodon.com |~ 
    /// | Christopher D. Cavell | 1.0.3.0 | 11/12/2022 | User Registration Development |~ 
    /// | Christopher D. Cavell | 1.0.2.0 | 10/02/2022 | Duende IdentityServer Development |~ 
    /// </revision>
    public class Authentication
    {
        /// <value>IdP</value>
        public IdP IdP { get; set; } = new();
        /// <value>OAuth</value>
        public OAuth Universeodon { get; set; } = new();
        /// <value>GitHub</value>
        public GitHub GitHub { get; set; } = new();
        /// <value>Microsoft</value>
        public Microsoft Microsoft { get; set; } = new();
        /// <value>Googel</value>
        public Google Google { get; set; } = new();
        /// <value>reCAPTCHA</value>
        public reCAPTCHA reCAPTCHA { get; set; } = new();
    }
}
