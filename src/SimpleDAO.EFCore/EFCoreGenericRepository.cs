namespace SimpleDAO.EFCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Mapping;
    using Exceptions;

    public abstract class EFCoreGenericRepository<TEntity, TDomain, TUnitOfWork, TDbContext> : IGenericRepository<TDomain>
        where TEntity : class, IMappableEntity<TDomain>, new()
        where TUnitOfWork : EFCoreUnitOfWork<TDbContext>
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
        public EFCoreGenericRepository(TUnitOfWork unitOfWork, Func<TEntity, TDomain, bool> finder)
        {
            this.unitOfWork = unitOfWork;
            this.finder = finder;
        }

        #endregion

        #region CRUD actions

        public void Create(TDomain domain)
        {
            bool exists;

            var entity = this.ToEntity(domain, out exists);

            if (exists)
            {
                throw new AlreadyExistingException<TDomain>(domain);
            }

            this.DbSet.Add(entity);
        }

        public abstract TDomain GetById(params object[] keyValues);

        public IList<TDomain> GetAll()
        {
            return this.DbSet.ToList()
                .Where(entity => this.DbContext.Entry(entity).State != EntityState.Deleted)
                .Select(entity => entity.ToDomain())
                .ToList();
        }

        public void Update(TDomain domain)
        {
            bool exists;

            var entity = this.ToEntity(domain, out exists);

            if (!exists)
            {
                throw new NotExistingException<TDomain>(domain);
            }
        }

        public void Remove(TDomain domain)
        {
            bool exists;

            var entity = this.ToEntity(domain, out exists);

            if (!exists)
            {
                throw new NotExistingException<TDomain>(domain);
            }

            this.DbSet.Remove(entity);
        }

        public void RemoveRange(IList<TDomain> list)
        {
            foreach (var domain in list)
            {
                this.Remove(domain);
            }
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
        /// Converts the specified domain into an entity
        /// </summary>
        /// <param name="domain">the domain to convert</param>
        /// <param name="exists">output parameter to know if the entity already exists in the database</param>
        /// <returns></returns>
        protected TEntity ToEntity(TDomain domain, out bool exists)
        {
            var entity = this.DbSet.FirstOrDefault(e => this.finder(e, domain));

            exists = entity != null;

            if (!exists)
                entity = new TEntity();

            entity.FillWith(domain);

            return entity;
        }

        #endregion
    }
}
