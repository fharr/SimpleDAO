namespace SimpleDAO.EF6
{
    using System.Data.Entity;

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
            this.DataStore = new EF6DataStore(dbContext);
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
