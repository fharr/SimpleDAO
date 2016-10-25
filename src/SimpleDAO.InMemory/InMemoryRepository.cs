namespace SimpleDAO.InMemory
{
    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class InMemoryRepository<T> : IGenericRepository<T>
        where T : new()
    {
        #region static constructor

        static InMemoryRepository()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<T, T>());
        }

        #endregion

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

        public abstract T GetById(params object[] keyValues);

        public void Remove(T domain)
        {
            _livingCollection.Remove(domain);
        }

        public void RemoveRange(IList<T> list)
        {
            foreach (var domain in list)
            {
                Remove(domain);
            }
        }

        public void Update(T domain)
        {
            var entity = Mapper.Map<T>(domain);

            _livingCollection[domain] = entity;
        }

        #endregion
    }
}
