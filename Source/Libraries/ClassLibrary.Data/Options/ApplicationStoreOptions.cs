using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Data.Options
{
    /// <summary>
    /// Options for configuring ApplicationDbContext
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.1.0 | 09/05/2022 | Data Access Layer Development |~ 
    /// </revision>
    public class ApplicationStoreOptions
    {
        /// <summary>
        /// Callback to configure the EF DbContext.
        /// </summary>
        /// <value>
        /// The configure database context.
        /// </value>
        public Action<DbContextOptionsBuilder>? ConfigureDbContext { get; set; }

        /// <summary>
        /// Callback in DI resolve the EF DbContextOptions. If set, ConfigureDbContext will not be used.
        /// </summary>
        /// <value>
        /// The configure database context.
        /// </value>
        public Action<IServiceProvider, DbContextOptionsBuilder>? ResolveDbContextOptions { get; set; }

        /// <summary>
        /// Gets or sets the default schema.
        /// </summary>
        /// <value>
        /// The default schema.
        /// </value>
        public string? DefaultSchema { get; set; } = null;

        /// <summary>
        /// Gets or sets the AuditHistory table configuration.
        /// </summary>
        /// <value>
        /// The AuditHistory.
        /// </value>
        public TableConfiguration AuditHistory { get; set; } = new TableConfiguration("AuditHistory");

        /// <summary>
        /// Gets or set if EF DbContext pooling is enabled.
        /// </summary>
        public bool EnablePooling { get; set; } = false;

        /// <summary>
        /// Gets or set the pool size to use when DbContext pooling is enabled. If not set, the EF default is used.
        /// </summary>
        public int? PoolSize { get; set; }
    }
}
