using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityExample.Domain
{
    public class ProductDomain
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CollectionId { get; set; }
    }
}
