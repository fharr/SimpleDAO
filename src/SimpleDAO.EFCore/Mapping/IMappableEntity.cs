namespace SimpleDAO.EFCore.Mapping
{
    public interface IMappableEntity<TDomain>
    {
        /// <summary>
        /// Fills the entity properties with the properties of the provided object
        /// </summary>
        /// <param name="domain">the object to fill from</param>
        void FillWith(TDomain domain);

        /// <summary>
        /// Converts the entity into a domain object
        /// </summary>
        /// <returns></returns>
        TDomain ToDomain();
    }
}
