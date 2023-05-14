﻿namespace gam.cdcavell.dev.Models
{
    /// <summary>
    /// Captcha response model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.5.0 | 05/11/2023 | Game Development - Sudoku |~ 
    /// </revision>
    public class CaptchaResponse
    {
        /// <value>bool</value>
        public bool success { get; set; }
        /// <value>DateTime</value>
        public DateTime challenge_ts { get; set; }
        /// <value>string</value>
        public string hostname { get; set; } = string.Empty;
        /// <value>double</value>
        public double score { get; set; }
        /// <value>string</value>
        public string action { get; set; } = string.Empty;
        /// <value>object[]</value>
        public object[] ErrorCodes { get; set; } = { };
    }
}
