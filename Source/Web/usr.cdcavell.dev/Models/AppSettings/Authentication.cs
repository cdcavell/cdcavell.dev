namespace usr.cdcavell.dev.Models.AppSettings
{
    /// <summary>
    /// Authentication model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/29/2022 | User Role Claims Development |~ 
    /// </revision>
    public class Authentication
    {
        /// <value>IdP</value>
        public IdP IdP { get; set; } = new();
        /// <value>reCAPTCHA</value>
        public reCAPTCHA reCAPTCHA { get; set; } = new();
    }
}
