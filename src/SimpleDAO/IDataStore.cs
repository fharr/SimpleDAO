namespace SimpleDAO
{
    /// <summary>
    /// This interface enables to manage the underlying datastore
    /// </summary>
    public interface IDataStore
    {
        /// <summary>
        /// Gets a value indicating whether the datastore exists
        /// </summary>
        /// <returns>true if a data store exists; otherwise false</returns>
        bool Exists();

        /// <summary>
        /// Instantiates a new datastore if it does not exist
        /// </summary>
        void CreateIfNotExists();

        /// <summary>
        /// Gets a value indicating whether the underlying datastore respects the domain format
        /// </summary>
        /// <returns>true is the data store is valid; otherwise false</returns>
        bool IsValid();

        /// <summary>
        /// Delete the current datastore
        /// </summary>
        void Delete();
    }
}
