using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace usr.cdcavell.dev.Models.Apis.Role
{
    /// <summary>
    /// Role Claims Model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 01/01/2023 | User Role Claims Development |~ 
    /// </revision>
    public class RoleClaimsModel : IdentityRole
    {
        /// <value>List&lt;Claim&gt;</value>
        public List<Claim> Claims { get; set; } = new();
    }
}
