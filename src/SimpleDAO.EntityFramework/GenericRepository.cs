namespace SimpleDAO.EntityFramework
{
    using Mapping;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public abstract class GenericRepository<TEntity, TDomain, TDbContext> : IGenericRepository<TDomain>
        where TDbContext : DbContext
        where TEntity : class, IMappableEntity<TDomain>, new()
    {
        #region fields

        protected TDbContext context;
        protected DbSet<TEntity> dbSet;
        protected Func<TEntity, TDomain, bool> finder;

        #endregion fields

        #region constructor

        /// <summary>
        /// Creates a new repositoriy with the specified dbContext and key finder
        /// </summary>
        /// <param name="dbContext">the dbContext of the underlying database</param>
        /// <param name="finder">a function to identify the entity from the domain</param>
        public GenericRepository(TDbContext dbContext, Func<TEntity, TDomain, bool> finder)
        {
            this.context = dbContext;
            this.dbSet = dbContext.Set<TEntity>();
            this.finder = finder;
        }

        #endregion

        #region CRUD actions

        public void Create(TDomain domain)
        {
            bool isAttached;

            var entity = this.ToEntity(domain, out isAttached);

            if (isAttached)
            {
                throw new AlreadyExistingException<TDomain>(domain);
            }

            this.dbSet.Add(entity);
        }

        public TDomain GetById(params object[] keyValues)
        {
            var entity = this.dbSet.Find(keyValues);

            return entity.ToDomain();
        }

        public IList<TDomain> GetAll()
        {
            return this.dbSet.Select(entity => entity.ToDomain()).ToList();
        }

        public void Update(TDomain domain)
        {
            this.Attach(domain);
        }

        public void Remove(TDomain domain)
        {
            var entity = this.Attach(domain);

            this.dbSet.Remove(entity);
        }

        public void RemoveRange(IList<TDomain> list)
        {
            var entities = list.Select(domain => this.Attach(domain));

            this.dbSet.RemoveRange(entities);
        }

        #endregion

        #region other actions

        public int Count()
        {
            return this.dbSet.Count();
        }

        #endregion

        #region protected methods

        /// <summary>
        /// Converts the specified domain into an entity and attaches it to the context if needed
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        protected TEntity Attach(TDomain domain)
        {
            bool isAttached;

            var entity = this.ToEntity(domain, out isAttached);

            if (!isAttached)
                this.dbSet.Attach(entity);

            return entity;
        }

        /// <summary>
        /// Converts the specified domain into an entity
        /// </summary>
        /// <param name="domain">the domain to convert</param>
        /// <param name="isAttached">output parameter to know if the entity is already attached into the context</param>
        /// <returns></returns>
        protected TEntity ToEntity(TDomain domain, out bool isAttached)
        {
            var entity = this.dbSet.Local.SingleOrDefault(user => this.finder(user, domain));

            isAttached = entity != null;

            if (!isAttached)
                entity = new TEntity();

            entity.FillWith(domain);

            return entity;
        }

        #endregion
    }
}
