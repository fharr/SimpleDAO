namespace SimpleDAO
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Commits all the changes made in this context to the underlying datastore
        /// </summary>
        void SaveChanges();
    }
}
