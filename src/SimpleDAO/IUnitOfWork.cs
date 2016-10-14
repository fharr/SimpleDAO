namespace SimpleDAO
{
    using System;

    /// <summary>
    /// This interface controls the application of the changes made to all its repositories.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the underlying datastore
        /// </summary>
        IDataStore DataStore { get; }

        /// <summary>
        /// Commits all the changes made in this context to the underlying datastore
        /// </summary>
        void SaveChanges();
    }
}
