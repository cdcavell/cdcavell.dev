using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace dis.cdcavell.dev.Models.Account
{
    /// <summary>
    /// Login Input Model
    /// &lt;br /&gt;&lt;br /&gt;
    /// Copyright (c) Duende Software. All rights reserved.
    /// See https://duendesoftware.com/license/identityserver.pdf for license information. 
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 11/12/2022 | User Registration Development |~ 
    /// | Christopher D. Cavell | 1.0.2.0 | 09/24/2022 | Duende IdentityServer Development |~ 
    /// </revision>
    public class LoginInputModel
    {
        /// <value>string</value>
        public const string BindProperties = "Username, Password, RememberLogin, ReturnUrl";

        /// <value>string</value>
        [Required(ErrorMessage = "ErrorMessage.Required")]
        [EmailAddress(ErrorMessage = "ErrorMessage.Invalid")]
        [FromForm(Name = "Username")]
        public string? Username { get; set; }
        /// <value>string</value>
        [Required(ErrorMessage = "ErrorMessage.Required")]
        [FromForm(Name = "Password")]
        public string? Password { get; set; }
        /// <value>bool</value>
        [FromForm(Name = "RememberLogin")]
        public bool RememberLogin { get; set; }
        /// <value>string</value>
        [FromForm(Name = "ReturnUrl")]
        public string? ReturnUrl { get; set; }
    }
}
