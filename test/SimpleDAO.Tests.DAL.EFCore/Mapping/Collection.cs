using SimpleDAO.EFCore.Mapping;
using SimpleDAO.Tests.Domain;
using System;
using System.Collections.Generic;

namespace SimpleDAO.Tests.DAL.EFCore.Mapping
{
    public partial class Collection : AutoMappableEntity<Collection, CollectionDomain>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
