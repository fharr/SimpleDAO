using SimpleDAO.EFCore;
using SimpleDAO.Tests.DAL.EFCore.Mapping;
using SimpleDAO.Tests.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleDAO.Tests.DAL.EFCore
{
    public class ProductRepository : EFCoreGenericRepository<Product, ProductDomain, TestEFCoreUnitOfWork, DbModel>, IProductRepository
    {
        public ProductRepository(TestEFCoreUnitOfWork unitOfWork)
            : base(unitOfWork, (entity, domain) => entity.Id == domain.Id)
        { }
    }
}
