namespace SimpleDAO.EFCore
{
    using System;
    using Microsoft.EntityFrameworkCore;

    public class UnitOfWork<TDbContext> : IUnitOfWork
        where TDbContext : DbContext, new()
    {
        #region properties

        public TDbContext DbContext { get; private set; }

        public IDataStore DataStore { get; private set; }

        #endregion

        #region constructors

        public UnitOfWork()
            : this(new TDbContext())
        { }

        public UnitOfWork(TDbContext dbContext)
        {
            this.DbContext = dbContext;
            this.DataStore = new EFCoreDataStore(dbContext);
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
