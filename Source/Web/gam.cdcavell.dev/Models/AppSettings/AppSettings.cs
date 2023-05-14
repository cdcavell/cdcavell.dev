namespace gam.cdcavell.dev.Models.AppSettings
{
    /// <summary>
    /// Application settings information.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.5.0 | 05/11/2023 | Game Development - Sudoku |~ 
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
