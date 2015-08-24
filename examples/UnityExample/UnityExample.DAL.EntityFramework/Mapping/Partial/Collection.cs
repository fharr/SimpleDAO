using SimpleDAO.EntityFramework.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityExample.Domain;

namespace UnityExample.DAL.EntityFramework.Mapping
{
    partial class Collection : AutoMappableEntity<Collection, CollectionDomain>
    { }
}
