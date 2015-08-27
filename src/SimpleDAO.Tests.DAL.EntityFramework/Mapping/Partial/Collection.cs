using AutoMapper;
using SimpleDAO.EntityFramework.Mapping;
using SimpleDAO.Tests.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDAO.Tests.DAL.EntityFramework.Mapping
{
    partial class Collection : AutoMappableEntity<Collection, CollectionDomain>
    {
        static Collection()
        {
            Initialize();

            Mapper.FindTypeMapFor<Collection, CollectionDomain>()
                .AddAfterMapAction((entity, domain) => (domain as CollectionDomain).NbProducts = (entity as Collection).Products.Count);
        }
    }
}
