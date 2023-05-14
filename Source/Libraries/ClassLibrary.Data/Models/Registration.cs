using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ClassLibrary.Data.Models
{
    /// <summary>
    /// AuditHistory Entity Model
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.3.0 | 11/13/2022 | User Registration Development |~ 
    /// </revision>
    [Table("Registration")]
    public class Registration : DataModel<Registration>
    {
        /// <value>string</value>
        public string UserId { get; set; } = string.Empty;

        /// <value>string</value>
        public string Email { get; set; } = string.Empty;

        /// <value>string</value>
        public string DisplayName { get; set; } = string.Empty;

        /// <value>string</value>
        public string Provider { get; set; } = string.Empty;

        /// <value>string</value>
        public string ProviderUserId { get; set; } = string.Empty;

        /// <value>DateTime</value>
        [DataType(DataType.DateTime)]
        public DateTime GeneratedOn { get; set; } = DateTime.Now;
    }
}
