namespace SimpleDAO.EF6.Mapping
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

        protected static void Initialize()
        { }

        public void FillWith(TDomain domain)
        {
            Mapper.Map(domain, this);
        }

        public TDomain ToDomain()
        {
            return Mapper.Map<TDomain>(this);
        }
    }
}
