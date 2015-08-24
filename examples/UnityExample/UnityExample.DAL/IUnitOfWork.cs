using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityExample.DAL
{
    public interface IUnitOfWork : SimpleDAO.IUnitOfWork
    {
        ICollectionRepository CollectionRepository { get; }
        IProductRepository ProductRepository { get; }
    }
}
