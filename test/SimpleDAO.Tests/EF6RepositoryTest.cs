#if NET451
namespace SimpleDAO.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using DAL.EF6;

    [TestClass]
    public class EF6RepositoryTest : GenericRepositoryTest<TestUnitOfWork>
    { }
}
#endif