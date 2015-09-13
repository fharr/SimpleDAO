namespace SimpleDAO.Tests.DAL.EntityFramework.Mapping
{
    using AutoMapper;
    using SimpleDAO.EntityFramework.Mapping;
    using Domain;

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
