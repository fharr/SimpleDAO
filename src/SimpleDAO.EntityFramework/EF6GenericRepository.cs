namespace SimpleDAO.EF6
{
    using Mapping;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public abstract class GenericRepository<TEntity, TDomain, TUnitOfWork, TDbContext> : IGenericRepository<TDomain>
        where TEntity : class, IMappableEntity<TDomain>, new()
        where TUnitOfWork : UnitOfWork<TDbContext>
        where TDbContext : DbContext, new()
    {
        #region fields

        protected TUnitOfWork unitOfWork;
        protected Func<TEntity, TDomain, bool> finder;

        #endregion fields

        #region properties

        protected TDbContext DbContext { get { return this.unitOfWork.DbContext; } }
        protected DbSet<TEntity> DbSet { get { return this.DbContext.Set<TEntity>(); } }

        #endregion

        #region constructor

        /// <summary>
        /// Creates a new repositoriy who belongs to the specified unit of work and with the specified key finder
        /// </summary>
        /// <param name="dbContext">the dbContext of the underlying database</param>
        /// <param name="finder">a function to identify the entity from the domain</param>
        public GenericRepository(TUnitOfWork unitOfWork, Func<TEntity, TDomain, bool> finder)
        {
            this.unitOfWork = unitOfWork;
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

            this.DbSet.Add(entity);
        }

        public TDomain GetById(params object[] keyValues)
        {
            var entity = this.DbSet.Find(keyValues);

            if (entity != null && this.DbContext.Entry(entity).State != EntityState.Deleted)
                return entity.ToDomain();

            return default(TDomain);
        }

        public IList<TDomain> GetAll()
        {
            return this.DbSet.ToList()
                .Where(entity => this.DbContext.Entry(entity).State != EntityState.Deleted)
                .Select(entity => entity.ToDomain())
                .ToList();
        }

        public void Update(TDomain domain)
        {
            var entity = this.Attach(domain);

            this.DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(TDomain domain)
        {
            var entity = this.Attach(domain);

            this.DbSet.Remove(entity);
        }

        public void RemoveRange(IList<TDomain> list)
        {
            var entities = list.Select(domain => this.Attach(domain));

            this.DbSet.RemoveRange(entities);
        }

        #endregion

        #region other actions

        public int Count()
        {
            return this.DbSet.Count();
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
                this.DbSet.Attach(entity);

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
            var entity = this.DbSet.Local.SingleOrDefault(user => this.finder(user, domain));

            isAttached = entity != null;

            if (!isAttached)
                entity = new TEntity();

            entity.FillWith(domain);

            return entity;
        }

        #endregion
    }
}
