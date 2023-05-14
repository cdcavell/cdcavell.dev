using System.Security.Claims;

namespace usr.cdcavell.dev.Models.Apis.User
{
    /// <summary>
    /// User Claims Model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 02/21/2023 | User Role Claims Development |~ 
    /// </revision>
    public class UserRolesModel
    {
        /// <value>string</value>
        public string SubjectId { get; set; } = string.Empty;

        /// <value>enum</value>
        public UserStatus Status { get; set; } = UserStatus.New;

        /// <value>string</value>
        public string DisplayName { get; set; } = string.Empty;

        /// <value>string</value>
        public string Email { get; set; } = string.Empty;

        /// <value>List&lt;string&gt;</value>
        public List<string> Roles { get; set; } = new();

        ///<value>bool</value>
        public bool LockNameEmail { get; set; } = true;
    }
}
