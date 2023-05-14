using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClassLibrary.Data.Models
{
    /// <summary>
    /// DataModel Class
    /// </summary>
    /// <revision>
    /// __Revisions:__~~
    /// | Contributor | Build | Revison Date | Description |~
    /// |-------------|-------|--------------|-------------|~
    /// | Christopher D. Cavell | 1.0.1.0 | 09/05/2022 | Data Access Layer Development |~ 
    /// </revision>
    public abstract partial class DataModel<T> : IDataModel<DataModel<T>> where T : DataModel<T>
    {
        /// <value>Guid</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; } = Guid.Empty;

        /// <value>bool</value>
        [NotMapped]
        public bool IsNew
        {
            get
            {
                if (this.Guid == Guid.Empty)
                    return true;

                return false;
            }
        }

        /// <summary>
        /// Method used to add new or update existing entity record.
        /// </summary>
        /// <param name="dbContext">ApplicationDbContext</param>
        /// <method>AddUpdate(ApplicationDbContext dbContext)</method>
        public virtual void AddUpdate(ApplicationDbContext dbContext)
        {
            var dbContextTransaction = dbContext.Database.CurrentTransaction;
            if (dbContextTransaction == null)
            {
                dbContextTransaction = dbContext.Database.BeginTransaction();
                using (dbContextTransaction)
                {
                    try
                    {
                        InternalAddUpdate(dbContext);
                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                InternalAddUpdate(dbContext);
            }
        }

        private void InternalAddUpdate(ApplicationDbContext dbContext)
        {
            if (this.IsNew)
            {
                this.Guid = Guid.NewGuid();
                dbContext.Add<DataModel<T>>(this);
            }
            else
                dbContext.Update<DataModel<T>>(this);

            if (dbContext.HasUnsavedChanges())
            {
                dbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Method used to remove existing entity record.
        /// </summary>
        /// <param name="dbContext">ApplicationDbContext</param>
        /// <method>Delete(ApplicationDbContext dbContext)</method>
        public virtual void Delete(ApplicationDbContext dbContext)
        {
            if (!this.IsNew)
            {
                var dbContextTransaction = dbContext.Database.CurrentTransaction;
                if (dbContextTransaction == null)
                {
                    dbContextTransaction = dbContext.Database.BeginTransaction();
                    using (dbContextTransaction)
                    {
                        try
                        {
                            InternalDelete(dbContext);
                            dbContextTransaction.Commit();
                        }
                        catch (Exception)
                        {
                            dbContextTransaction.Rollback();
                            throw;
                        }
                    }
                }
                else
                {
                    InternalDelete(dbContext);
                }
            }
        }

        private void InternalDelete(ApplicationDbContext dbContext)
        {
            dbContext.Attach<DataModel<T>>(this);
            dbContext.Remove<DataModel<T>>(this);
            dbContext.SaveChanges();
        }

        /// <summary>
        /// Equality method.
        /// </summary>
        /// <param name="obj">DataModel&lt;T&gt;</param>
        /// <method>Equals(DataModel&lt;T&gt; obj)</method>
        public virtual bool Equals(DataModel<T> obj)
        {
            return (this == obj);
        }
    }
}
