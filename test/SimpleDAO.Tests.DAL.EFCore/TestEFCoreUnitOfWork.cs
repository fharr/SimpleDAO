using SimpleDAO.EFCore;
using SimpleDAO.Tests.DAL.EFCore.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleDAO.Tests.DAL.EFCore
{
    public class TestEFCoreUnitOfWork : EFCoreUnitOfWork<DbModel>, ITestUnitOfWork
    {
        public TestEFCoreUnitOfWork()
            : base(new DbModel())
        {
            CollectionRepository = new CollectionRepository(this);
            ProductRepository = new ProductRepository(this);
        }

        public ICollectionRepository CollectionRepository { get; set; }

        public IProductRepository ProductRepository { get; set; }
    }
}
