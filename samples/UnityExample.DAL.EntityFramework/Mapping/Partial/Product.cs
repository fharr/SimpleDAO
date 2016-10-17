using AutoMapper;
using SimpleDAO.EntityFramework.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityExample.Domain;

namespace UnityExample.DAL.EntityFramework.Mapping
{
    partial class Product : AutoMappableEntity<Product, ProductDomain>
    {
        static Product()
        {
            Initialize();

            Mapper.FindTypeMapFor<Product, ProductDomain>()
                .AddAfterMapAction((entity, domain) => (domain as ProductDomain).CollectionName = (entity as Product).Collection.Name);
        }
    }
}
