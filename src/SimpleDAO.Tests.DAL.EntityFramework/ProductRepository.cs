﻿using SimpleDAO.EntityFramework;
using SimpleDAO.Tests.DAL.EntityFramework.Mapping;
using SimpleDAO.Tests.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDAO.Tests.DAL.EntityFramework
{
    public class ProductRepository : GenericRepository<Product, ProductDomain, DbEntities>, IProductRepository
    {
        public ProductRepository(DbEntities dbContext)
            : base(dbContext, (entity,domain) => entity.Id == domain.Id)
        { }
    }
}
