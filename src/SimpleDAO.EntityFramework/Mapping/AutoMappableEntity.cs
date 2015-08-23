namespace SimpleDAO.EntityFramework.Mapping
{
    using AutoMapper;

    public abstract class AutoMappableEntity<TEntity, TDomain> : IMappableEntity<TDomain>
    {
        // AutoMapper's Initialization
        static AutoMappableEntity()
        {
            Mapper.CreateMap<TEntity, TDomain>();
            Mapper.CreateMap<TDomain, TEntity>();
        }

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
