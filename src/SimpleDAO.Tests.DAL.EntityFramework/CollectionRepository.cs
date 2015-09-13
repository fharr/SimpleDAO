namespace SimpleDAO.Tests.DAL.EntityFramework
{
    using SimpleDAO.EntityFramework;
    using Mapping;
    using Domain;

    public class CollectionRepository : GenericRepository<Collection, CollectionDomain, EFTestUnitOfWork, DbEntities>, ICollectionRepository
    {
        public CollectionRepository(EFTestUnitOfWork unitOfWork)
            : base(unitOfWork, (entity,domain) => entity.Id == domain.Id)
        { }
    }
}
