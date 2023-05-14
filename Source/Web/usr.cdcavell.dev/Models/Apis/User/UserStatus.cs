namespace usr.cdcavell.dev.Models.Apis.User
{
    /// <summary>
    /// ApplicationUser Status Enum
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 02/21/2023 | User Role Claims Development |~ 
    /// </revision>
    public enum UserStatus
    {
        /// <summary>New User</summary>
        New,
        /// <summary>Pending Registration Validation</summary>
        Pending,
        /// <summary>Active User</summary>
        Active,
        /// <summary>Disabled User</summary>
        Disabled
    }
}
