namespace SimpleDAO.InMemory
{
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class InMemoryRepository<T> : IGenericRepository<T>
        where T : new()
    {
        #region fields

        protected Dictionary<T, T> _livingCollection;

        #endregion

        #region constructor

        public InMemoryRepository()
        {
            _livingCollection = new Dictionary<T, T>();
        }

        #endregion

        #region methods

        public int Count()
        {
            return _livingCollection.Count;
        }

        public void Create(T domain)
        {
            var entity = Mapper.Map<T>(domain);

            _livingCollection.Add(domain, entity);
        }

        public IList<T> GetAll()
        {
            return _livingCollection.Values.ToList();
        }

        public T GetById(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public void Remove(T domain)
        {
            _livingCollection.Remove(domain);
        }

        public void RemoveRange(IList<T> list)
        {
            throw new NotImplementedException();
        }

        public void Update(T domain)
        {
            var entity = Mapper.Map<T>(domain);

            _livingCollection[domain] = entity;
        }

        #endregion
    }
}
