using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDAO.Tests.DAL
{
    public interface ITestUnitOfWork : IUnitOfWork
    {
        IProductRepository ProductRepository { get; set; }
        ICollectionRepository CollectionRepository { get; set; }
    }
}
