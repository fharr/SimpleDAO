namespace SimpleDAO.Tests.DAL.EntityFramework.Mapping
{
    using AutoMapper;
    using SimpleDAO.EntityFramework.Mapping;
    using Domain;

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
