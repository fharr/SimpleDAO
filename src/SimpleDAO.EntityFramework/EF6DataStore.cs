namespace SimpleDAO.EF6
{
    using System.Data.Entity;

    public class EF6DataStore : IDataStore
    {
        #region private fields

        private DbContext _dbContext;

        #endregion

        #region constructors

        public EF6DataStore(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region methods

        public void CreateIfNotExists()
        {
            _dbContext.Database.CreateIfNotExists();
        }

        public void Delete()
        {
            _dbContext.Database.Delete();
        }

        public bool Exists()
        {
            return _dbContext.Database.Exists();
        }

        public bool IsValid()
        {
            return _dbContext.Database.CompatibleWithModel(true);
        }

        #endregion
    }
}
