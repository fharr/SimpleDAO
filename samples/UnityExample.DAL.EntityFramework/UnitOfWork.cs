using SimpleDAO.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityExample.DAL.EntityFramework.Mapping;

namespace UnityExample.DAL.EntityFramework
{
    public class UnitOfWork : UnitOfWork<Entities>, IUnitOfWork
    {
        public ICollectionRepository CollectionRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }

        public UnitOfWork(Entities dbContext)
            : base(dbContext)
        {
            this.CollectionRepository = new CollectionRepository(this);
            this.ProductRepository = new ProductRepository(this);
        }
    }
}
