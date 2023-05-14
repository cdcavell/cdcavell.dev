namespace cdcavell.dev.Models.AppSettings
{
    /// <summary>
    /// Application settings information.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 11/12/2022 | User Registration Development |~ 
    /// | Christopher D. Cavell | 1.0.2.0 | 10/01/2022 | Duende IdentityServer Development |~ 
    /// | Christopher D. Cavell | 1.0.0.0 | 08/20/2021 | Initial Development |~ 
    /// </revision>
    public class AppSettings : ClassLibrary.Mvc.Services.AppSettings.Models.AppSettings
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">IConfiguration</param>
        /// <method>AppSettings(IConfiguration configuration)</method>
        public AppSettings(IConfiguration configuration) : base(configuration) { }

        /// <value>Authentication</value>
        public Authentication Authentication { get; set; } = new();
    }
}
