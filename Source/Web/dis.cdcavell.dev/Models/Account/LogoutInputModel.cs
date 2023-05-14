namespace dis.cdcavell.dev.Models.Account
{
    /// <summary>
    /// Logout Input Model
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.2.0 | 10/08/2022 | Duende IdentityServer Development |~ 
    /// </revision>
    public class LogoutInputModel
    {
        /// <value>string</value>
        public const string BindProperties = "LogoutId";

        /// <value>string</value>
        public string? LogoutId { get; set; }
    }
}
