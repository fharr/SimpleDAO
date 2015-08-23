using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDAO.EntityFramework.Exceptions
{
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
