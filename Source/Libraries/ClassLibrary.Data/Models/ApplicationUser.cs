using Microsoft.AspNetCore.Identity;

namespace ClassLibrary.Data.Models
{
    /// <summary>
    /// ApplicationUser Status Enum
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 11/13/2022 | User Registration Development |~ 
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

    /// <summary>
    /// ApplicationUser Entity
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 10/03/2022 | User Registration Development |~ 
    /// | Christopher D. Cavell | 1.0.1.0 | 09/05/2022 | Data Access Layer Development |~ 
    /// </revision>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Initializes a user identity
        /// </summary>
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        /// <value>string</value>
        public string SubjectId { get => this.Id; }

        /// <value>enum</value>
        public UserStatus Status { get; set; } = UserStatus.New;

        /// <value>string</value>
        public string DisplayName { get; set; } = "Anonymous";
        /// <value>ICollection&lt;IdentityUserClaim&lt;string&gt;&gt;</value>
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; } = new List<IdentityUserClaim<string>>();
    }
}
