#if NET451
namespace SimpleDAO.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using DAL.EF6;

    [TestClass]
    public class EntityFrameworkRepositoryTest : GenericRepositoryTest<TestUnitOfWork>
    { }
}
#endif