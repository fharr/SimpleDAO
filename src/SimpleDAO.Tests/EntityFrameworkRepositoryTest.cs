using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleDAO.Tests.DAL.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDAO.Tests
{
    [TestClass]
    public class EntityFrameworkRepositoryTest : GenericRepositoryTest<EFTestUnitOfWork>
    { }
}
