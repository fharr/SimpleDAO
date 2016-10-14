namespace SimpleDAO.Tests.DAL.EF6
{
    using EF6;
    using Mapping;
    using Domain;
    using SimpleDAO.EF6;

    public class CollectionRepository : EF6GenericRepository<Collection, CollectionDomain, TestUnitOfWork, Model>, ICollectionRepository
    {
        public CollectionRepository(TestUnitOfWork unitOfWork)
            : base(unitOfWork, (entity, domain) => entity.Id == domain.Id)
        { }
    }
}
