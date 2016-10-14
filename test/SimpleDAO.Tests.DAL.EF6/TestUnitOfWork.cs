namespace SimpleDAO.Tests.DAL.EF6
{
    using Mapping;
    using SimpleDAO.EF6;

    public class TestUnitOfWork : EF6UnitOfWork<Model>, ITestUnitOfWork
    {
        public ICollectionRepository CollectionRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }

        public TestUnitOfWork()
            : base(new Model())
        {
            this.CollectionRepository = new CollectionRepository(this);
            this.ProductRepository = new ProductRepository(this);
        }
    }
}
