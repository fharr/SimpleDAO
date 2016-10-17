using SimpleDAO.EFCore;
using SimpleDAO.Tests.DAL.EFCore.Mapping;
using SimpleDAO.Tests.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleDAO.Tests.DAL.EFCore
{
    public class CollectionRepository : EFCoreGenericRepository<Collection, CollectionDomain, TestEFCoreUnitOfWork, DbModel>, ICollectionRepository
    {
        public CollectionRepository(TestEFCoreUnitOfWork unitOfWork)
            : base(unitOfWork, (entity, domain) => entity.Id == domain.Id)
        { }
    }
}
