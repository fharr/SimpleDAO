namespace SimpleDAO
{
    using System.Collections.Generic;

    public interface IGenericRepository<TDomain>
    {
        /// <summary>
        /// Creates a new object
        /// </summary>
        /// <param name="domain">the object to create</param>
        void Create(TDomain domain);

        /// <summary>
        /// Gets an existing object by its id
        /// </summary>
        /// <param name="keyValues">the object's id</param>
        /// <returns></returns>
        TDomain GetById(params object[] keyValues);

        /// <summary>
        /// Gets all the object
        /// </summary>
        /// <returns></returns>
        IList<TDomain> GetAll();

        /// <summary>
        /// Updates the specified object
        /// </summary>
        /// <param name="domain">the object to update</param>
        void Update(TDomain domain);

        /// <summary>
        /// Deletes the specified object
        /// </summary>
        /// <param name="domain">the object to delete</param>
        void Remove(TDomain domain);

        /// <summary>
        /// Deletes all the objects in the specified list
        /// </summary>
        /// <param name="list">the objects to delete</param>
        void RemoveRange(IList<TDomain> list);

        /// <summary>
        /// Counts how many objects exist in the underlyning datastore
        /// </summary>
        /// <returns>the number of object</returns>
        int Count();
    }
}
