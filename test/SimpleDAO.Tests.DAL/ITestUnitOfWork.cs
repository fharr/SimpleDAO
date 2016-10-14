namespace SimpleDAO.Tests.DAL
{
    public interface ITestUnitOfWork : IUnitOfWork
    {
        IProductRepository ProductRepository { get; set; }
        ICollectionRepository CollectionRepository { get; set; }
    }
}
