using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityExample.DAL;
using UnityExample.DAL.EntityFramework;
using UnityExample.Domain;

namespace UnityExample
{
    class Program
    {
        public static IUnityContainer Container { get; set; }

        static Program()
        {
            Program.Container = new UnityContainer()
                .RegisterType<IUnitOfWork, UnitOfWork>()
                .RegisterType<ICollectionRepository, CollectionRepository>()
                .RegisterType<IProductRepository, ProductRepository>();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("===============================");
            Console.WriteLine("====== --- SimpleDAO --- ======");
            Console.WriteLine("====== - Unity Example - ======");
            Console.WriteLine("===============================");
            Console.WriteLine();

            var unitOfWork = Program.Container.Resolve<IUnitOfWork>();

            Console.WriteLine("List of existing collections:");

            var existingCollections = unitOfWork.CollectionRepository.GetAll();

            foreach(var collec in existingCollections)
            {
                Console.WriteLine(" - " + collec.Name);
            }

            if (existingCollections.Count == 0)
                Console.WriteLine(" [no existing collections]");

            Console.WriteLine();
            Console.WriteLine("Deletion of all existing collections:");

            unitOfWork.CollectionRepository.RemoveRange(existingCollections);
            unitOfWork.SaveChanges();

            Console.WriteLine("Collections successfully deleted!");

            Console.WriteLine();
            Console.WriteLine("Creation of the Card collection:");

            var collection = new CollectionDomain
            {
                Id = 1,
                Name = "Card"
            };

            unitOfWork.CollectionRepository.Create(collection);
            unitOfWork.SaveChanges();

            Console.WriteLine("Collection successfully created!");

            Console.WriteLine();
            Console.WriteLine("===============================");
            Console.WriteLine("====== --- SimpleDAO --- ======");
            Console.WriteLine("====== - Unity Example - ======");
            Console.WriteLine("===============================");

            Console.WriteLine("Press any key to finish...");
            Console.ReadKey();
        }
    }
}
