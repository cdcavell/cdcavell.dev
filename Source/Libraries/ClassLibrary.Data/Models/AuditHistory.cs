using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
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
    /// | Christopher D. Cavell | 1.0.1.0 | 09/05/2022 | Data Access Layer Development |~ 
    /// </revision>
    [Table("AuditHistory")]
    public class AuditHistory : DataModel<AuditHistory>
    {
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string ModifiedBy { get; set; } = string.Empty;
        /// <value>DateTime?</value>
        [AllowNull]
        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; } = DateTime.Now;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string Application { get; set; } = string.Empty;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string Entity { get; set; } = string.Empty;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string State { get; set; } = string.Empty;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string KeyValues { get; set; } = string.Empty;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string OriginalValues { get; set; } = string.Empty;
        /// <value>string</value>
        [DataType(DataType.Text)]
        public string CurrentValues { get; set; } = string.Empty;
    }
}
