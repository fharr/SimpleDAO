namespace SimpleDAO.EF6.Exceptions
{
    using System;

    public class AlreadyExistingException<TDomain> : Exception
    {
        public TDomain Domain { get; set; }

        public AlreadyExistingException(TDomain domain)
            : base("An object with that key already exists and cannot be created")
        {
            this.Domain = domain;
        }
    }
}
