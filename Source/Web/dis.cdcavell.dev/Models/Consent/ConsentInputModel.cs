using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace dis.cdcavell.dev.Models.Consent
{
    /// <summary>
    /// Consent Input Model
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
    public class ConsentInputModel
    {
        /// <value>string</value>
        public const string BindProperties = "Button, ScopesConsented, RememberConsent, ReturnUrl, Description";

        /// <value>string</value>
        [FromForm(Name = "Button")]
        public string? Button { get; set; }
        /// <value>IEnumerable&lt;string&gt;</value>
        [FromForm(Name = "ScopesConsented")]
        public IEnumerable<string>? ScopesConsented { get; set; }
        /// <value>bool</value>
        [FromForm(Name = "RememberConsent")]
        public bool RememberConsent { get; set; }
        /// <value>string</value>
        [FromForm(Name = "ReturnUrl")]
        public string? ReturnUrl { get; set; }
        /// <value>string</value>
        [FromForm(Name = "Description")]
        public string? Description { get; set; }
    }
}
