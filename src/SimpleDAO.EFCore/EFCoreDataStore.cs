using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleDAO.EFCore
{
    public class EFCoreDataStore : IDataStore
    {
        #region private fields

        private DbContext _dbContext;

        #endregion

        #region constructors

        internal EFCoreDataStore(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region methods

        public void CreateIfNotExists()
        {
            _dbContext.Database.EnsureCreated();
        }

        public void Delete()
        {
            _dbContext.Database.EnsureDeleted();
        }

        public bool Exists()
        {
            throw new NotSupportedException();
        }

        public bool IsValid()
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
