using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClassLibrary.Mvc.Models.Home
{
    /// <class>CultureModel</class>
    /// <summary>
    /// Culture model class
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 11/23/2022 | User Registration Development |~ 
    /// | Christopher D. Cavell | 1.0.0.0 | 08/20/2022 | Initial Development |~ 
    /// </revision>
    public class CultureModel
    {
        /// <value>string</value>
        public const string BindProperties = "Culture, ReturnUrl";

        /// <value>string</value>
        [Required]
        [FromForm(Name = "Culture")]
        public string Culture { get; set; } = string.Empty;
        /// <value>string</value>
        [Required]
        [FromForm(Name = "ReturnUrl")]
        public string ReturnUrl { get; set; } = string.Empty;
    }
}
