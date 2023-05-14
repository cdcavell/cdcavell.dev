namespace dis.cdcavell.dev.Models.AppSettings
{
    /// <summary>
    /// Microsoft Authentication model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 11/11/2022 | User Registration Development |~ 
    /// </revision>
    public class Microsoft
    {
        /// <value>string</value>
        public string ClientId { get; set; } = string.Empty;
        /// <value>string</value>
        public string ClientSecret { get; set; } = string.Empty;
    }
}
