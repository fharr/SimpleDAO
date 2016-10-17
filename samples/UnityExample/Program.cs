﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityExample.DAL;
using UnityExample.DependencyResolver;
using UnityExample.Domain;

namespace UnityExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===============================");
            Console.WriteLine("====== --- SimpleDAO --- ======");
            Console.WriteLine("====== - Unity Example - ======");
            Console.WriteLine("===============================");
            Console.WriteLine();

            IUnitOfWork unitOfWork = Resolver.Resolve<IUnitOfWork>();

            var wantToLeave = false;

            while (!wantToLeave)
            {
                Console.WriteLine("Enter a number according to the associated action:");
                Console.WriteLine("\t-1. Create collection");
                Console.WriteLine("\t-2. List all collections");
                Console.WriteLine("\t-3. Update collection");
                Console.WriteLine("\t-4. Delete collection");
                Console.WriteLine("\t-5. Delete all collections");
                Console.WriteLine("\t-6. Create products");
                Console.WriteLine("\t-7. List all products");
                Console.WriteLine("\t-8. Update product");
                Console.WriteLine("\t-9. Delete product");
                Console.WriteLine("\t-0. Delete all products");
                Console.WriteLine("\t-X. Leave the program");
                Console.WriteLine();

                var key = Console.ReadKey(true);
                try
                {
                    switch (key.KeyChar)
                    {
                        case '1': CreateCollection(unitOfWork); break;
                        case '2': GetAllCollections(unitOfWork); break;
                        case '3': UpdateCollection(unitOfWork); break;
                        case '4': DeleteCollection(unitOfWork); break;
                        case '5': DeleteAllCollections(unitOfWork); break;
                        case '6': CreateProduct(unitOfWork); break;
                        case '7': GetAllProducts(unitOfWork); break;
                        case '8': UpdateProduct(unitOfWork); break;
                        case '9': DeleteProduct(unitOfWork); break;
                        case '0': DeleteAllProducts(unitOfWork); break;
                        case 'x':
                        case 'X': wantToLeave = true; break;
                        default: Console.WriteLine("Please, enter a valid command"); break;
                    }
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sorry, an error occured.");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    unitOfWork = Resolver.Resolve<IUnitOfWork>();
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("===============================");
            Console.WriteLine("====== --- SimpleDAO --- ======");
            Console.WriteLine("====== - Unity Example - ======");
            Console.WriteLine("===============================");

            Console.WriteLine("Press any key to finish...");
            Console.ReadKey();
        }

        #region manage collection

        private static void CreateCollection(IUnitOfWork uow)
        {
            Console.WriteLine("Create Collection!");
            Console.WriteLine("------------------");
            Console.WriteLine();

            Console.Write("Please, enter the name of your collection: ");
            var collectionName = Console.ReadLine();

            var collection = new CollectionDomain
            {
                Name = collectionName
            };

            uow.CollectionRepository.Create(collection);
            uow.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Collection successfully ceated.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void DeleteCollection(IUnitOfWork uow)
        {
            Console.WriteLine("Delete Collection!");
            Console.WriteLine("------------------");
            Console.WriteLine();

            Console.Write("Please, enter the id of collection you want to delete: ");
            var collectionId = int.Parse(Console.ReadLine());

            var collection = uow.CollectionRepository.GetById(collectionId);

            uow.CollectionRepository.Remove(collection);
            uow.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Collection successfully deleted.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void DeleteAllCollections(IUnitOfWork uow)
        {
            Console.WriteLine("Delete all collections!");
            Console.WriteLine("------------------");
            Console.WriteLine();

            var collections = uow.CollectionRepository.GetAll();

            uow.CollectionRepository.RemoveRange(collections);
            uow.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Collections successfully deleted.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void UpdateCollection(IUnitOfWork uow)
        {
            Console.WriteLine("Update Collection!");
            Console.WriteLine("------------------");
            Console.WriteLine();

            Console.Write("Please, enter the id of collection you want to update: ");
            var collectionId = int.Parse(Console.ReadLine());

            var collection = uow.CollectionRepository.GetById(collectionId);

            Console.WriteLine();
            Console.Write("Please, enter the new name of the collection you want to update: ");
            var collectionName = Console.ReadLine();

            collection.Name = collectionName;

            uow.CollectionRepository.Update(collection);
            uow.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Collection successfully updated.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void GetAllCollections(IUnitOfWork uow)
        {
            Console.WriteLine("List of all Collections:");
            Console.WriteLine("------------------------");
            Console.WriteLine();

            var collections = uow.CollectionRepository.GetAll();

            foreach(var collection in collections)
            {
                Console.WriteLine("- Id: {0}, Name: {1}, nb products: {2}", collection.Id, collection.Name, collection.NbProducts);
            }

            if (collections.Count == 0)
                Console.WriteLine("[No collection]");
        }

        #endregion

        #region manage product

        private static void CreateProduct(IUnitOfWork uow)
        {
            Console.WriteLine("Create Product!");
            Console.WriteLine("---------------");
            Console.WriteLine();

            Console.Write("Please, enter the name of your product: ");
            var productName = Console.ReadLine();

            Console.Write("Please, enter the id of the related collection: ");
            var collectionIdStr = Console.ReadLine();
            var collectionId = int.Parse(collectionIdStr);

            var product = new ProductDomain
            {
                Name = productName,
                CollectionId = collectionId
            };

            uow.ProductRepository.Create(product);
            uow.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Product successfully ceated.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void DeleteProduct(IUnitOfWork uow)
        {
            Console.WriteLine("Delete Product!");
            Console.WriteLine("------------------");
            Console.WriteLine();

            Console.Write("Please, enter the id of product you want to delete: ");
            var productId = int.Parse(Console.ReadLine());

            var product = uow.ProductRepository.GetById(productId);

            uow.ProductRepository.Remove(product);
            uow.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Product successfully deleted.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void DeleteAllProducts(IUnitOfWork uow)
        {
            Console.WriteLine("Delete all product!");
            Console.WriteLine("------------------");
            Console.WriteLine();

            Console.WriteLine("Please enter the collection id of the products you want to delete (leave empty to delete all products): ");
            var collectionIdStr = Console.ReadLine();

            var products = uow.ProductRepository.GetAll();

            if (!String.IsNullOrWhiteSpace(collectionIdStr))
            {
                products = products.Where(product => product.CollectionId == int.Parse(collectionIdStr)).ToList();
            }

            uow.ProductRepository.RemoveRange(products);
            uow.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Products successfully deleted.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void UpdateProduct(IUnitOfWork uow)
        {
            Console.WriteLine("Update Product!");
            Console.WriteLine("---------------");
            Console.WriteLine();

            Console.Write("Please, enter the id of product you want to update: ");
            var productId = int.Parse(Console.ReadLine());

            var product = uow.ProductRepository.GetById(productId);

            Console.WriteLine();
            Console.Write("Please, enter the new name of the product you want to update: ");
            var productName = Console.ReadLine();

            product.Name = productName;

            uow.ProductRepository.Update(product);
            uow.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Product successfully updated.");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void GetAllProducts(IUnitOfWork uow)
        {
            Console.WriteLine("List of all Products:");
            Console.WriteLine("---------------------");
            Console.WriteLine();

            var products = uow.ProductRepository.GetAll();

            foreach (var product in products)
            {
                Console.WriteLine("- Id: {0}, Name: {1}, Collection name: {2}", product.Id, product.Name, product.CollectionName);
            }

            if (products.Count == 0)
                Console.WriteLine("[No product]");
        }

        #endregion
    }
}
