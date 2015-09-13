namespace SimpleDAO.Tests.DAL.EntityFramework
{
    using SimpleDAO.EntityFramework;
    using Mapping;
    using Domain;

    public class ProductRepository : GenericRepository<Product, ProductDomain, EFTestUnitOfWork, DbEntities>, IProductRepository
    {
        public ProductRepository(EFTestUnitOfWork unitOfWork)
            : base(unitOfWork, (entity,domain) => entity.Id == domain.Id)
        { }
    }
}
