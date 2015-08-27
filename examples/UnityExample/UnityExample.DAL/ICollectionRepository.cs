using SimpleDAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityExample.Domain;

namespace UnityExample.DAL
{
    public interface ICollectionRepository : IGenericRepository<CollectionDomain>
    { }
}
