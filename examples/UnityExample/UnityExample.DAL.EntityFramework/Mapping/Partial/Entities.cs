using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityExample.DAL.EntityFramework.Mapping
{
    partial class Entities
    {
        //workaround to force sqlserver dll to be copied into the main project
        private static SqlProviderServices instance = SqlProviderServices.Instance;
    }
}
