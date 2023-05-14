namespace gam.cdcavell.dev.Models.AppSettings
{
    /// <summary>
    /// Authentication model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.5.0 | 05/11/2023 | Game Development - Sudoku |~ 
    /// </revision>
    public class Authentication
    {
        /// <value>IdP</value>
        public IdP IdP { get; set; } = new();
        /// <value>reCAPTCHA</value>
        public reCAPTCHA reCAPTCHA { get; set; } = new();
    }
}
