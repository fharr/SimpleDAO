namespace SimpleDAO.InMemory
{
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        public IDataStore DataStore { get; private set; }

        public InMemoryUnitOfWork()
        {
            this.DataStore = new InMemoryDataStore();
        }

        public void Dispose()
        { }

        public void SaveChanges()
        { }
    }
}
