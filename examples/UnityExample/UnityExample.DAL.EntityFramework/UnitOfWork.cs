using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityExample.DAL.EntityFramework.Mapping;

namespace UnityExample.DAL.EntityFramework
{
    public class UnitOfWork : SimpleDAO.EntityFramework.UnitOfWork, IUnitOfWork
    {
        public ICollectionRepository CollectionRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }

        public UnitOfWork(Entities dbContext, ICollectionRepository collectionRepository, IProductRepository productRepository)
            : base(dbContext)
        {
            this.CollectionRepository = collectionRepository;
            this.ProductRepository = productRepository;
        }
    }
}
