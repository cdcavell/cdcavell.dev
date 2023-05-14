namespace dis.cdcavell.dev.Models.Grants
{
    /// <summary>
    /// Grant View Model
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.2.0 | 09/24/2022 | Duende IdentityServer Development |~ 
    /// </revision>
    public class GrantViewModel
    {
        /// <value>string</value>
        public string? ClientId { get; set; }
        /// <value>string</value>
        public string? ClientName { get; set; }
        /// <value>string</value>
        public string? ClientUrl { get; set; }
        /// <value>string</value>
        public string? ClientLogoUrl { get; set; }
        /// <value>string</value>
        public string? Description { get; set; }
        /// <value>DateTime</value>
        public DateTime Created { get; set; }
        /// <value>DateTime?</value>
        public DateTime? Expires { get; set; }
        /// <value>IEnumerable&lt;string&gt;</value>
        public IEnumerable<string> IdentityGrantNames { get; set; } = new List<string>();
        /// <value>IEnumerable&lt;string&gt;</value>
        public IEnumerable<string> ApiGrantNames { get; set; } = new List<string>();
    }
}
