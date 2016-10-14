namespace SimpleDAO.Tests.DAL.EF6
{
    using Mapping;
    using Domain;
    using EF6;
    using SimpleDAO.EF6;

    public class ProductRepository : EF6GenericRepository<Product, ProductDomain, TestUnitOfWork, Model>, IProductRepository
    {
        public ProductRepository(TestUnitOfWork unitOfWork)
            : base(unitOfWork, (entity, domain) => entity.Id == domain.Id)
        { }
    }
}
