namespace SimpleDAO.EFCore.Mapping
{
    using AutoMapper;

    public abstract class AutoMappableEntity<TEntity, TDomain> : IMappableEntity<TDomain>
    {
        // AutoMapper's Initialization
        static AutoMappableEntity()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TEntity, TDomain>());
            Mapper.Initialize(cfg => cfg.CreateMap<TDomain, TEntity>());
        }

        public void FillWith(TDomain domain)
        {
            BeforeFilling(domain);
            Mapper.Map(domain, this);
            AfterFilling(domain);
        }

        public TDomain ToDomain()
        {
            var domain = Mapper.Map<TDomain>(this);
            OnConvert(domain);
            return domain;
        }

        #region protected methods

        protected virtual void AfterFilling(TDomain domain)
        { }

        protected virtual void BeforeFilling(TDomain domain)
        { }

        protected virtual void OnConvert(TDomain domain)
        { }

        #endregion
    }
}
