using SimpleDAO.EntityFramework;
using SimpleDAO.Tests.DAL.EntityFramework.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDAO.Tests.DAL.EntityFramework
{
    public class EFTestUnitOfWork : UnitOfWork, ITestUnitOfWork
    {
        public ICollectionRepository CollectionRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }

        public EFTestUnitOfWork()
            : base(new DbEntities())
        {
            this.CollectionRepository = new CollectionRepository(this.DbContext as DbEntities);
            this.ProductRepository = new ProductRepository(this.DbContext as DbEntities);
        }
    }
}
