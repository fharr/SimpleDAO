namespace SimpleDAO.EntityFramework
{
    using System;
    using System.Data.Entity;

    public class UnitOfWork : IUnitOfWork
    {
        #region properties

        public DbContext DbContext { get; private set; }

        #endregion

        #region constructors

        public UnitOfWork(DbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        #endregion

        #region methods

        public void SaveChanges()
        {
            this.DbContext.SaveChanges();
        }

        #endregion

        #region dispose

        public void Dispose()
        {
            this.DbContext.Dispose();
        }

        #endregion
    }
}
