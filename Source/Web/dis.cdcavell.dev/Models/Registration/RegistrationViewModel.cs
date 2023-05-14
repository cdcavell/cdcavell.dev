using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace dis.cdcavell.dev.Models.Registration
{
    /// <summary>
    /// Registration View Model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.4.0 | 12/30/2022 | User Role Claims Development |~ 
    /// | Christopher D. Cavell | 1.0.3.0 | 11/12/2022 | User Registration Development |~ 
    /// </revision>
    public class RegistrationViewModel
    {
        /// <value>string</value>
        public const string BindProperties = "DisplayName, Email, LockNameEmail, Id, Provider, ProviderUserId, IsSubmit";

        /// <value>string</value>
        [Required(ErrorMessage = "ErrorMessage.Required")]
        [FromForm(Name = "DisplayName")]
        public string DisplayName { get; set; } = string.Empty;
        /// <value>string</value>
        [Required(ErrorMessage = "ErrorMessage.Required")]
        [EmailAddress(ErrorMessage = "ErrorMessage.Invalid")]
        [FromForm(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        /// <value>bool</value>
        [FromForm(Name = "LockNameEmail")]
        public bool LockNameEmail { get; set; } = false;
        /// <value>string</value>
        [FromForm(Name = "Id")]
        public string Id { get; set; } = string.Empty;
        /// <value>string</value>
        [FromForm(Name = "Provider")]
        public string Provider { get; set; } = string.Empty;
        /// <value>string</value>
        [FromForm(Name = "ProviderUserId")]
        public string ProviderUserId { get; set; } = string.Empty;
        /// <value>bool</value>
        [FromForm(Name = "IsSubmit")]
        public bool IsSubmit { get; set; } = false;
    }
}
