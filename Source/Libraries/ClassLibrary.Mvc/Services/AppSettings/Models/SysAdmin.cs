using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Mvc.Services.AppSettings.Models
{
    /// <summary>
    /// System Administrator information.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/25/2022 | User Role Claims Development |~ 
    /// </revision>
    public class SysAdmin
    {
        /// <value>string</value>
        public string Email { get; set; } = string.Empty;
    }
}
