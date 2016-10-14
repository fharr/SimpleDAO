namespace SimpleDAO.Tests.DAL.EntityFramework
{
    using SimpleDAO.EntityFramework;
    using Mapping;

    public class EFTestUnitOfWork : UnitOfWork<DbEntities>, ITestUnitOfWork
    {
        public ICollectionRepository CollectionRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }

        public EFTestUnitOfWork()
            : base(new DbEntities())
        {
            this.CollectionRepository = new CollectionRepository(this);
            this.ProductRepository = new ProductRepository(this);
        }
    }
}
