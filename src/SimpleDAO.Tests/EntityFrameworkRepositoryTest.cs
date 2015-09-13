namespace SimpleDAO.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using DAL.EntityFramework;

    [TestClass]
    public class EntityFrameworkRepositoryTest : GenericRepositoryTest<EFTestUnitOfWork>
    { }
}
