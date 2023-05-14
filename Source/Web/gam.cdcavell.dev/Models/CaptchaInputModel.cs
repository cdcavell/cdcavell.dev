using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace gam.cdcavell.dev.Models
{
    /// <summary>
    /// Captcha input model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.5.0 | 05/11/2023 | Game Development - Sudoku |~ 
    /// </revision>
    public class CaptchaInputModel
    {
        /// <value>string</value>
        public const string BindProperties = "CaptchaToken";

        /// <value>string</value>
        [Required]
        [FromForm(Name = "CaptchaToken")]
        public string CaptchaToken { get; set; } = string.Empty;
    }
}
