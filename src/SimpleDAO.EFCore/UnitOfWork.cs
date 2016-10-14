namespace SimpleDAO.EntityFramework
{
    using Microsoft.EntityFrameworkCore;

    public class UnitOfWork<TDbContext> : IUnitOfWork
        where TDbContext : DbContext, new()
    {
        #region properties

        public TDbContext DbContext { get; private set; }

        #endregion

        #region constructors

        public UnitOfWork()
            : this(new TDbContext())
        { }

        public UnitOfWork(TDbContext dbContext)
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
