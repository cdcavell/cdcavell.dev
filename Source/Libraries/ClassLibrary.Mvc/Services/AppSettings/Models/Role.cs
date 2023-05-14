using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Mvc.Services.AppSettings.Models
{
    /// <summary>
    /// Role information.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/24/2022 | User Role Claims Development |~ 
    /// </revision>
    public class Role
    {
        /// <value>string</value>
        public string Name { get; set; } = string.Empty;

        /// <value>List&lt;string&gt;</value>
        public List<string> Claims { get; set; } = new();
    }
}
