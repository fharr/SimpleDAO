using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleDAO.Tests.DAL;
using SimpleDAO.Tests.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

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
            this.Connection = new SqlConnection(@"data source=(localdb)\MSSQLLocalDB;initial catalog=SimpleDAO.Tests.Database;integrated security=True;MultipleActiveResultSets=True;");
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
            // Tests on cached object
            var newCollection = new CollectionDomain
            {
                Id = this.CurrentCollectionId + 1,
                Name = "New Collec"
            };

            this.UnitOfWork.CollectionRepository.Create(newCollection);

            var collectionFromAdo = this.GetCollectionById(newCollection.Id);

            Assert.IsNull(collectionFromAdo, "Create: Collection created before call to SaveChanges.");

            this.UnitOfWork.SaveChanges();

            collectionFromAdo = this.GetCollectionById(newCollection.Id);

            Assert.IsNotNull(collectionFromAdo, "Create: Collection not created after a call to SaveChanges.");
            Assert.AreEqual(newCollection.Id, collectionFromAdo.Id, "Create: Wrong Collection Id");
            Assert.AreEqual(newCollection.Name, collectionFromAdo.Name, "Create: Wrong Collection Name");

            this.UnitOfWork.CollectionRepository.Remove(newCollection);

            collectionFromAdo = this.GetCollectionById(newCollection.Id);
            var collectionFromRepo = this.UnitOfWork.CollectionRepository.GetById(newCollection.Id);
            var collectionListFromRepo = this.UnitOfWork.CollectionRepository.GetAll();

            Assert.IsNotNull(collectionFromAdo, "Delete: Ado Collection deleted before call to SaveChanges.");
            Assert.IsNull(collectionFromRepo, "Delete: Repo Collection not deleted from cache before call to SaveChanges.");
            Assert.IsFalse(collectionListFromRepo.Any(collec => collec.Id == newCollection.Id), "Delete: Repo Collection not deleted from cache before call to SaveChanges.");

            this.UnitOfWork.SaveChanges();

            collectionFromAdo = this.GetCollectionById(newCollection.Id);
            collectionFromRepo = this.UnitOfWork.CollectionRepository.GetById(newCollection.Id);
            collectionListFromRepo = this.UnitOfWork.CollectionRepository.GetAll();

            Assert.IsNull(collectionFromAdo, "Delete: Ado Collection not deleted after call to SaveChanges.");
            Assert.IsNull(collectionFromRepo, "Delete: Repo Collection not deleted after call to SaveChanges.");
            Assert.IsFalse(collectionListFromRepo.Any(collec => collec.Id == newCollection.Id), "Delete: Collection not deleted after call to SaveChanges.");

            // Tests on not cached object
            var notCachedObject = new ProductDomain
            {
                Id = this.BaseProductId
            };

            this.UnitOfWork.ProductRepository.Remove(notCachedObject);

            var productFromAdo = this.GetProductById(notCachedObject.Id);
            var productFromRepo = this.UnitOfWork.ProductRepository.GetById(notCachedObject.Id);
            var productListFromRepo = this.UnitOfWork.ProductRepository.GetAll();

            Assert.IsNotNull(productFromAdo, "Delete: Ado Product not cached deleted before a call to SaveChanges.");
            Assert.IsNull(productFromRepo, "Delete: Repo Product not cached not deleted from cache before a call to SaveChanges.");
            Assert.IsFalse(productListFromRepo.Any(product => product.Id == notCachedObject.Id), "Delete: Repo Product not cached not deleted from cache before call to SaveChanges.");

            this.UnitOfWork.SaveChanges();

            productFromRepo = this.UnitOfWork.ProductRepository.GetById(notCachedObject.Id);
            productFromAdo = this.GetProductById(notCachedObject.Id);
            productListFromRepo = this.UnitOfWork.ProductRepository.GetAll();

            Assert.IsNull(productFromRepo, "Delete: Repo Product not cached not deleted after a call to SaveChanges.");
            Assert.IsNull(productFromAdo, "Delete: Repo Product not cached not deleted after a call to SaveChanges.");
            Assert.IsFalse(productListFromRepo.Any(product => product.Id == notCachedObject.Id), "Delete: Repo Product not cached not deleted after call to SaveChanges.");
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
            // Tests on not cached objects
            var products = this.UnitOfWork.ProductRepository.GetAll();

            Assert.IsTrue(products.Count() >= this.NbProducts);

            for(int i = 0; i < this.NbProducts; i++)
            {
                var product = products.FirstOrDefault(prod => prod.Id == this.BaseProductId + i);

                Assert.IsNotNull(product, "GetAll: Product not retrieved, index: " + i);
            }

            // Tests on cached objects
            products = this.UnitOfWork.ProductRepository.GetAll();

            Assert.IsTrue(products.Count() >= this.NbProducts);

            for (int i = 0; i < this.NbProducts; i++)
            {
                var product = products.FirstOrDefault(prod => prod.Id == this.BaseProductId + i);

                Assert.IsNotNull(product, "GetAll: Product not retrieved, index: " + i);
            }
        }

        [TestMethod]
        public void TestUpdate()
        {
            // Tests on not cached object
            var collectionEdit = new CollectionDomain
            {
                Id = this.CurrentCollectionId,
                Name = "New Collec"
            };
            var originalCollection = this.GetCollectionById(this.CurrentCollectionId);

            this.UnitOfWork.CollectionRepository.Update(collectionEdit);

            var collectionFromAdo = this.GetCollectionById(this.CurrentCollectionId);
            var collectionFromRepo = this.UnitOfWork.CollectionRepository.GetById(this.CurrentCollectionId);

            Assert.AreEqual(originalCollection.Name, collectionFromAdo.Name, "Update: Ado Collection not cached updated before call to SaveChanges.");
            Assert.AreNotEqual(originalCollection.Name, collectionFromRepo.Name, "Update: Repo Collection not cached not updated in cache before call to SaveChanges.");

            this.UnitOfWork.SaveChanges();

            collectionFromAdo = this.GetCollectionById(this.CurrentCollectionId);
            collectionFromRepo = this.UnitOfWork.CollectionRepository.GetById(this.CurrentCollectionId);

            Assert.AreEqual(collectionEdit.Name, collectionFromAdo.Name, "Update: Ado Collection not cached not updated after call to SaveChanges.");
            Assert.AreEqual(collectionEdit.Name, collectionFromRepo.Name, "Update: Repo Collection not cached not updated after call to SaveChanges.");

            // Tests on cached object
            collectionEdit.Name = "Bis Collec";

            originalCollection = this.GetCollectionById(this.CurrentCollectionId);

            this.UnitOfWork.CollectionRepository.Update(collectionEdit);

            collectionFromAdo = this.GetCollectionById(this.CurrentCollectionId);
            collectionFromRepo = this.UnitOfWork.CollectionRepository.GetById(this.CurrentCollectionId);

            Assert.AreEqual(originalCollection.Name, collectionFromAdo.Name, "Update: Ado Collection updated before call to SaveChanges.");
            Assert.AreNotEqual(originalCollection.Name, collectionFromRepo.Name, "Update: Repo Collection not updated in cache before call to SaveChanges.");

            this.UnitOfWork.SaveChanges();

            collectionFromAdo = this.GetCollectionById(this.CurrentCollectionId);
            collectionFromRepo = this.UnitOfWork.CollectionRepository.GetById(this.CurrentCollectionId);

            Assert.AreEqual(collectionEdit.Name, collectionFromAdo.Name, "Update: Ado Collection not updated after call to SaveChanges.");
            Assert.AreEqual(collectionEdit.Name, collectionFromRepo.Name, "Update: Repo Collection not updated after call to SaveChanges.");
        }

        [TestMethod]
        public void TestRemoveRange()
        {
            // Tests on not cached object
            var productsToRemove = new List<ProductDomain>();

            for (int i = 0; i < this.NbProducts / 2; i++)
            {
                productsToRemove.Add(new ProductDomain
                {
                    Id = this.BaseProductId + i
                });
            }

            this.UnitOfWork.ProductRepository.RemoveRange(productsToRemove);

            var productListFromRepo = this.UnitOfWork.ProductRepository.GetAll();

            for (int i = 0; i < this.NbProducts / 2; i++)
            {
                var productFromAdo = this.GetProductById(this.BaseProductId + i);
                var productFromRepo = this.UnitOfWork.ProductRepository.GetById(this.BaseProductId + i);

                Assert.IsNotNull(productFromAdo, "RemoveRange: Ado product not cached deleted before call to SaveChanges.");
                Assert.IsNull(productFromRepo, "RemoveRange: Ado product not cached not deleted from cache before call to SaveChanges.");
                Assert.IsFalse(productListFromRepo.Any(prod => prod.Id == this.BaseProductId + i), "RemoveRange: Product not cached not deleted from cache before call to SaveChanges.");
            }

            this.UnitOfWork.SaveChanges();

            productListFromRepo = this.UnitOfWork.ProductRepository.GetAll();

            for (int i = 0; i < this.NbProducts / 2; i++)
            {
                var productFromAdo = this.GetProductById(this.BaseProductId + i);
                var productFromRepo = this.UnitOfWork.ProductRepository.GetById(this.BaseProductId + i);

                Assert.IsNull(productFromAdo, "RemoveRange: Ado product not cached not deleted after call to SaveChanges.");
                Assert.IsNull(productFromRepo, "RemoveRange: Ado product not cached not after call to SaveChanges.");
                Assert.IsFalse(productListFromRepo.Any(prod => prod.Id == this.BaseProductId + i), "RemoveRange: Product not cached not deleted after call to SaveChanges.");
            }

            // Tests on cached object
            productsToRemove = new List<ProductDomain>();

            for (int i = this.NbProducts / 2; i < this.NbProducts; i++)
            {
                productsToRemove.Add(this.UnitOfWork.ProductRepository.GetById(this.BaseProductId + i));
            }

            this.UnitOfWork.ProductRepository.RemoveRange(productsToRemove);

            productListFromRepo = this.UnitOfWork.ProductRepository.GetAll();

            for (int i = this.NbProducts / 2; i < this.NbProducts; i++)
            {
                var productFromAdo = this.GetProductById(this.BaseProductId + i);
                var productFromRepo = this.UnitOfWork.ProductRepository.GetById(this.BaseProductId + i);

                Assert.IsNotNull(productFromAdo, "RemoveRange: Ado product deleted before call to SaveChanges.");
                Assert.IsNull(productFromRepo, "RemoveRange: Ado product not deleted from cache before call to SaveChanges.");
                Assert.IsFalse(productListFromRepo.Any(prod => prod.Id == this.BaseProductId + i), "RemoveRange: Product not deleted from cache before call to SaveChanges.");
            }

            this.UnitOfWork.SaveChanges();

            productListFromRepo = this.UnitOfWork.ProductRepository.GetAll();

            for (int i = this.NbProducts / 2; i < this.NbProducts; i++)
            {
                var productFromAdo = this.GetProductById(this.BaseProductId + i);
                var productFromRepo = this.UnitOfWork.ProductRepository.GetById(this.BaseProductId + i);

                Assert.IsNull(productFromAdo, "RemoveRange: Ado product not deleted after call to SaveChanges.");
                Assert.IsNull(productFromRepo, "RemoveRange: Ado product not after call to SaveChanges.");
                Assert.IsFalse(productListFromRepo.Any(prod => prod.Id == this.BaseProductId + i), "RemoveRange: Product not deleted after call to SaveChanges.");
            }
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