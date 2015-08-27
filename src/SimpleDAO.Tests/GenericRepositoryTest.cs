using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Data;
using SimpleDAO.Tests.Domain;
using SimpleDAO.Tests.DAL;

namespace SimpleDAO.Tests
{
    [TestClass]
    public abstract class GenericRepositoryTest<TUnitOfWork>
        where TUnitOfWork : ITestUnitOfWork, new()
    {
        public SqlConnection Connection { get; set; }
        public ITestUnitOfWork UnitOfWork { get; set; }

        public int CurrentCollectionId { get; set; }
        public int BaseProductId { get; set; }
        public int NbProducts { get; set; }

        public GenericRepositoryTest()
        {
            this.UnitOfWork = new TUnitOfWork();
        }

        #region private CRUD methods

        private void Create(CollectionDomain domain)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = String.Format("INSERT INTO Collection VALUES ({0}, '{1}')", domain.Id, domain.Name);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = this.Connection;

            var nbRow = cmd.ExecuteNonQuery();
        }

        private void Create(ProductDomain domain)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = String.Format("INSERT INTO Product VALUES ({0}, '{1}', {2}, {3})", domain.Id, domain.Name, domain.Price, domain.CollectionId);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = this.Connection;

            var nbRow = cmd.ExecuteNonQuery();
        }

        private int GetNextId(string table)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "SELECT ISNULL(MAX(Id), 0) FROM " + table;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = this.Connection;

            var id = (int)cmd.ExecuteScalar();

            return ++id;
        }

        private CollectionDomain GetCollectionById(int id)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT * FROM Collection WHERE Id = " + id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = this.Connection;

            reader = cmd.ExecuteReader();
            
            var domain = new CollectionDomain();

            if (reader.Read())
            {                 
                domain.Id = reader.GetInt32(0);
                domain.Name = reader.GetString(1);

                return domain;
            }

            return null;
        }

        private ProductDomain GetProductById(int id)
        {
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT * FROM Product WHERE Id = " + id;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = this.Connection;

            reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                var domain = new ProductDomain();

                domain.Id = reader.GetInt32(0);
                domain.Name = reader.GetString(1);
                domain.Price = reader.GetDecimal(2);

                return domain;
            }

            return null;
        }

        private void Delete(string table, int id)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = String.Format("DELETE {0} WHERE Id = {1}", table, id);
            cmd.CommandType = CommandType.Text;
            cmd.Connection = this.Connection;

            cmd.ExecuteNonQuery();
        }

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            this.Connection = new SqlConnection(@"data source=(localdb)\ProjectsV12;initial catalog=SimpleDAO.Tests.Database;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework");
            this.Connection.Open();

            this.CurrentCollectionId = this.GetNextId("Collection");
            this.BaseProductId = this.GetNextId("Product");
            this.NbProducts = 5;

            var collection = new CollectionDomain
            {
                Id = this.CurrentCollectionId,
                Name = "Collection"
            };

            this.Create(collection);

            for(int i = 0; i< this.NbProducts; i++)
            {
                var product = new ProductDomain
                {
                    Id = this.BaseProductId + i,
                    CollectionId = this.CurrentCollectionId,
                    Name = "Product " + i,
                    Price = i * 5
                };

                this.Create(product);
            }
        }

        [TestMethod]
        public void TestInsertDelete()
        {

        }

        [TestMethod]
        public void TestGet()
        {
            // Collection Test,  object not cached
            var collectionFromRepo = this.UnitOfWork.CollectionRepository.GetById(this.CurrentCollectionId);
            var collectionFromAdo = this.GetCollectionById(this.CurrentCollectionId);

            Assert.AreEqual(collectionFromAdo.Id, collectionFromRepo.Id, "Get collection not cached: Wrong Id");
            Assert.AreEqual(collectionFromAdo.Name, collectionFromRepo.Name, "Get collection not cached: Wrong Name");
            Assert.AreEqual(this.NbProducts, collectionFromRepo.NbProducts, "Get collection not cached: Wrong number of products");

            // Collection Test,  object already cached
            collectionFromRepo = this.UnitOfWork.CollectionRepository.GetById(this.CurrentCollectionId);

            Assert.AreEqual(collectionFromAdo.Id, collectionFromRepo.Id, "Get collection already cached: Wrong Id");
            Assert.AreEqual(collectionFromAdo.Name, collectionFromRepo.Name, "Get collection already cached: Wrong Name");
            Assert.AreEqual(this.NbProducts, collectionFromRepo.NbProducts, "Get collection already cached: Wrong number of products");

            // Product Tests, object not cached
            var productFromRepo = this.UnitOfWork.ProductRepository.GetById(this.BaseProductId);
            var productFromAdo = this.GetProductById(this.BaseProductId);

            Assert.AreEqual(productFromAdo.Id, productFromRepo.Id, "Get product not cached: Wrong Id");
            Assert.AreEqual(productFromAdo.Name, productFromRepo.Name, "Get product not cached: Wrong Name");
            Assert.AreEqual(collectionFromAdo.Name, productFromRepo.CollectionName, "Get product not cached: Wrong collection's name");

            // Product Tests, object not cached
            productFromRepo = this.UnitOfWork.ProductRepository.GetById(this.BaseProductId);

            Assert.AreEqual(productFromAdo.Id, productFromRepo.Id, "Get product already cached: Wrong Id");
            Assert.AreEqual(productFromAdo.Name, productFromRepo.Name, "Get product already cached: Wrong Name");
            Assert.AreEqual(collectionFromAdo.Name, productFromRepo.CollectionName, "Get product already cached: Wrong collection's name");
        }

        [TestMethod]
        public void TestGetAll()
        {

        }

        [TestMethod]
        public void TestUpdate()
        {

        }

        [TestMethod]
        public void TestRemoveAll()
        {

        }

        [TestCleanup]
        public void TestCleanup()
        {
            for(int i = 0; i < this.NbProducts; i++)
            {
                this.Delete("Product", this.BaseProductId + i);
            }

            this.Delete("Collection", this.CurrentCollectionId);

            this.Connection.Dispose();
        }
    }
}
