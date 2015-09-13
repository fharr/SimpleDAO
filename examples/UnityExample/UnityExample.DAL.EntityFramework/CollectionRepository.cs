using SimpleDAO.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityExample.DAL.EntityFramework.Mapping;
using UnityExample.Domain;

namespace UnityExample.DAL.EntityFramework
{
    public class CollectionRepository : GenericRepository<Collection, CollectionDomain, UnitOfWork, Entities>, ICollectionRepository
    {
        public CollectionRepository(UnitOfWork unitOfWork)
            : base(unitOfWork, (entity, domain) => entity.Id == domain.Id)
        { }
    }
}
