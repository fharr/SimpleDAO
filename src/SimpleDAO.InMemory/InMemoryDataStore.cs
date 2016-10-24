namespace SimpleDAO.InMemory
{
    using System;

    public class InMemoryDataStore : IDataStore
    {
        public void CreateIfNotExists()
        { }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public bool Exists()
        {
            return true;
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
