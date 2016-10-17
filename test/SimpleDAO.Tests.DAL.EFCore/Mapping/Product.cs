using SimpleDAO.EFCore.Mapping;
using SimpleDAO.Tests.Domain;
using System;
using System.Collections.Generic;

namespace SimpleDAO.Tests.DAL.EFCore.Mapping
{
    public partial class Product : AutoMappableEntity<Product, ProductDomain>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public virtual Collection Collection { get; set; }
    }
}
