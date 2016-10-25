namespace SimpleDAO.EFCore.Exceptions
{
    using System;

    public class NotExistingException<TDomain> : Exception
    {
        public TDomain Domain { get; set; }

        public NotExistingException(TDomain domain)
            : base("No object with that key exists.")
        {
            this.Domain = domain;
        }
    }
}
