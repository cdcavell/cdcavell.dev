using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ClassLibrary.Data.Options;
using ClassLibrary.Data.Models;

namespace ClassLibrary.Data.Extensions
{
    /// <summary>
    /// Extension methods to define the database schema for the application data stores.
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.1.0 | 09/05/2022 | Data Access Layer Development |~ 
    /// </revision>
    public static class ModelBuilderExtensions
    {
        private static EntityTypeBuilder<TEntity> ToTable<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, TableConfiguration configuration)
            where TEntity : class
        {
            return string.IsNullOrWhiteSpace(configuration.Schema) ? entityTypeBuilder.ToTable(configuration.Name) : entityTypeBuilder.ToTable(configuration.Name, configuration.Schema);
        }

        /// <summary>
        /// Configures the AuditHistory context.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <param name="storeOptions">The store options.</param>
        public static void ConfigureApplicationContext(this ModelBuilder modelBuilder, ApplicationStoreOptions storeOptions)
        {
            if (!string.IsNullOrWhiteSpace(storeOptions.DefaultSchema)) modelBuilder.HasDefaultSchema(storeOptions.DefaultSchema);

            modelBuilder.Entity<AuditHistory>(entity =>
            {
                entity.ToTable(storeOptions.AuditHistory);
                entity.HasKey(x => x.Guid);

                entity.Property(x => x.ModifiedBy).HasMaxLength(200).IsRequired();
                entity.Property(x => x.ModifiedOn).IsRequired();
                entity.Property(x => x.Application).HasMaxLength(200).IsRequired();
                entity.Property(x => x.Entity).HasMaxLength(64).IsRequired();
                entity.Property(x => x.State).HasMaxLength(20).IsRequired();
                entity.Property(x => x.KeyValues).HasMaxLength(Int32.MaxValue).IsRequired();
                entity.Property(x => x.OriginalValues).HasMaxLength(Int32.MaxValue).IsRequired();
                entity.Property(x => x.CurrentValues).HasMaxLength(Int32.MaxValue).IsRequired();
            });

        }
    }
}
