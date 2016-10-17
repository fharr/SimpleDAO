namespace SimpleDAO.Tests
{
    using DAL.EFCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EFCoreRepositoryTest : GenericRepositoryTest<TestEFCoreUnitOfWork>
    { }
}