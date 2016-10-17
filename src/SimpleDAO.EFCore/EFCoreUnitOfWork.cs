namespace SimpleDAO.EFCore
{
    using Microsoft.EntityFrameworkCore;

    public class EFCoreUnitOfWork<TDbContext> : IUnitOfWork
        where TDbContext : DbContext, new()
    {
        #region properties

        public TDbContext DbContext { get; private set; }

        public IDataStore DataStore { get; private set; }

        #endregion

        #region constructors

        public EFCoreUnitOfWork()
            : this(new TDbContext())
        { }

        public EFCoreUnitOfWork(TDbContext dbContext)
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
